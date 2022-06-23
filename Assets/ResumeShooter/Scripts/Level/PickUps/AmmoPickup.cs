using UnityEngine;

public class AmmoPickup : MonoBehaviour, IInteractable
{
	#region SERIALIZE FIELDS
	[SerializeField] private SerializableDictionary<AmmunitionType, int> storedAmmo;
	#endregion

	void IInteractable.Interact(FPCharacter interactedPlayer)
	{
		interactedPlayer.AmmoManager.IncreaseAmmunition(storedAmmo);
		Destroy(gameObject);
	}
}
