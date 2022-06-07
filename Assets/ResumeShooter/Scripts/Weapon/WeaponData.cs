using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
	public bool isFullAuto = false;
	public float shotDistance = 2000f;

	[Header("Ammo")]
	public float clipSize = 30f;

	[Header("Fire information")]
	[Tooltip("shots per second")]
	[Range(1f, 1000f)] public float fireRate = 400f;
	[Range(0.1f, 1000f)] public float damage = 20f;

	[Header("Particles")]
	public ParticleSystem muzzleFlash;

	[Header("Audio")]
	public AudioClip fireSound;
	public AudioClip emptyFireSound;
}
