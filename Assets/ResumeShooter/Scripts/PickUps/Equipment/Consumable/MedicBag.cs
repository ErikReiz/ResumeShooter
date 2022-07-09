using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public override void Use()
	{
		if(!playerHealth)
			playerHealth = ServiceManager.GetPlayer().HealthComponent;

		if (playerHealth.HealthPercents == 1) { return; }

		playerHealth.CurrentHealth += healthToRestore;
		Count--;
	}
	/*
	private IEnumerator OnTimedEvent()
	{
		yield return new WaitForSeconds(useDelay);
		Debug.Log("used");
		//Count--;
	}
	*/
	public override void StopUsing()
	{

	}
}