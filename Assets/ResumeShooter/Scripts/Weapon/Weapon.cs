using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[RequireComponent(typeof(AudioSource), typeof(ImpactManager))]
public class Weapon : MonoBehaviour
{
    #region SERIALIZE FIELDS 
    [SerializeField] private WeaponData weaponData;
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
    #endregion

    private void Awake()
	{
        audioSource = GetComponent<AudioSource>();
        impactManager = GetComponent<ImpactManager>();
        ammoManager = GetComponentInParent<AmmoManager>();

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
        if(currentAmmo > 0)
		{
            if (weaponData.isFullAuto)
               return;

            Shot();
		}
        else
            audioSource.PlayOneShot(weaponData.emptyFireSound);
    }

    private void Shot()
	{
        if(currentFireCooldown <= 0)
		{
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
        if(!weaponData.muzzleFlash.isPlaying)
            weaponData.muzzleFlash.Emit(5);
	}

    private void PlayFireSounds()
    { 
        if(audioSource.isPlaying)
	    {
            audioSource.Stop();
	    }

        audioSource.PlayOneShot(weaponData.fireSound);
	}
	#endregion

	#region RELOADING
    public void TryReload()
	{
        if(currentAmmo == weaponData.magazineSize || isReloading || isHoldingFire) { return; }

        if (ammoManager.HasAmmunitionOfType(weaponData.ammoType))
		{
            StartCoroutine(Reload());
		}
	}

    private IEnumerator Reload()
	{
        isReloading = true;
        yield return new WaitForSeconds(weaponData.timeToReload);

        currentAmmo = ammoManager.UpdateAmmoCountOfType(weaponData.ammoType, weaponData.magazineSize, currentAmmo);

        isReloading = false;
	}
	#endregion
}