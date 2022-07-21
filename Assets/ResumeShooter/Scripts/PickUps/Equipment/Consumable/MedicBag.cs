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
		#endregion

		public override void Use()
		{
			if (!playerHealth)
				playerHealth = ServiceManager.GetPlayer().HealthComponent;

			if (playerHealth.HealthPercents == 1) { return; }

			playerHealth.CurrentHealth += healthToRestore;
			Count--;		
		}
	}
}