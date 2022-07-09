using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IInteractable
{
	#region SERIALIZE FIELDS
	[Tooltip("What weapon will be picked up")]
	[SerializeField] private Weapon weapon;

	[SerializeField] private uint magazineAmmo;
	#endregion

	#region PROPERTIES
	public Weapon Weapon { get { return weapon; } }

	public uint MagazineAmmo
	{
		get { return magazineAmmo; }
		set
		{
			if (value >= 0)
				magazineAmmo = value;
		}
	}
	#endregion

	void IInteractable.Interact(FPCharacter interactedPlayer)
	{
		interactedPlayer.PickUpWeapon(this);
	}
}
