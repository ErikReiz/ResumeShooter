using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IInteractable
{
	#region SERIALIZE FIELDS
	[SerializeField] private int magazineAmmo;
	[Tooltip("What weapon will be picked up")]
	[SerializeField] private GameObject equipment;
	#endregion

	#region PROPERTIES
	public GameObject Equipment { get { return equipment; } }

	public int MagazineAmmo
	{ 
		get { return magazineAmmo; }
		set
		{
			if(value >= 0)
				magazineAmmo = value;
		} 
	}
	#endregion

	void IInteractable.Interact(FPCharacter interactedPlayer)
	{
		interactedPlayer.InteractedWithEquipment(this);
	}
}
