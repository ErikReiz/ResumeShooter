using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class FPCharacter : MonoBehaviour, IDamage
{
	#region SERIALIZE FIELDS
	[SerializeField] private float maxHealth = 100f;
	[SerializeField] private WeaponCarrier weaponCarrier;
	[Tooltip("Sets locomotion smoothness")]
	[SerializeField] private float dampTimeLocomotion = 0.15f;
	#endregion

	#region PROPERTIES
	public int WeaponCurrentAmmo { get { return currentWeapon.CurrentAmmo; } }
	public int WeaponGeneralAmmo { get { return currentWeapon.GeneralAmmo; } }
	private float GetHeatlhPercents { get { return currentHealth / maxHealth; } }
	#endregion

	#region FIELDS
	public UnityAction<float> OnPlayerDamaged;
	public UnityAction<Weapon> OnWeaponChanged;

	private Weapon currentWeapon;
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
		currentHealth = maxHealth;
		SetupAnimator();

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
		eventsReceiver.OnHolsterStateSwitched += OnHolsterStateSwitched;
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
		if (holstered)
		{
			weaponCarrier.SwitchWeapon(true);
			SetCurrentWeapon();
		}
	}

	private void OnHolsterStateSwitched()
	{
		holstered = !holstered;
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

	void IDamage.ReceiveDamage(float damage)
	{
		if (isDead) { return; }

		damage = Mathf.Clamp(damage, 0, maxHealth);
		currentHealth -= damage;

		OnPlayerDamaged.Invoke(GetHeatlhPercents);

		if (currentHealth == 0)
		{
			isDead = true;
			KillPlayer();
		}
	}

	private void KillPlayer()
	{
		FindObjectOfType<FPSGameMode>().CharacterKilled(this);
	}
}
