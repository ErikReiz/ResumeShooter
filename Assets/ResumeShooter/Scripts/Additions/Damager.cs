using UnityEngine;

public static class Damager
{
	public static void ApplyDamage(GameObject damageableObject, float damage)
	{
		IDamageable damagedObject = damageableObject.GetComponent<IDamageable>();
		if (damagedObject != null)
			damagedObject.ReceiveDamage(damage);
	}
}
