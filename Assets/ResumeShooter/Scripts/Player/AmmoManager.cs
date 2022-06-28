using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private SerializableDictionary<AmmunitionType, int> ammoCount;
	#endregion

	public bool HasAmmunitionOfType(AmmunitionType ammunitionType)
	{
		if (!ammoCount.ContainsKey(ammunitionType)) { return false; }

		if (ammoCount[ammunitionType] > 0)
			return true;
		else
			return false;
	}

	public int GetAmmoCountOfType(AmmunitionType ammunitionType)
	{
		if(ammoCount.ContainsKey(ammunitionType))
			return ammoCount[ammunitionType];
		else
			return 0;
	}

	public void UpdateAmmoCountOfType(Weapon weapon)
	{
		if (!ammoCount.ContainsKey(weapon.AmmoType)) { return; }

		int ammoOfType = ammoCount[weapon.AmmoType];
		int ammoCountToFullMagazine = weapon.MagazineSize - weapon.CurrentAmmo;

		if (ammoCountToFullMagazine > ammoOfType)
		{
			ammoCount[weapon.AmmoType] = 0;
			weapon.CurrentAmmo = ammoOfType;
		}
		else
		{
			ammoCount[weapon.AmmoType] -= ammoCountToFullMagazine;
			weapon.CurrentAmmo = weapon.MagazineSize;
		}
	}

	public void IncreaseAmmunition(SerializableDictionary<AmmunitionType, int> storedAmmo)
	{
		foreach(var currentStoredAmmo in storedAmmo)
		{
			if(ammoCount.ContainsKey(currentStoredAmmo.Key))
			{
				ammoCount[currentStoredAmmo.Key] += currentStoredAmmo.Value;
			}
		}
	}
}
