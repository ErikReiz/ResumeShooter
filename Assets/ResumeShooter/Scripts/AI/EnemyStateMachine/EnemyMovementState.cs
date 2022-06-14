using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementState : EnemyBaseState
{
	private Vector3 targetPosition;

	public EnemyMovementState(BaseStateData stateData) : base(stateData)
	{ }

	public override void EnterState()
	{
		context.EnemyAnimator.SetBool(context.IsAttackingHash, false);
		context.EnemyAnimator.SetBool(context.IsMovingHash, true);

		aiPerception.OnPlayerSeen += OnPlayerSeen;
		aiPerception.OnLostVision += OnLostVision;
	}

	public override void Tick()
	{

	}

	public override void ExitState()
	{
		aiPerception.OnPlayerSeen -= OnPlayerSeen;
		aiPerception.OnLostVision -= OnLostVision;
	}

	public override void OnPlayerSeen(Vector3 targetPosition)
	{
		this.targetPosition = targetPosition;
		EngageTarget();
	}

	public override void OnLostVision()
	{
		SwitchState(stateFactory.Idle());
	}

	private void EngageTarget()
	{
		
		float distanceToTarget = Vector3.Distance(context.transform.position, targetPosition);

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
