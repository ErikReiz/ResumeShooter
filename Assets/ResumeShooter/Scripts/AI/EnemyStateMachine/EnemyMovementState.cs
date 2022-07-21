using UnityEngine;

namespace ResumeShooter.AI
{

	public class EnemyMovementState : EnemyBaseState
	{
		#region FIELDS
		private Vector3 targetPosition;

		private float distanceToTarget;
		#endregion

		public EnemyMovementState(BaseStateData stateData, Vector3 targetPosition) : base(stateData)
		{
			this.targetPosition = targetPosition;
		}

		public override void EnterState()
		{
			context.EnemyAnimator.SetBool(context.IsMovingHash, true);
			context.NavMesh.isStopped = false;
			EngageTarget();
		}

		public override void ExitState()
		{
			context.EnemyAnimator.SetBool(context.IsMovingHash, false);
			context.NavMesh.isStopped = true;
		}

		public override void OnPlayerSpotted(Vector3 targetPosition)
		{
			this.targetPosition = targetPosition;
			EngageTarget();
		}

		public override void OnLostVision()
		{
			if (context.NavMesh.velocity.sqrMagnitude <= 0.5)
				SwitchState(stateFactory.Idle());
		}

		private void EngageTarget()
		{
			distanceToTarget = Vector3.Distance(context.Position, targetPosition);

			if (distanceToTarget >= context.NavMesh.stoppingDistance)
			{
				ChaseTarget();
			}

			if (distanceToTarget <= context.NavMesh.stoppingDistance)
			{
				SwitchState(stateFactory.Attacking(targetPosition));
			}
		}

		private void ChaseTarget()
		{
			context.NavMesh.SetDestination(targetPosition);
		}
	}
}