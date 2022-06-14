using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
	private Vector3 targetPosition;

	public EnemyAttackingState(BaseStateData stateData, Vector3 targetPosition) : base(stateData)
	{ 
		 this.targetPosition = targetPosition;
	}

	public override void EnterState()
	{
		context.EnemyAnimator.SetBool(context.IsAttackingHash, true);
		context.EnemyAnimator.SetBool(context.IsMovingHash, false);

		aiPerception.OnPlayerSeen += OnPlayerSeen;
		aiPerception.OnLostVision += OnLostVision;
	}

	public override void Tick()
	{
		FaceTarget();
	}

	public override void ExitState()
	{
		aiPerception.OnPlayerSeen -= OnPlayerSeen;
		aiPerception.OnLostVision -= OnLostVision;
	}

	public override void OnPlayerSeen(Vector3 targetPosition)
	{
		this.targetPosition = targetPosition;
		CheckSwitchState();
	}

	private void CheckSwitchState()
	{
		float distanceToTarget = Vector3.Distance(context.transform.position, targetPosition);
		if (distanceToTarget >= context.NavMesh.stoppingDistance)
		{
			SwitchState(stateFactory.Moving());
		}
	}

	private void FaceTarget()
	{
		Vector3 direction = (targetPosition - context.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		context.transform.rotation = Quaternion.Slerp(context.transform.rotation, lookRotation, Time.deltaTime * 5f); // ������������ turn speed
	}

	public override void OnLostVision()
	{
		SwitchState(stateFactory.Idle());
	}
}