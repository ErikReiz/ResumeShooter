using System.Collections;
using UnityEngine;
using ResumeShooter.Player;
using ResumeShooter.Services;

namespace ResumeShooter.PickUp
{

	public class MedicBag : Equipment
	{
		#region PROPERTIES
		public override Inventory.EquipmentType Type { get { return Inventory.EquipmentType.Consumable; } }
		#endregion

		#region FIELDS
		private HealthComponent playerHealth;

		private float healthToRestore = 50f;
		private float useDelay = 1f; // Player should hold "use consumable" button this amount of time to actually use it
		#endregion

		private IEnumerator OnTimedEvent()
		{
			yield return new WaitForSeconds(useDelay);

			playerHealth.CurrentHealth += healthToRestore;
			Count--;
		}

		public override void Use()
		{
			if (!playerHealth)
				playerHealth = ServiceManager.GetPlayer().HealthComponent;

			if (playerHealth.HealthPercents == 1) { return; }

			ServiceManager.GetNonMonoBehaviourCorotine().StartCoroutine(OnTimedEvent());
		}

		public override void StopUsing()
		{
			ServiceManager.GetNonMonoBehaviourCorotine().StopCoroutine(OnTimedEvent());
		}
	}
}