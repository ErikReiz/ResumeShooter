using UnityEngine;
using UnityEngine.AI;
using ResumeShooter.Services;

namespace ResumeShooter.AI
{

	public class EnemyStateMachine : MonoBehaviour
	{
		#region PROPERTIES
		public Animator EnemyAnimator { get { return enemyAnimator; } }
		public EnemyBaseState CurrentState { set { currentState = value; } }
		public NavMeshAgent NavMesh { get { return navMesh; } }
		public Vector3 Position { get { return transform.parent.position; } }

		public int IsMovingHash { get { return isMovingHash; } }
		public int IsAttackingHash { get { return isAttackingHash; } }
		#endregion

		#region FIELDS
		private HealthComponent healthComponent;

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
		private int isDeadHash;
		#endregion

		#endregion

		private void Awake()
		{
			SetupAnimator();

			aiPerception = GetComponentInParent<AIPerception>();
			navMesh = GetComponentInParent<NavMeshAgent>();
			enemyAnimator = GetComponent<Animator>();
			healthComponent = GetComponent<HealthComponent>();

			stateFactory = new EnemyStateFactory(this);
		}

		private void OnEnable()
		{
			currentState = stateFactory.Idle();
			currentState.EnterState();

			aiPerception.OnPlayerSeen.AddListener(OnPlayerSeen);
			aiPerception.OnLostVision.AddListener(OnLostVision);
			aiPerception.OnHearedSomething.AddListener(OnHearedSomething);

			healthComponent.OnDeath.AddListener(OnDeath);
		}

		private void OnDisable()
		{
			aiPerception.OnPlayerSeen.RemoveListener(OnPlayerSeen);
			aiPerception.OnLostVision.RemoveListener(OnLostVision);
			aiPerception.OnHearedSomething.RemoveListener(OnHearedSomething);

			healthComponent.OnDeath.RemoveListener(OnDeath);
		}

		private void SetupAnimator()
		{
			isMovingHash = Animator.StringToHash("isMoving");
			isAttackingHash = Animator.StringToHash("isAttacking");
			isDeadHash = Animator.StringToHash("isDead");
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

		private void OnDeath()
		{
			EnemyAnimator.SetBool(isDeadHash, true);
			currentState.ExitState();
		}

		private void FixedUpdate()
		{
			currentState.Tick();
		}
	}
}