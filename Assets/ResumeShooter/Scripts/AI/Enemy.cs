using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamage
{
	#region SERIALIZE FIELDS 
	[SerializeField] private float maxHealth = 100f;
	[Header("Damage")]
	[SerializeField] private float attackDamage = 10f;
	[Tooltip("Sphere radius around zombie hand, when attacking")]
	[SerializeField] private float attackRadius = 10f;
	[Tooltip("Hand around which the sphere will be created")]
	[SerializeField] private Transform armSocket;
	[SerializeField] private LayerMask playerLayerMask;
	#endregion

	#region PROPERTIES
	public bool IsDead { get { return isDead; } }
	#endregion

	#region FIELDS
	public UnityAction OnDamaged;

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

		OnDamaged?.Invoke();
		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			isDead = true;
			KillEnemy();
		}
	}

	private void KillEnemy()
	{
		FindObjectOfType<FPSGameMode>().CharacterKilled(this);
	}

	public void OnApplyDamage()
	{
		Collider[] overlappingObjects = Physics.OverlapSphere(armSocket.position, attackRadius, playerLayerMask);

		if (overlappingObjects[0])
			Damager.ApplyDamage(overlappingObjects[0].gameObject, attackDamage);
	}	
}
