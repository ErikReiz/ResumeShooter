using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType : byte
{
	Weapon,
	Consumable,
	Grenade
}

public class Inventory : MonoBehaviour
{
	#region FIELDS
	private Dictionary<EquipmentType, Equipment> equipment = new();
	#endregion


	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
