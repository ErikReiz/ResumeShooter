using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FPCharacter : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Tooltip("Range at which character will be able to interact with objects")]
	[SerializeField] private float interactionRange = 3f;
	#endregion

	#region PROPERTIES
	public AmmoManager AmmoManager { get { return ammoManager; } }
	public Camera PlayerCamera { get { return playerCamera; } }
	public HealthComponent HealthComponent { get { return healthComponent; } }

	public uint WeaponMagazineAmmo { get { return currentWeapon.MagazineAmmo; } }
	public uint WeaponGeneralAmmo { get { return currentWeapon.GeneralAmmo; } }
	#endregion

	#region FIELDS
	private UnityAction unholsterAction;

	private Camera playerCamera;
	private AmmoManager ammoManager;
	private PlayerController playerController;
	private WeaponCarrier weaponCarrier;
	private PlayerAnimationManager playerAnimation;
	private HealthComponent healthComponent;
	private PlayerInput input;
	private Weapon currentWeapon;

	private bool holstered = true;
	private bool isSprinting = false;

	private int inputIndex;
	#endregion

	private void Awake()
	{
		playerCamera = GetComponentInChildren<Camera>();
		ammoManager = GetComponent<AmmoManager>();
		playerController = GetComponent<PlayerController>();
		weaponCarrier = GetComponent<WeaponCarrier>();
		healthComponent = GetComponent<HealthComponent>();

		SetupAnimations();
		SetupInput();
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

	private void SetupAnimations()
	{
		playerAnimation = GetComponentInChildren<PlayerAnimationManager>();
		playerAnimation.OnWeaponSwitched += OnWeaponSwitched;
		playerAnimation.OnHolsterStateSwitched += OnHolsterStateSwitched;
	}

	private void SetupInput()
	{
		input = new();

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

	private void SetCurrentWeapon()
	{
		currentWeapon = weaponCarrier.CurrentWeapon;
		if (currentWeapon)
		{
			playerAnimation.ChangeAnimatorController(currentWeapon.AnimatorController);
			playerAnimation.PlayerHolsterAnimation(holstered);
		}
	}

	#region INPUT
	private void OnMovementInput(InputAction.CallbackContext context)
	{
		Vector2 movementInput = context.ReadValue<Vector2>();
		playerController.ReceiveMovementInput(movementInput);
		playerAnimation.ReceiveMovementInput(movementInput);
	}

	private void OnSprintInput(InputAction.CallbackContext context)
	{
		if (currentWeapon.IsFiring) { return; }

		isSprinting = IsKeyDown(context);
		ToggleSprint();
	}

	private bool IsKeyDown(InputAction.CallbackContext context)
	{
		return context.phase == InputActionPhase.Started ? true : false;
	}

	private void ToggleSprint()
	{
		playerController.ReceiveSprintingInput(isSprinting);
		playerAnimation.ToogleSprint(isSprinting);
	}

	private void OnMouseInput(InputAction.CallbackContext context)
	{
		playerController.ReceiveMouseInput(context.ReadValue<Vector2>());
	}

	private void OnFireInput(InputAction.CallbackContext context)
	{
		if (holstered) { return; }
		if (isSprinting)
		{
			isSprinting = false;
			ToggleSprint();
		}

		if (IsKeyDown(context))
			currentWeapon.StartFire();
		else
			currentWeapon.StopFire();
	}

	private void OnReloadInput(InputAction.CallbackContext context)
	{
		if (holstered) { return; }

		currentWeapon.StartReloading();
	}

	private void OnSwitchWeaponInput(InputAction.CallbackContext context)
	{
		inputIndex = Mathf.RoundToInt(context.ReadValue<float>());

		if (CanSwitchWeapon())
		{
			unholsterAction = new UnityAction(SwitchWeapon);
			playerAnimation.PlayerHolsterAnimation(holstered);
		}
	}

	private bool CanSwitchWeapon()
	{
		if (!currentWeapon)
			return false;
		if (currentWeapon.IsFiring || currentWeapon.IsReloading)
			return false;
		if (holstered)
			return false;

		return weaponCarrier.CanChangeWeapon(inputIndex);
	}

	private void OnInteractInput(InputAction.CallbackContext context)
	{
		RaycastHit hitResult;
		Vector3 cameraPosition = playerCamera.transform.position;

		bool isHit = Physics.Raycast(cameraPosition, playerCamera.transform.forward, out hitResult, interactionRange);

		if (isHit)
			Interact();

		void Interact()
		{
			GameObject hitObject = hitResult.transform.gameObject;
			IInteractable[] interactedObjects = hitObject.GetComponents<IInteractable>();

			foreach (IInteractable interactedObject in interactedObjects)
			{
				interactedObject.Interact(this);
			}
		}
	}
	#endregion

	#region ANIMATION EVENTS
	private void OnWeaponSwitched()
	{
		unholsterAction?.Invoke();
	}

	private void OnHolsterStateSwitched()
	{
		holstered = !holstered;
	}
	#endregion

	private void SwitchWeapon()
	{
		weaponCarrier.SwitchWeapon(inputIndex);
		SetCurrentWeapon();
	}

	public void PickUpWeapon(WeaponPickUp equipmentPickUp)
	{
		holstered = true;
		playerAnimation.PlayerHolsterAnimation(holstered);

		weaponCarrier.PickUpWeapon(equipmentPickUp);
		SetCurrentWeapon();
	}
}
