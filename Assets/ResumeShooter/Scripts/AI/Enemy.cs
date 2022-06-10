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
	private EnemyMovement enemyMovement;
	private float currentHealth;
	private bool isDead = false;
	#endregion

	private void Awake()
	{
		enemyMovement = GetComponent<EnemyMovement>();
		currentHealth = maxHealth;
	}

	private void OnEnable()
	{
		enemyMovement.NearPlayerEvent += AttackTarget;
	}

	private void OnDisable()
	{
		enemyMovement.NearPlayerEvent -= AttackTarget;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1, 1, 0, 0.75F);
		Gizmos.DrawSphere(transform.position, EnemyMovement.ChaseRange);
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
		enemyMovement.enabled = false;
		Debug.Log("Enemy Killed");
	}

	private void AttackTarget(GameObject target)
	{
		Damager.ApplyDamage(target, damage);
	}
}
