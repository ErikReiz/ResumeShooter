using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ImpactManager))]
public class Weapon : MonoBehaviour
{
	#region SERIALIZE FIELDS 
	[SerializeField] private WeaponData weaponData;
	#endregion

	#region PROPERTIES
	public RuntimeAnimatorController AnimatorController { get { return weaponData.AnimatorController; } }
	public GameObject WeaponPickUp { get { return weaponData.WeaponPickUp; } }

	public AmmunitionType AmmoType { get { return weaponData.AmmoType; } }

	public bool IsFiring { get { return isHoldingFire; } }
	public bool IsReloading { get { return isReloading; } }

	public uint GeneralAmmo { get { return ammoManager.GetAmmoCountOfType(weaponData.AmmoType); } }
	public uint MagazineSize { get { return weaponData.MagazineSize; } }
	public uint MagazineAmmo
	{
		get { return weaponData.MagazineAmmo; }
		set
		{
			if (value >= 0)
				weaponData.MagazineAmmo = value;
		}
	}
	#endregion

	#region FIELDS
	private ImpactManager impactManager;
	private Animator weaponAnimator;
	private AmmoManager ammoManager;
	private PlayerAnimationManager playerAnimation;
	private Camera playerCamera;
	private AudioManager audioManager;

	private bool isHoldingFire = false;
	private bool isReloading = false;
	private bool ableToFire = true;

	private float fireCooldown;
	#endregion

	private void Awake()
	{
		impactManager = GetComponent<ImpactManager>();
		weaponAnimator = GetComponent<Animator>();
		ammoManager = GetComponentInParent<AmmoManager>();
		playerAnimation = GetComponentInParent<PlayerAnimationManager>();
		audioManager = ServiceManager.AudioSpawner;
	}

	private void Start()
	{
		fireCooldown = 60f / weaponData.FireRate;
		playerCamera = ServiceManager.GetPlayer().PlayerCamera;
	}

	private void OnEnable()
	{
		playerAnimation.OnEndedReload += OnReloadEnded;
		playerAnimation.OnEjectCasing += OnEjectCasing;
		playerAnimation.OnAmmunitionFill += OnAmmunitionFill;
	}

	private void OnDisable()
	{
		playerAnimation.OnEndedReload -= OnReloadEnded;
		playerAnimation.OnEjectCasing -= OnEjectCasing;
		playerAnimation.OnAmmunitionFill -= OnAmmunitionFill;
	}

	#region SHOOTING
	public void StartFire()
	{
		if (!ableToFire || isReloading) { return; }

		isHoldingFire = true;
		StartCoroutine(Shoot());
	}

	public void StopFire()
	{
		isHoldingFire = false;
	}

	private IEnumerator Shoot()
	{
		ableToFire = false;
		if (weaponData.MagazineAmmo > 0)
		{
			do
			{
				Shot();
				yield return new WaitForSeconds(fireCooldown);
				ableToFire = true;
			} while (CanShoot());
		}

		if (weaponData.MagazineAmmo <= 0)
		{
			playerAnimation.FireAnimation(true);
			ableToFire = true;
			audioManager.PlaySound(weaponData.EmptyFireSound);
		}

	}

	private bool CanShoot()
	{
		if (weaponData.MagazineAmmo <= 0)
			return false;
		if (!weaponData.IsFullAuto)
			return false;
		if (!isHoldingFire)
			return false;

		return true;
	}

	private void Shot()
	{
		playerAnimation.FireAnimation(false);
		weaponAnimator.Play("Fire", 0, 0.0f);

		NoiseMaker.MakeNoise(transform.position, weaponData.ShotSoundRange);

		weaponData.MagazineAmmo--;

		ProcessRaycast();
		SpawnFireParticles();
		audioManager.PlaySound(weaponData.FireSound);
	}

	private void ProcessRaycast()
	{
		RaycastHit hitResult;
		bool isHit = Physics.Raycast(transform.position, playerCamera.transform.forward, out hitResult, weaponData.ShotDistance);
		if (isHit)
		{
			impactManager.SpawnImpactParticle(hitResult);
			ApplyDamage(hitResult);
		}
	}

	private void ApplyDamage(RaycastHit hitResult)
	{
		GameObject hitObject = hitResult.transform.gameObject;
		Damager.ApplyDamage(hitObject, weaponData.Damage);
	}

	private void SpawnFireParticles()
	{
		if (!weaponData.MuzzleFlash.isPlaying)
			weaponData.MuzzleFlash.Emit(5);
	}
	#endregion

	#region RELOADING
	public void StartReloading()
	{
		if (weaponData.MagazineAmmo == weaponData.MagazineSize || isReloading || isHoldingFire) { return; }

		if (ammoManager.HasAmmunitionOfType(weaponData.AmmoType))
		{
			Reload();
		}
	}

	private void Reload()
	{
		isReloading = true;

		if (weaponData.MagazineAmmo > 0)
		{
			playerAnimation.ReloadAnimation(false);
			weaponAnimator.Play("Reload", 0, 0.0f);
			audioManager.PlaySound(weaponData.ReloadSound);
		}
		else
		{
			playerAnimation.ReloadAnimation(true);
			weaponAnimator.Play("Reload Empty", 0, 0.0f);
			audioManager.PlaySound(weaponData.EmptyReloadSound);
		}
	}
	#endregion

	#region OTHER
	private void OnAmmunitionFill()
	{
		ammoManager.UpdateAmmoCountOfType(this);
	}

	private void OnReloadEnded()
	{
		isReloading = false;
	}

	private void OnEjectCasing()
	{
		Instantiate(weaponData.ShellPrefab, weaponData.ShellSocket.position, weaponData.ShellSocket.rotation);
	}
	#endregion
}