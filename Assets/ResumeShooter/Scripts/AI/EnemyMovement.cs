using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private static float chaseRange = 5f;
	[SerializeField] private float turnSpeed = 5f;
	#endregion

	#region PROPERTIES
	public static float ChaseRange { get { return chaseRange; } }
	#endregion

	#region FIELDS
	public UnityAction<GameObject> NearPlayerEvent;

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

	private void EngageTarget()
	{
		FaceTarget();
		if (distanceToTarget >= navMesh.stoppingDistance)
		{
			ChaseTarget();
		}

		if (distanceToTarget <= navMesh.stoppingDistance)
		{
			NearPlayerEvent.Invoke(target.gameObject);
		}
	}

	private void FaceTarget()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
	}

	private void ChaseTarget()
	{
		navMesh.SetDestination(target.position);
	}
}