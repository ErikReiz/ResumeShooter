using System.Collections;
using UnityEngine;
using ResumeShooter.Services;
using ResumeShooter.Player;
using ResumeShooter.Animations;

namespace ResumeShooter.Weaponary
{

	public enum AmmunitionType : byte
	{
		RifleAmmo,
		PistolAmmo
	}

	[RequireComponent(typeof(ImpactManager))]
	public class Weapon : MonoBehaviour
	{
		#region SERIALIZE FIELDS 
		[Header("General")]
		[SerializeField] private GameObject shellPrefab;
		[SerializeField] private Transform shellSocket;
		[Tooltip("Character animator override controller")]
		[SerializeField] private RuntimeAnimatorController animatorController;
		[Tooltip("Weapon pick up prefab")]
		[SerializeField] private GameObject weaponPickUp;
		[SerializeField] private LayerMask enemyLayerMask;

		[Header("Ammo")]
		[SerializeField] private uint magazineSize = 30;
		[SerializeField] private uint magazineAmmo;
		[SerializeField] private AmmunitionType ammoType;

		[Header("Fire information")]
		[SerializeField] private bool isFullAuto = false;
		[Tooltip("shots per second")]
		[Range(1f, 1000f)] public float fireRate = 400f;
		[SerializeField] private float shotDistance = 2000f;
		[Range(0.1f, 1000f)] public float damage = 20f;
		[Tooltip("Distance at which npc will hear your shot")]
		[Range(0f, 1000f)] public float shotSoundRange = 50f;

		[Header("Particles")]
		[SerializeField] private ParticleSystem muzzleFlash;

		[Header("Audio")]
		[SerializeField] private AudioManager.Audio fireSound;
		[SerializeField] private AudioManager.Audio emptyFireSound;
		[SerializeField] private AudioManager.Audio reloadSound;
		[SerializeField] private AudioManager.Audio emptyReloadSound;
		#endregion

		#region PROPERTIES
		public RuntimeAnimatorController AnimatorController { get { return animatorController; } }
		public GameObject WeaponPickUp { get { return weaponPickUp; } }

		public AmmunitionType AmmoType { get { return ammoType; } }

		public bool IsFiring { get { return isHoldingFire; } }
		public bool IsReloading { get { return isReloading; } }

		public uint GeneralAmmo { get { return ammoManager.GetAmmoCountOfType(ammoType); } }
		public uint MagazineSize { get { return magazineSize; } }
		public uint MagazineAmmo
		{
			get { return magazineAmmo; }
			set
			{
				if (value >= 0)
					magazineAmmo = value;
			}
		}
		#endregion

		#region FIELDS
		private ImpactManager impactManager;
		private Animator weaponAnimator;
		private PlayerAmmoManager ammoManager;
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
			ammoManager = GetComponentInParent<PlayerAmmoManager>();
			playerAnimation = GetComponentInParent<PlayerAnimationManager>();
			audioManager = ServiceManager.AudioSpawner;
		}

		private void Start()
		{
			fireCooldown = 60f / fireRate;
			playerCamera = ServiceManager.GetPlayer().PlayerCamera;
		}

		private void OnEnable()
		{
			playerAnimation.OnEndedReload.AddListener(OnReloadEnded);
			playerAnimation.OnEjectCasing.AddListener(OnEjectCasing);
			playerAnimation.OnAmmunitionFill.AddListener(OnAmmunitionFill);
		}

		private void OnDisable()
		{
			playerAnimation.OnEndedReload.RemoveListener(OnReloadEnded);
			playerAnimation.OnEjectCasing.RemoveListener(OnEjectCasing);
			playerAnimation.OnAmmunitionFill.RemoveListener(OnAmmunitionFill);
		}

		#region SHOOTING
		private IEnumerator Shoot()
		{
			ableToFire = false;
			if (magazineAmmo > 0)
			{
				do
				{
					Shot();
					yield return new WaitForSeconds(fireCooldown);
					ableToFire = true;
				} while (CanShoot());
			}

			if (magazineAmmo <= 0)
			{
				playerAnimation.FireAnimation(true);
				ableToFire = true;
				audioManager.PlaySound(emptyFireSound);
			}

		}

		private bool CanShoot()
		{
			if (magazineAmmo <= 0)
				return false;
			if (!isFullAuto)
				return false;
			if (!isHoldingFire)
				return false;

			return true;
		}

		private void Shot()
		{
			PlayAnimation();

			NoiseMaker.MakeNoise(transform.position, shotSoundRange, enemyLayerMask);

			magazineAmmo--;

			ProcessRaycast();
			SpawnFireParticles();
			audioManager.PlaySound(fireSound);
		}

		private void PlayAnimation()
		{
			playerAnimation.FireAnimation(false);
			weaponAnimator.Play("Fire", 0, 0.0f);
		}

		private void ProcessRaycast()
		{
			RaycastHit hitResult;
			bool isHit = Physics.Raycast(transform.position, playerCamera.transform.forward, out hitResult, shotDistance);
			if (isHit)
			{
				impactManager.SpawnImpactParticle(hitResult);
				ApplyDamage(hitResult);
			}
		}

		private void ApplyDamage(RaycastHit hitResult)
		{
			GameObject hitObject = hitResult.transform.gameObject;
			
			IDamageable damagedObject = hitObject.GetComponent<IDamageable>();
			if (damagedObject != null)
				damagedObject.ReceiveDamage(damage);
		}

		private void SpawnFireParticles()
		{
			if (!muzzleFlash.isPlaying)
				muzzleFlash.Emit(5);
		}

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
		#endregion

		#region RELOADING
		public void StartReloading()
		{
			if (magazineAmmo == magazineSize || isReloading || isHoldingFire) { return; }

			if (ammoManager.HasAmmunitionOfType(ammoType))
			{
				Reload();
			}
		}

		private void Reload()
		{
			isReloading = true;

			if (magazineAmmo > 0)
			{
				playerAnimation.ReloadAnimation(false);
				weaponAnimator.Play("Reload", 0, 0.0f);
				audioManager.PlaySound(reloadSound);
			}
			else
			{
				playerAnimation.ReloadAnimation(true);
				weaponAnimator.Play("Reload Empty", 0, 0.0f);
				audioManager.PlaySound(emptyReloadSound);
			}
		}
		#endregion

		#region OTHER
		private void OnAmmunitionFill()
		{
			MagazineAmmo = ammoManager.UpdateAmmoCountOfType(ammoType, magazineAmmo, magazineSize);
		}

		private void OnReloadEnded()
		{
			isReloading = false;
		}

		private void OnEjectCasing()
		{
			Instantiate(shellPrefab, shellSocket.position, shellSocket.rotation);
		}
		#endregion
	}
}