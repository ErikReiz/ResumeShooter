using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FPCharacter : MonoBehaviour, IDamageable
{
	#region SERIALIZE FIELDS
	[Tooltip("Range at which character will be able to interact with objects")]
	[SerializeField] private float interactionRange = 30f;
	[SerializeField] private float maxHealth = 100f;
	[Tooltip("Player object for weapon")]
	[SerializeField] private WeaponCarrier weaponCarrier;
	#endregion

	#region PROPERTIES
	public AmmoManager AmmoManager { get { return ammoManager; } }
	public Vector3 CameraForwardVector { get { return playerCamera.transform.forward; } }
	public int WeaponCurrentAmmo { get { return currentWeapon.CurrentAmmo; } }
	public int WeaponGeneralAmmo { get { return currentWeapon.GeneralAmmo; } }
	public float HealthPercents { get { return currentHealth / maxHealth; } }
	#endregion

	#region FIELDS
	private UnityAction unholsterAction;

	private PlayerAnimationManager playerAnimation;
	private Camera playerCamera;
	private Weapon currentWeapon;
	private WeaponPickUp pickedUpEquipment;
	private AmmoManager ammoManager;
	private PlayerController playerMovement;
	private PlayerInput input;
	private float currentHealth;
	private bool isDead = false;
	private bool holstered = true;
	private bool isMouseScrollUp = false;
	private bool isSprinting = false;

	#endregion

	private void Awake()
	{
		SetupInput();

		currentHealth = maxHealth;

		playerCamera = GetComponentInChildren<Camera>();
		ammoManager = GetComponent<AmmoManager>();
		playerMovement = GetComponent<PlayerController>();
		playerAnimation = GetComponentInChildren<PlayerAnimationManager>();
		playerAnimation.OnEndedHolster += OnEndedHolster;
	}

	private void Start()
	{
		SetCurrentWeapon();
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnEnable()
	{
		input.Player.Enable();
	}

	private void OnDisable()
	{
		input.Player.Disable();
	}

	private void SetupInput()
	{
		input = new PlayerInput();

		input.Player.Movement.started += OnMovementInput;
		input.Player.Movement.performed += OnMovementInput;
		input.Player.Movement.canceled += OnMovementInput;

		input.Player.Run.started += OnSprintInput;
		input.Player.Run.canceled += OnSprintInput;

		input.Player.Look.started += OnMouseInput;
		input.Player.Look.performed += OnMouseInput;
		input.Player.Look.canceled += OnMouseInput;

		input.Player.Fire.started += OnFireInput;
		input.Player.Fire.canceled += OnFireInput;

		input.Player.Reload.started += OnReloadInput;
		input.Player.SwitchWeapon.started += OnSwitchWeaponInput;
		input.Player.Interact.started += OnInteractInput;
	}

	#region INPUT
	private void OnMovementInput(InputAction.CallbackContext context)
	{
		Vector2 movementInput = context.ReadValue<Vector2>();
		playerMovement.ReceiveMovementInput(movementInput);
		playerAnimation.ReceiveMovementInput(movementInput);
	}

	private void OnSprintInput(InputAction.CallbackContext context)
	{
		isSprinting = context.phase == InputActionPhase.Started ? true : false;
		playerMovement.ReceiveSprintingInput(isSprinting);
		playerAnimation.ToogleSprint(isSprinting);
	}

	private void OnMouseInput(InputAction.CallbackContext context)
	{
		playerMovement.ReceiveMouseInput(context.ReadValue<Vector2>());
	}

	private void OnFireInput(InputAction.CallbackContext context)
	{
		if (IsWeaponActionBlocked()) { return; }

		switch (context.phase)
		{
			case InputActionPhase.Started:
				currentWeapon.StartFire();
				break;
			case InputActionPhase.Canceled:
				currentWeapon.StopFire();
				break;
		}
	}

	private void OnReloadInput(InputAction.CallbackContext context)
	{
		if (IsWeaponActionBlocked()) { return; }

		currentWeapon.StartReloading();
	}

	private void OnSwitchWeaponInput(InputAction.CallbackContext context)
	{
		if(holstered) { return; }
		if (!weaponCarrier.CanChangeWeapon()) { return; }

		if (currentWeapon?.CanChangeWeapon() ?? true)
		{
			isMouseScrollUp = Input.GetAxis("SwitchWeapon") > 0 ? true : false;
			unholsterAction = new UnityAction(SwitchWeapon);
			playerAnimation.PlayerHolsterAnimation(holstered);
		}
	}

	private void OnInteractInput(InputAction.CallbackContext context)
	{
		RaycastHit hitResult;
		Vector3 cameraPosition = playerCamera.transform.position;
		bool isHit = Physics.Raycast(cameraPosition, CameraForwardVector, out hitResult, interactionRange);

		if (isHit)
		{
			Interact(ref hitResult);
		}
	}
	#endregion

	#region ANIMATIONS
	private void OnEndedHolster()
	{
		holstered = !holstered;
		if (holstered)
			unholsterAction.Invoke();
	}
	#endregion

	private void SwitchWeapon()
	{
		weaponCarrier.SwitchWeapon(isMouseScrollUp);
		SetCurrentWeapon();
	}

	private void SetCurrentWeapon()
	{
		currentWeapon = weaponCarrier.CurrentWeapon;
		if(currentWeapon)
		{
			playerAnimation.ChangeAnimatorController(currentWeapon.AnimatorController);
			playerAnimation.PlayerHolsterAnimation(holstered);
		}
	}

	private bool IsWeaponActionBlocked()
	{
		if (!currentWeapon)
			return true;
		if (holstered)
			return true;
		if (isSprinting)
			return true;

		return false;
	}

	void IDamageable.ReceiveDamage(float damage)
	{
		if (isDead) { return; }

		damage = Mathf.Clamp(damage, 0, maxHealth);
		currentHealth -= damage;

		if (currentHealth == 0)
		{
			isDead = true;
			KillPlayer();
		}
	}

	private void Interact(ref RaycastHit hitResult)
	{
		GameObject hitObject = hitResult.transform.gameObject;
		IInteractable[] interactedObjects = hitObject.GetComponents<IInteractable>();

		foreach(IInteractable interactedObject in interactedObjects)
		{
			interactedObject.Interact(this);
		}
	}

	private void KillPlayer()
	{
		GameModeBase gameMode = ServiceManager.GetGameMode();
		gameMode.CharacterKilled(this);
	}

	public void IncreaseHealth(float healthToRestore)
	{
		currentHealth += Mathf.Clamp(healthToRestore, 0, maxHealth - currentHealth);
	}

	public void InteractedWithEquipment(WeaponPickUp equipmentPickUp)
	{
		pickedUpEquipment = equipmentPickUp;
		unholsterAction = new UnityAction(PickUpEquipment);
		playerAnimation.PlayerHolsterAnimation(holstered);
	}

	private void PickUpEquipment()
	{
		weaponCarrier.PickUpEquipment(pickedUpEquipment);
		SetCurrentWeapon();
	}
}
