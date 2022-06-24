using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{ 
	#region PROPERTIES
	public int IsMovingHash { get { return isMovingHash; } }
	public int IsAttackingHash { get { return isAttackingHash; } }
	public Vector3 Position { get { return transform.parent.position; } }
	public Animator EnemyAnimator { get { return enemyAnimator; } }
	public EnemyBaseState CurrentState { set { currentState = value; } }
	public NavMeshAgent NavMesh { get { return navMesh; } }
	#endregion

	#region FIELDS

	#region STATES
	private EnemyBaseState currentState;
    private EnemyStateFactory stateFactory;
	#endregion
	#region PERCEPTIONS
    private AIPerception aiPerception;
    private NavMeshAgent navMesh;
	#endregion
	#region ANIMATOR
	private Animator enemyAnimator;
	private int isMovingHash;
	private int isAttackingHash;
	#endregion

	#endregion

	private void Awake()
	{
		SetupAnimator();

		aiPerception = GetComponentInParent<AIPerception>();
		navMesh = GetComponentInParent<NavMeshAgent>();

        stateFactory = new EnemyStateFactory(this);
        currentState = stateFactory.Idle();
        currentState.EnterState();
	}

	private void OnEnable()
	{
		aiPerception.OnPlayerSeen += OnPlayerSeen;
		aiPerception.OnLostVision += OnLostVision;
		aiPerception.OnHearedSomething += OnHearedSomething;
	}

	private void OnDisable()
	{
		aiPerception.OnPlayerSeen -= OnPlayerSeen;
		aiPerception.OnLostVision -= OnLostVision;
		aiPerception.OnHearedSomething -= OnHearedSomething;
	}

	private void SetupAnimator()
	{
		enemyAnimator = GetComponent<Animator>();
		isMovingHash = Animator.StringToHash("isMoving");
		isAttackingHash = Animator.StringToHash("isAttacking");
	}

	private void OnPlayerSeen(Vector3 targetPosition)
	{
		currentState.OnPlayerSpotted(targetPosition);
	}

	private void OnLostVision()
	{
		currentState.OnLostVision();
	}

	private void OnHearedSomething(Vector3 targetPosition)
	{
		currentState.OnPlayerSpotted(targetPosition);
	}

	private void FixedUpdate()
	{
		currentState.Tick();
	}
}
