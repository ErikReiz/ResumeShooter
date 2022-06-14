using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFactory
{
	#region FIELDS
	BaseStateData stateData;
	#endregion


	public EnemyStateFactory(EnemyStateMachine context, AIPerception aiPerception)
	{
		stateData.context = context;
		stateData.stateFactory = this;
		stateData.aiPerception = aiPerception;
	}

	public EnemyBaseState Idle()
	{
		return new EnemyIdleState(stateData);
	}

	public EnemyBaseState Moving()
	{
		return new EnemyMovementState(stateData);
	}

	public EnemyBaseState Attacking(Vector3 targetPosition)
	{
		return new EnemyAttackingState(stateData, targetPosition);
	}

}