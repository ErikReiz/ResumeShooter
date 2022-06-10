using UnityEngine;

public enum AmmunitionType
{
	RifleAmmo,
	PistolAmmo
}

[System.Serializable]
public class WeaponData
{
	[Header("General")]
	public bool isFullAuto = false;
	public float timeToReload = 2f;

	[Header("Ammo")]
	public int magazineSize = 30;
	public AmmunitionType ammoType;

	[Header("Fire information")]
	[Tooltip("shots per second")]
	[Range(1f, 1000f)] public float fireRate = 400f;
	public float shotDistance = 2000f;
	[Range(0.1f, 1000f)] public float damage = 20f;

	[Header("Particles")]
	public ParticleSystem muzzleFlash;

	[Header("Audio")]
	public AudioClip fireSound;
	public AudioClip emptyFireSound;
}