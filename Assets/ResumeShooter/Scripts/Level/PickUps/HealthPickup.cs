using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IInteractable
{
	#region SERIALIZE FIELDS
	[SerializeField] private float healhToRestore;
	#endregion

	void IInteractable.Interact(FPCharacter interactedPlayer)
	{
		interactedPlayer.IncreaseHealth(healhToRestore);
		Destroy(gameObject);
	}
}
