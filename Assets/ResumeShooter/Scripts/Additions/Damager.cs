using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damager
{
	public static void ApplyDamage(GameObject damageableObject, float damage)
	{
		IDamage damageableObjectInterface = damageableObject.GetComponent<IDamage>();

		if (damageableObject != null)
		{
			damageableObjectInterface.ReceiveDamage(damage);
		}
	}
}
