using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BaseStateData
{
	public EnemyStateMachine context;
	public EnemyStateFactory stateFactory;
}

public abstract class EnemyBaseState
{
	protected EnemyStateMachine context;
	protected EnemyStateFactory stateFactory;

	public EnemyBaseState(BaseStateData stateData)
	{
		context = stateData.context;
		stateFactory = stateData.stateFactory;
	}

	public abstract void EnterState();

	public virtual void Tick()
	{

	}

	public virtual void ExitState()
	{

	}

	public virtual void OnPlayerSpotted(Vector3 targetPosition)
	{

	}

	public virtual void OnLostVision()
	{

	}

	protected void SwitchState(EnemyBaseState newState) 
	{
		ExitState();

		newState.EnterState();
		context.CurrentState = newState;
	}
}
