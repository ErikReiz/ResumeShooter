using UnityEngine;
using UnityEditor;
using ResumeShooter.Player;

namespace ResumeShooter.PickUp
{

	public class EquipmentPickUp : MonoBehaviour, IInteractable
	{
		#region SERIALIZE FIELDS
		[SerializeField] private MonoScript storedEquipment;
		#endregion

		void IInteractable.Interact(FPCharacter player)
		{
			Inventory inventory = player.PlayerInventory;
			Equipment equipment = System.Activator.CreateInstance(storedEquipment.GetClass()) as Equipment;

			if (inventory.TryPickUpEquipment(equipment))
				Destroy(gameObject);
		}
	}
}