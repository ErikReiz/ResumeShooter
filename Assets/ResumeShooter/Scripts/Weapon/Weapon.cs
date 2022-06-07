using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[RequireComponent(typeof(WeaponAttachmentManager), typeof(AudioSource), typeof(ImpactManager))]
public class Weapon : MonoBehaviour
{
    #region SERIALIZE FIELDS 
    [SerializeField] private WeaponData weaponData;
    #endregion

    #region FIELDS
    private float fireCooldown;
    private float currentFireCooldown;
    private float currentAmmo;
    private bool isHoldingFire = false;
    private AudioSource audioSource;
    private ImpactManager impactManager;
    #endregion

    private void Awake()
	{
        audioSource = GetComponent<AudioSource>();
        impactManager = GetComponent<ImpactManager>();

        currentAmmo = weaponData.clipSize;
	}

	private void Start()
    {
        fireCooldown = 60f / weaponData.fireRate;
        currentFireCooldown = fireCooldown;
    }

    private void Update()
    {
        ProcessFireInput();
        currentFireCooldown -= Time.deltaTime;
    }

    private void ProcessFireInput()
	{
        if(Input.GetButtonDown("Fire1"))
		{
            isHoldingFire = true;
            OnFireButtonDown();

		}
        else if(Input.GetButtonUp("Fire1"))
		{
            isHoldingFire = false;
        }

        if(isHoldingFire)
		{
            if (weaponData.isFullAuto && currentAmmo > 0)
                Shot();
		}
        
	}

	#region FIRE
	private void OnFireButtonDown()
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
        IDamage damage = hitObject.GetComponent<IDamage>();

        if(damage != null)
		{
            damage.ApplyDamage(weaponData.damage);
		}
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
}
