using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource), typeof(ImpactManager))]
public class Weapon : MonoBehaviour
{
	#region SERIALIZE FIELDS 
	[SerializeField] private WeaponData weaponData;
	#endregion

	#region PROPERTIES
	public int CurrentAmmo { get { return currentAmmo; } }
	public int GeneralAmmo { get { return ammoManager.GetAmmoCountOfType(weaponData.ammoType); } }
	#endregion

	#region FIELDS
	private bool isHoldingFire = false;
	private bool isReloading = false;

	private int currentAmmo;
	private float fireCooldown;
	private float currentFireCooldown;

	private AudioSource audioSource;
	private ImpactManager impactManager;
	private AmmoManager ammoManager;
	private CharacterAnimationManager characterAnimationManager;
	private Animator weaponAnimator;
	#endregion

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		impactManager = GetComponent<ImpactManager>();
		ammoManager = GetComponentInParent<AmmoManager>();
		characterAnimationManager = GetComponentInParent<CharacterAnimationManager>();
		weaponAnimator = GetComponent<Animator>();

		currentAmmo = weaponData.magazineSize;
	}

	private void Start()
	{
		fireCooldown = 60f / weaponData.fireRate;
		currentFireCooldown = fireCooldown;
	}

	private void Update()
	{
		currentFireCooldown -= Time.deltaTime;
	}

	#region SHOOTING
	public void StartFire()
	{
		isHoldingFire = true;
		FirstShot();
	}

	public void StopFire()
	{
		isHoldingFire = false;
	}

	public void ProcessAutomaticFire()
	{
		if (isHoldingFire)
		{
			if (weaponData.isFullAuto && currentAmmo > 0)
				Shot();
		}
	}

	private void FirstShot()
	{
		if (currentAmmo > 0)
		{
			if (weaponData.isFullAuto)
				return;

			Shot();
		}
		else
		{
			characterAnimationManager.FireAnimation(true);
			audioSource.PlayOneShot(weaponData.emptyFireSound);
		}

	}

	private void Shot()
	{
		if (currentFireCooldown <= 0 && !isReloading)
		{
			characterAnimationManager.FireAnimation(false);
			weaponAnimator.Play("Fire", 0, 0.0f);

			currentAmmo--;
			currentFireCooldown = fireCooldown;

			ProcessRaycast();
			SpawnFireParticles();
			PlayFireSounds();
		}
	}

	private void ProcessRaycast()
	{

		RaycastHit hitResult;
		bool isHit = Physics.Raycast(transform.parent.position, transform.forward, out hitResult, weaponData.shotDistance);
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
	public void TryReload()
	{
		if (currentAmmo == weaponData.magazineSize || isReloading || isHoldingFire) { return; }

		if (ammoManager.HasAmmunitionOfType(weaponData.ammoType))
		{
			Reload();
		}
	}

	private void Reload()
	{
		isReloading = true;

		if (currentAmmo > 0)
		{
			characterAnimationManager.ReloadAnimation(false);
			weaponAnimator.Play("Reload", 0, 0.0f);
		}
		else
		{
			characterAnimationManager.ReloadAnimation(true);
			weaponAnimator.Play("Reload Empty", 0, 0.0f);
		}
	}

	public void OnAmmunitionFill()
	{
		currentAmmo = ammoManager.UpdateAmmoCountOfType(weaponData.ammoType, weaponData.magazineSize, currentAmmo);
	}

	public void OnReloadAnimationEnded()
	{
		isReloading = false;
	}
	public void OnEjectCasing()
	{
		Instantiate(weaponData.shellPrefab, weaponData.shellSocket.position, weaponData.shellSocket.rotation);
	}

	#endregion
}