using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BaseStateData
{
	public EnemyStateMachine context;
	public EnemyStateFactory stateFactory;
	public AIPerception aiPerception;
}

public abstract class EnemyBaseState
{
	protected EnemyStateMachine context;
	protected EnemyStateFactory stateFactory;
	protected AIPerception aiPerception;

	public EnemyBaseState(BaseStateData stateData)
	{
		context = stateData.context;
		stateFactory = stateData.stateFactory;
		aiPerception = stateData.aiPerception;
	}

	public abstract void EnterState();
	public abstract void Tick();
	public abstract void ExitState();
	public abstract void OnPlayerSeen(Vector3 targetPosition);
	public abstract void OnLostVision();

	protected void UpdateStates() { }
	protected void SwitchState(EnemyBaseState newState) 
	{
		ExitState();

		newState.EnterState();
		context.CurrentState = newState;
	}
	protected void SetState() { }
}
