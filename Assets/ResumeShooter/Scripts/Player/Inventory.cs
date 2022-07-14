using System.Collections.Generic;
using UnityEngine;
using ResumeShooter.PickUp;

namespace ResumeShooter.Player
{

	public class Inventory : MonoBehaviour
	{
		public enum EquipmentType : byte
		{
			Consumable,
			Grenade
		}

		#region FIELDS
		private Dictionary<EquipmentType, Equipment> equipment = new();
		#endregion

		public void OnUseEquipmentInput(EquipmentType equipmentType, bool isKeyDown)
		{
			if (isKeyDown)
				equipment[equipmentType].Use();
			else
				equipment[equipmentType].StopUsing();
		}

		public bool TryPickUpEquipment(Equipment pickedUpEquipment)
		{
			EquipmentType type = pickedUpEquipment.Type;
			if (equipment.ContainsKey(type))
			{
				if (equipment[type].GetType() == pickedUpEquipment.GetType())
				{
					if (!equipment[type].CanIncrease())
						return false;

					equipment[type].Count++;
				}
			}
			else
			{
				pickedUpEquipment.Count = 1;
				equipment.Add(type, pickedUpEquipment);
			}

			return true;
		}
	}
}