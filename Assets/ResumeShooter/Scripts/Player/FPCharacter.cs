using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class FPCharacter : MonoBehaviour, IDamageable
{
	#region SERIALIZE FIELDS
	[Tooltip("Range at which character will be able to interact with objects")]
	[SerializeField] private float interactionRange = 30f;
	[SerializeField] private float maxHealth = 100f;
	[Tooltip("Player object for weapon")]
	[SerializeField] private WeaponCarrier weaponCarrier;
	[Tooltip("Sets locomotion smoothness")]
	[SerializeField] private float dampTimeLocomotion = 0.15f;
	#endregion

	#region PROPERTIES
	public AmmoManager AmmoManager { get { return ammoManager; } }
	public Vector3 CameraForwardVector { get { return playerCamera.transform.forward; } }
	public int WeaponCurrentAmmo { get { return currentWeapon.CurrentAmmo; } }
	public int WeaponGeneralAmmo { get { return currentWeapon.GeneralAmmo; } }
	public float HealthPercents { get { return currentHealth / maxHealth; } }
	#endregion

	#region FIELDS
	public UnityAction<Weapon> OnWeaponChanged;

	private Camera playerCamera;
	private Weapon currentWeapon;
	private AmmoManager ammoManager;
	private float currentHealth;
	private bool isDead = false;
	private bool holstered = true;

	#region ANIMATIONS
	private Animator characterAnimator;
	private PlayerAnimationEventsReceiver eventsReceiver;

	private int reloadingLayer;
	private int firingLayer;
	private int holsterLayer;
	private int hashMovement;
	#endregion

	#endregion

	private void Awake()
	{
		SetupAnimator();
		currentHealth = maxHealth;

		playerCamera = GetComponentInChildren<Camera>();
		ammoManager = GetComponent<AmmoManager>();
	}

	private void Start()
	{
		SetCurrentWeapon();
	}

	private void Update()
	{
		ProcessFireInput();
		ProcessReloadInput();
		ProcessSwitchWeaponInput();
		ProcessInteract();
	}

	private void SetupAnimator()
	{
		characterAnimator = GetComponentInChildren<Animator>();

		firingLayer = characterAnimator.GetLayerIndex("Firing Layer");
		reloadingLayer = characterAnimator.GetLayerIndex("Reloading Layer");
		holsterLayer = characterAnimator.GetLayerIndex("Layer Holster");
		hashMovement = Animator.StringToHash("Movement");

		eventsReceiver = GetComponentInChildren<PlayerAnimationEventsReceiver>();
		eventsReceiver.OnEndedHolster += OnEndedHolster;
	}

	#region INPUT
	private void ProcessFireInput()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (!currentWeapon || holstered) { return; }

			currentWeapon.StartFire();
		}
		else if (Input.GetButtonUp("Fire1"))
		{
			if (!currentWeapon || holstered) { return; }

			currentWeapon.StopFire();
		}
	}

	private void ProcessReloadInput()
	{
		if (Input.GetButtonDown("Reload"))
		{
			if (!currentWeapon) { return; }

			currentWeapon.TryReload();
		}
	}

	private void ProcessSwitchWeaponInput()
	{
		if(Input.GetButtonDown("SwitchWeapon"))
		{
			if (currentWeapon?.CanChangeWeapon() ?? true && !holstered)
			{
				PlayerHolsterAnimation();	
			}
		
		}
	}

	private void ProcessInteract()
	{
		if(Input.GetButtonDown("Interact"))
		{
			RaycastHit hitResult;
			Vector3 cameraPosition = playerCamera.transform.position;
			bool isHit = Physics.Raycast(cameraPosition, CameraForwardVector, out hitResult, interactionRange);

			if(isHit)
			{
				Interact(ref hitResult);
			}
		}
	}
	#endregion

	#region ANIMATIONS
	public void PlayMovementAnimation(float horizontal, float vertical)
	{
		characterAnimator.SetFloat(hashMovement, Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical)), dampTimeLocomotion, Time.deltaTime);
	}

	public void FireAnimation(bool isEmpty)
	{
		characterAnimator.CrossFade(isEmpty ? "Fire Empty" : "Fire", 0f, firingLayer, 0);
	}

	public void ReloadAnimation(bool isEmpty)
	{
		characterAnimator.CrossFade(isEmpty ? "Reload Empty" : "Reload", 0f, reloadingLayer, 0);
	}

	private void PlayerHolsterAnimation()
	{
		characterAnimator.CrossFade(holstered ? "Unholster" : "Holster", 0f, holsterLayer, 0);
	}

	private void OnEndedHolster()
	{
		holstered = !holstered;
		if (holstered)
		{
			weaponCarrier.SwitchWeapon(true);
			SetCurrentWeapon();
		}
	}
	#endregion

	private void SetCurrentWeapon()
	{
		currentWeapon = weaponCarrier.GetCurrentWeapon;
		if(currentWeapon)
		{
			characterAnimator.runtimeAnimatorController = currentWeapon.AnimatorController;
			PlayerHolsterAnimation();
		}
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
		FindObjectOfType<FPSGameMode>().CharacterKilled(this);
	}

	public void IncreaseHealth(float healthToRestore)
	{
		currentHealth += Mathf.Clamp(healthToRestore, 0, maxHealth - currentHealth);
	}
}
