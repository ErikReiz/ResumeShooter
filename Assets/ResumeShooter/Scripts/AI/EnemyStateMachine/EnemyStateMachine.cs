using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    #region SERIALIZE FIELDS
    [SerializeField] private float turnSpeed = 5f;
	#endregion

	#region PROPERTIES
	public int IsMovingHash { get { return isMovingHash; } }
	public int IsAttackingHash { get { return isAttackingHash; } }
	public Animator EnemyAnimator { get { return enemyAnimator; } }

	public EnemyBaseState CurrentState { set { currentState = value; } }
    public NavMeshAgent NavMesh { get { return navMesh; } }
	#endregion

	#region FIELDS

	#region STATES
	private EnemyBaseState currentState;
    private EnemyStateFactory states;
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
		enemyAnimator = GetComponentInChildren<Animator>();
		isMovingHash = Animator.StringToHash("isMoving");
		isAttackingHash = Animator.StringToHash("isAttacking");


		aiPerception = GetComponent<AIPerception>();
        navMesh = GetComponent<NavMeshAgent>();

        states = new EnemyStateFactory(this, aiPerception);
        currentState = states.Idle();
        currentState.EnterState();
	}

	private void FixedUpdate()
	{
		currentState.Tick();
	}
}
