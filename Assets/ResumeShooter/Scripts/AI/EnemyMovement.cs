using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
	#region SERIALIZEFIELDS
	[SerializeField] private float chaseRange = 5f;
	#endregion

	#region FIELDS
	private Transform target;
	private bool isProvoked = false;
	private float distanceToTarget = Mathf.Infinity;
	private NavMeshAgent navMesh;
	#endregion

	private void Awake()
	{
		target = FindObjectOfType<FPCharacter>().transform;
		navMesh = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		distanceToTarget = Vector3.Distance(transform.position, target.position);

		if(isProvoked)
		{
			EngageTarget();
		}
		else if (distanceToTarget <= chaseRange)
		{
			isProvoked = true;
			navMesh.SetDestination(target.position);
		}
			
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1, 1, 0, 0.75F);
		Gizmos.DrawSphere(transform.position, chaseRange);
	}

	private void EngageTarget()
	{
		if (distanceToTarget >= navMesh.stoppingDistance)
		{
			ChaseTarget();
		}

		if (distanceToTarget <= navMesh.stoppingDistance)
		{
			AttackTarget();
		}
	}

	private void ChaseTarget()
	{
		navMesh.SetDestination(target.position);
	}

	private void AttackTarget()
	{
		Debug.Log(name + " has seeked and is destroying " + target.name);
	}
}
