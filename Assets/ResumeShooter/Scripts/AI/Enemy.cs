using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	#region FIELDS
	private EnemyHealth enemyHealth;
	private EnemyMovement enemyMovement;
	#endregion

	private void Awake()
	{
		enemyHealth = GetComponent<EnemyHealth>();
		enemyMovement = GetComponent<EnemyMovement>();
	}

	private void OnEnable()
	{
		if (enemyHealth)
			enemyHealth.DeadEvent += OnDead;		
	}

	private void OnDisable()
	{
		if (enemyHealth)
			enemyHealth.DeadEvent -= OnDead;
	}

	private void OnDead()
	{
		enemyHealth.enabled = false;
		enemyMovement.enabled = false;
	}
}
