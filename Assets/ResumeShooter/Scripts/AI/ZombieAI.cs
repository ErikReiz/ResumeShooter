using UnityEngine;

public class ZombieAI : MonoBehaviour
{
	#region SERIALIZE FIELDS 
	[Header("Damage")]
	[SerializeField] private Transform armSocket;
	[SerializeField] private LayerMask playerLayerMask;

	[SerializeField] private float attackDamage = 10f;
	[Tooltip("Sphere radius around zombie hand, when attacking")]
	[SerializeField] private float attackRadius = 2f;
	[Tooltip("Hand around which the sphere will be created")]
	#endregion

	public void OnApplyDamage()
	{
		Collider[] overlappingObjects = Physics.OverlapSphere(armSocket.position, attackRadius, playerLayerMask);
		if (overlappingObjects.Length == 0) { return; }

		if (overlappingObjects[0])
			Damager.ApplyDamage(overlappingObjects[0].gameObject, attackDamage);
	}
}
