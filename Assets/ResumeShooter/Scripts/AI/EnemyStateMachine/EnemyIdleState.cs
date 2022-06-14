using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
	public EnemyIdleState(BaseStateData stateData) : base (stateData)
	{ }

	public override void EnterState()
	{
		context.EnemyAnimator.SetBool(context.IsAttackingHash, false);
		context.EnemyAnimator.SetBool(context.IsMovingHash, false);
		aiPerception.OnPlayerSeen += OnPlayerSeen;
	}

	public override void Tick()
	{

	}

	public override void ExitState()
	{
		aiPerception.OnPlayerSeen -= OnPlayerSeen;
	}

	public override void OnPlayerSeen(Vector3 targetPosition)
	{
		SwitchState(stateFactory.Moving());
	}

	public override void OnLostVision()
	{

	}
}
