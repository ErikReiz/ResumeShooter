using UnityEngine;
using UnityEditor;

public class EquipmentPickUp : MonoBehaviour, IInteractable
{
	#region SERIALIZE FIELDS
	[SerializeField] MonoScript storedEquipment;
	#endregion

	void IInteractable.Interact(FPCharacter player)
	{
		Inventory inventory = player.PlayerInventory;
		Equipment equipment = System.Activator.CreateInstance(storedEquipment.GetClass()) as Equipment;

		if (inventory.TryPickUpEquipment(equipment))
			Destroy(gameObject);
	}
}