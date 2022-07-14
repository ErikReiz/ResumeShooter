using UnityEngine;
using ResumeShooter.Weaponary;
using ResumeShooter.Services;

namespace ResumeShooter.Player
{

	public class PlayerAmmoManager : MonoBehaviour
	{
		#region SERIALIZE FIELDS
		[SerializeField] private SerializableDictionary<AmmunitionType, uint> ammoCount;
		#endregion

		public bool HasAmmunitionOfType(AmmunitionType ammunitionType)
		{
			if (!ammoCount.ContainsKey(ammunitionType)) { return false; }

			if (ammoCount[ammunitionType] > 0)
				return true;
			else
				return false;
		}

		public uint GetAmmoCountOfType(AmmunitionType ammunitionType)
		{
			if (ammoCount.ContainsKey(ammunitionType))
				return ammoCount[ammunitionType];
			else
				return 0;
		}

		public uint UpdateAmmoCountOfType(AmmunitionType ammoType, uint magazineAmmo, uint magazineSize)
		{
			if (!ammoCount.ContainsKey(ammoType)) { return magazineAmmo; }

			uint ammoOfType = ammoCount[ammoType];
			uint ammoCountToFullMagazine = magazineSize - magazineAmmo;

			if (ammoCountToFullMagazine > ammoOfType)
			{
				ammoCount[ammoType] = 0;
				magazineAmmo = ammoOfType;
			}
			else
			{
				ammoCount[ammoType] -= ammoCountToFullMagazine;
				magazineAmmo = magazineSize;
			}

			return magazineAmmo;
		}

		public void IncreaseAmmunition(SerializableDictionary<AmmunitionType, uint> storedAmmo)
		{
			foreach (var currentStoredAmmo in storedAmmo)
			{
				if (ammoCount.ContainsKey(currentStoredAmmo.Key))
				{
					ammoCount[currentStoredAmmo.Key] += currentStoredAmmo.Value;
				}
			}
		}
	}
}