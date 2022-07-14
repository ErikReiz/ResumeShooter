using UnityEngine;
using ResumeShooter.Player;
using ResumeShooter.Weaponary;
using ResumeShooter.Services;

namespace ResumeShooter.PickUp
{

	public class AmmoPickup : MonoBehaviour, IInteractable
	{
		#region SERIALIZE FIELDS
		[SerializeField] private SerializableDictionary<AmmunitionType, uint> storedAmmo;
		#endregion

		void IInteractable.Interact(FPCharacter interactedPlayer)
		{
			interactedPlayer.AmmoManager.IncreaseAmmunition(storedAmmo);
			Destroy(gameObject);
		}
	}
}