using UnityEngine;
using ResumeShooter.Player;

namespace ResumeShooter.PickUp
{

	public abstract class Equipment : Object
	{
		#region SERIALIZE FIELDS
		[SerializeField] private int maxCount = 2;
		#endregion

		#region PROPERTIES
		public int Count { get { return count; } set { count = Mathf.Clamp(value, 0, maxCount); } }
		public abstract Inventory.EquipmentType Type { get; }
		#endregion

		#region FIELDS
		private int count = 1;
		#endregion

		public abstract void Use();
		public abstract void StopUsing();

		public bool CanIncrease()
		{
			if (count >= maxCount)
				return false;

			return true;
		}
	}
}