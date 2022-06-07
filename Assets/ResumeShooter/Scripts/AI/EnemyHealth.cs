using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour, IDamage
{
	#region SERIALIZE FIELDS 
	[SerializeField] private float maxHealth = 100f;
	#endregion

	#region FIELDS
	public UnityAction DeadEvent;

	private float currentHealth;
	private bool isDead = false;
	#endregion

	private void Start()
	{
		currentHealth = maxHealth;
	}
	
	void IDamage.ApplyDamage(float damage)
	{
		if(!isDead)
		{
			damage = Mathf.Clamp(damage, 0, maxHealth);
			currentHealth -= damage;

			if(currentHealth == 0)
			{
				isDead = true;
				DeadEvent?.Invoke();
			}
		}
	}
}