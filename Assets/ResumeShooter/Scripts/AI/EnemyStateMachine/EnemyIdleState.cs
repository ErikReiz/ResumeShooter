using UnityEngine;

namespace ResumeShooter.AI
{

	public class EnemyIdleState : EnemyBaseState
	{
		public EnemyIdleState(BaseStateData stateData) : base(stateData) { }

		public override void EnterState()
		{
			context.EnemyAnimator.SetBool(context.IsAttackingHash, false);
			context.EnemyAnimator.SetBool(context.IsMovingHash, false);
		}

		public override void OnPlayerSpotted(Vector3 targetPosition)
		{
			SwitchState(stateFactory.Moving(targetPosition));
		}
	}
}