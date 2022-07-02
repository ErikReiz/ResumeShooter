using UnityEngine;

public enum AmmunitionType : byte
{
	RifleAmmo,
	PistolAmmo
}

public enum WeaponType : byte
{
	Primary,
	Secondary
}

[System.Serializable]
public class WeaponData
{
	#region FIELDS
	[Header("General")]
	public WeaponType weaponType;
	public GameObject shellPrefab;
	public Transform shellSocket;
	[Tooltip("Character animator override controller")]
	public RuntimeAnimatorController animatorController;
	[Tooltip("Weapon pick up prefab")]
	public GameObject weaponPickUp;

	[Header("Ammo")]
	public uint magazineSize = 30;
	public uint magazineAmmo;
	public AmmunitionType ammoType;

	[Header("Fire information")]
	public bool isFullAuto = false;
	[Tooltip("shots per second")]
	[Range(1f, 1000f)] public float fireRate = 400f;
	public float shotDistance = 2000f;
	[Range(0.1f, 1000f)] public float damage = 20f;
	[Tooltip("Distance at which npc will hear your shot")]
	[Range(0f, 1000f)] public float shotSoundRange = 50f;

	[Header("Particles")]
	public ParticleSystem muzzleFlash;

	[Header("Audio")]
	public AudioClip fireSound;
	public AudioClip emptyFireSound;
	#endregion
}