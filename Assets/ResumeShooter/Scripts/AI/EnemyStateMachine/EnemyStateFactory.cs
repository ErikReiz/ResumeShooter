using UnityEngine;

namespace ResumeShooter.AI
{

	public class EnemyStateFactory
	{
		#region FIELDS
		BaseStateData stateData;
		#endregion

		public EnemyStateFactory(EnemyStateMachine context)
		{
			stateData.context = context;
			stateData.stateFactory = this;
		}

		public EnemyBaseState Idle()
		{
			return new EnemyIdleState(stateData);
		}

		public EnemyBaseState Moving(Vector3 targetPosition)
		{
			return new EnemyMovementState(stateData, targetPosition);
		}

		public EnemyBaseState Attacking(Vector3 targetPosition)
		{
			return new EnemyAttackingState(stateData, targetPosition);
		}
	}
}