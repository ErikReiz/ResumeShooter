using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEventsReceiver : MonoBehaviour
{
	#region FIELDS
	public UnityAction OnHolsterStateSwitched;
	public UnityAction OnEndedHolster;
	public UnityAction OnEndedReload;
	public UnityAction OnEjectCasing;
	public UnityAction OnAmmunitionFill;
	#endregion

	#region CHARACTER EVENTS
	private void OnAnimationChangedHolster()
	{
		OnHolsterStateSwitched?.Invoke();
	}

	private void OnAnimationHolster()
	{
		OnEndedHolster?.Invoke();
	}
	#endregion

	#region WEAPON EVENTS
	private void OnAnimationEndedReload()
	{
		OnEndedReload?.Invoke();
	}

	private void OnAnimationEjectCasing()
	{
		OnEjectCasing?.Invoke();
	}

	private void OnAnimationAmmunitionFill()
	{
		OnAmmunitionFill?.Invoke();
	}
	#endregion
}