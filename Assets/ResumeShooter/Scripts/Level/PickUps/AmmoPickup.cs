using UnityEngine;

public class AmmoPickup : MonoBehaviour, IInteractable
{
	#region SERIALIZE FIELDS
	[SerializeField] private SerializableDictionary<AmmunitionType, int> storedAmmo;
	#endregion

	private void Start()
	{
		Debug.Log(storedAmmo.Count);
		storedAmmo.Clear();
		Debug.Log(storedAmmo.Count);
	}

	void IInteractable.Interact(FPCharacter interactedPlayer)
	{
		interactedPlayer.AmmoManager.IncreaseAmmunition(storedAmmo);
		Destroy(gameObject);
	}
}
