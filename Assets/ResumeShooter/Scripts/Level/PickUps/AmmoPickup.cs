using UnityEngine;

public class AmmoPickup : MonoBehaviour, IInteractable
{
	#region SERIALIZE FIELDS
	[SerializeField] private SerializableDictionary<AmmunitionType, int> stotedAmmo;
	#endregion

	void IInteractable.Interact(FPCharacter interactedPlayer)
	{
		interactedPlayer.AmmoManager.IncreaseAmmunition(stotedAmmo.Dictionary);
		Destroy(gameObject);
	}
}
