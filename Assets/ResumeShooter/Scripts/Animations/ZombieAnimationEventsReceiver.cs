using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ResumeShooter.Animations
{

    public class ZombieAnimationEventsReceiver : MonoBehaviour
    {
		#region FIELDS
		public UnityAction OnAttackTriggered;
		#endregion

		private void OnAttackTrigger()
		{
			OnAttackTriggered.Invoke();
		}
	}
}