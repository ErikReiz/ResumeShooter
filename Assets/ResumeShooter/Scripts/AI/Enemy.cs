using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamage
{
	#region SERIALIZE FIELDS 
	[SerializeField] private float maxHealth = 100f;
	[SerializeField] private float damage = 10f;
	#endregion

	#region FIELDS
	private Animator enemyAnimator;
	private float currentHealth;
	private bool isDead = false;
	#endregion

	private void Awake()
	{
		currentHealth = maxHealth;
		enemyAnimator = GetComponentInChildren<Animator>();
	}

	void IDamage.ReceiveDamage(float damage)
	{
		if (isDead) { return; }

		damage = Mathf.Clamp(damage, 0, maxHealth);
		currentHealth -= damage;

		if (currentHealth == 0)
		{
			isDead = true;
			KillEnemy();
		}
	}

	private void KillEnemy()
	{
		Debug.Log("Enemy Killed");
	}
}
