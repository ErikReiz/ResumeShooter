using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
	#region FIELDS
	private Vector3 targetPosition;
	private float turnSpeed = 5f;
	#endregion

	public EnemyAttackingState(BaseStateData stateData, Vector3 targetPosition) : base(stateData)
	{ 
		 this.targetPosition = targetPosition;
	}

	public override void EnterState()
	{
		context.EnemyAnimator.SetBool(context.IsAttackingHash, true);
	}

	public override void Tick()
	{
		FaceTarget();
	}

	public override void ExitState()
	{
		context.EnemyAnimator.SetBool(context.IsAttackingHash, false);
	}

	public override void OnPlayerSpotted(Vector3 targetPosition)
	{
		this.targetPosition = targetPosition;
		CheckSwitchState();
	}

	public override void OnLostVision()
	{
		SwitchState(stateFactory.Idle());
	}

	private void CheckSwitchState()
	{
		float distanceToTarget = Vector3.Distance(context.Position, targetPosition);
		if (distanceToTarget >= context.NavMesh.stoppingDistance)
		{
			SwitchState(stateFactory.Moving(targetPosition));
		}
	}

	private void FaceTarget()
	{
		Vector3 direction = (targetPosition - context.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		context.transform.parent.rotation = Quaternion.Slerp(context.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
	}
}