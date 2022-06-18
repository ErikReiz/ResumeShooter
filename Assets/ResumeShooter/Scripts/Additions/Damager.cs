using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damager
{
	public static void ApplyDamage(GameObject damageableObject, float damage)
	{
		IDamage[] damageableObjectsInterface = damageableObject.GetComponents<IDamage>();

		foreach(IDamage damaged in damageableObjectsInterface)
		if (damaged != null)
		{
			damaged.ReceiveDamage(damage);
		}
	}
}
