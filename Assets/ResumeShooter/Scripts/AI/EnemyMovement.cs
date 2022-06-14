using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private float chaseRange = 5f;
	[SerializeField] private float turnSpeed = 5f;
	#endregion

	#region PROPERTIES
	public float ChaseRange { get { return chaseRange; } }
	public bool IsAttacking { set { isAttacking = value; } }
	#endregion

	#region FIELDS
	public UnityAction<GameObject> NearPlayerEvent;

	private AIPerception aiPerception;
	private NavMeshAgent navMesh;
	private Animator enemyAnimator;
	private readonly string floatNameMovement = "Velocity";
	private readonly string boolNameAttacking = "isAttacking";

	private Vector3 targetPosition;
	private bool isChasingPlayer = false;
	private bool isAttacking = false;
	#endregion

	private void Awake()
	{
		navMesh = GetComponent<NavMeshAgent>();
		enemyAnimator = GetComponentInChildren<Animator>();
	}

	private void OnPlayerSeen(Vector3 playerPosition)
	{
		targetPosition = playerPosition;
		if (!isChasingPlayer)
		{
			isChasingPlayer = true;
		}
	}

	private void OnLostVision()
	{
		if(isChasingPlayer)
		{
			isChasingPlayer = false;
		}
	}

	private void Update()
	{
		//FaceTarget();
		if (isChasingPlayer)
			EngageTarget();	
			
	}

	private void FixedUpdate()
	{
		enemyAnimator.SetFloat(floatNameMovement, navMesh.velocity.sqrMagnitude);
	}

	private void EngageTarget()
	{
		float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

		if (distanceToTarget >= navMesh.stoppingDistance && isAttacking == false)
		{
			ChaseTarget();
			enemyAnimator.SetBool(boolNameAttacking, false);
		}

		if (distanceToTarget <= navMesh.stoppingDistance)
		{
			isAttacking = true;
			enemyAnimator.SetBool(boolNameAttacking, true);
		}
	}

	private void FaceTarget()
	{
		Vector3 direction = (targetPosition - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
	}

	private void ChaseTarget()
	{
		navMesh.SetDestination(targetPosition);
	}
}