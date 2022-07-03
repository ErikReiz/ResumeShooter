using UnityEngine;

public enum AmmunitionType : byte
{
	RifleAmmo,
	PistolAmmo
}

[System.Serializable]
public class WeaponData
{
	#region FIELDS
	[Header("General")]
	public GameObject ShellPrefab;
	public Transform ShellSocket;
	[Tooltip("Character animator override controller")]
	public RuntimeAnimatorController AnimatorController;
	[Tooltip("Weapon pick up prefab")]
	public GameObject WeaponPickUp;

	[Header("Ammo")]
	public uint MagazineSize = 30;
	public uint MagazineAmmo;
	public AmmunitionType AmmoType;

	[Header("Fire information")]
	public bool IsFullAuto = false;
	[Tooltip("shots per second")]
	[Range(1f, 1000f)] public float FireRate = 400f;
	public float ShotDistance = 2000f;
	[Range(0.1f, 1000f)] public float Damage = 20f;
	[Tooltip("Distance at which npc will hear your shot")]
	[Range(0f, 1000f)] public float ShotSoundRange = 50f;

	[Header("Particles")]
	public ParticleSystem MuzzleFlash;

	[Header("Audio")]
	public AudioManager.Audio FireSound;
	public AudioManager.Audio EmptyFireSound;
	public AudioManager.Audio ReloadSound;
	public AudioManager.Audio EmptyReloadSound;
	#endregion
}