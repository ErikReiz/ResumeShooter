using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(ImpactManager))]
public class Weapon : MonoBehaviour
{
	#region SERIALIZE FIELDS 
	[SerializeField] private WeaponData weaponData;
	#endregion

	#region PROPERTIES
	public RuntimeAnimatorController AnimatorController { get { return weaponData.animatorController; } }
	public WeaponType WeaponType { get { return weaponData.weaponType; } }
	public GameObject WeaponPickUp { get { return weaponData.weaponPickUp; } }

	public int CurrentAmmo 
	{
		get { return weaponData.currentAmmo; }
		set
		{
			if (value >= 0)
				weaponData.currentAmmo = value;
		}
	}
	public int GeneralAmmo { get { return ammoManager.GetAmmoCountOfType(weaponData.ammoType); } }
	#endregion

	#region FIELDS
	private bool isHoldingFire = false;
	private bool isReloading = false;
	private bool ableToFire = true;

	//private int currentAmmo;
	private float fireCooldown;

	private AudioSource audioSource;
	private ImpactManager impactManager;
	private AmmoManager ammoManager;
	private PlayerAnimationManager playerAnimation;
	private Animator weaponAnimator;
	private Camera playerCamera;
	#endregion

	private void Awake()
	{
		playerCamera = transform.root.gameObject.GetComponentInChildren<Camera>();
		audioSource = GetComponent<AudioSource>();
		impactManager = GetComponent<ImpactManager>();
		weaponAnimator = GetComponent<Animator>();
		ammoManager = GetComponentInParent<AmmoManager>();
		playerAnimation = GetComponentInParent<PlayerAnimationManager>();
	}

	private void Start()
	{
		fireCooldown = 60f / weaponData.fireRate;
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
		if (weaponData.currentAmmo > 0)
		{
			do
			{
				Shot();
				yield return new WaitForSeconds(fireCooldown);
				ableToFire = true;
			} while (CanShoot());
		}

		if (weaponData.currentAmmo <= 0)
		{
			playerAnimation.FireAnimation(true);
			audioSource.PlayOneShot(weaponData.emptyFireSound);
			ableToFire = true;
		}

	}

	private bool CanShoot()
	{
		if (weaponData.currentAmmo <= 0)
			return false;
		if (!weaponData.isFullAuto)
			return false;
		if (!isHoldingFire)
			return false;

		return true;
	}

	private void Shot()
	{
		playerAnimation.FireAnimation(false);
		weaponAnimator.Play("Fire", 0, 0.0f);

		NoiseMaker.MakeNoise(transform.position, weaponData.shotSoundRange);

		weaponData.currentAmmo--;

		ProcessRaycast();
		SpawnFireParticles();
		PlayFireSounds();
	}

	private void ProcessRaycast()
	{
		RaycastHit hitResult;
		bool isHit = Physics.Raycast(transform.position, playerCamera.transform.forward, out hitResult, weaponData.shotDistance);
		if (isHit)
		{
			impactManager.SpawnImpactParticle(hitResult);
			ApplyDamage(hitResult);
		}
	}

	private void ApplyDamage(RaycastHit hitResult)
	{
		GameObject hitObject = hitResult.transform.gameObject;
		Damager.ApplyDamage(hitObject, weaponData.damage);
	}

	private void SpawnFireParticles()
	{
		if (!weaponData.muzzleFlash.isPlaying)
			weaponData.muzzleFlash.Emit(5);
	}

	private void PlayFireSounds()
	{
		if (audioSource.isPlaying)
		{
			audioSource.Stop();
		}

		audioSource.PlayOneShot(weaponData.fireSound);
	}
	#endregion

	#region RELOADING
	public void StartReloading()
	{
		if (weaponData.currentAmmo == weaponData.magazineSize || isReloading || isHoldingFire) { return; }

		if (ammoManager.HasAmmunitionOfType(weaponData.ammoType))
		{
			Reload();
		}
	}

	private void Reload()
	{
		isReloading = true;

		if (weaponData.currentAmmo > 0)
		{
			playerAnimation.ReloadAnimation(false);
			weaponAnimator.Play("Reload", 0, 0.0f);
		}
		else
		{
			playerAnimation.ReloadAnimation(true);
			weaponAnimator.Play("Reload Empty", 0, 0.0f);
		}
	}

	private void OnAmmunitionFill()
	{
		weaponData.currentAmmo = ammoManager.UpdateAmmoCountOfType(weaponData.ammoType, weaponData.magazineSize, weaponData.currentAmmo);
	}

	private void OnReloadEnded()
	{
		isReloading = false;
	}

	private void OnEjectCasing()
	{
		Instantiate(weaponData.shellPrefab, weaponData.shellSocket.position, weaponData.shellSocket.rotation);
	}

	public bool CanChangeWeapon()
	{
		return !isReloading && !isHoldingFire;
	}
	#endregion
}