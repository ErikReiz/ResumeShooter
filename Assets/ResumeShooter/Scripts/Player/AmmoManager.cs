using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
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
		if(ammoCount.ContainsKey(ammunitionType))
			return ammoCount[ammunitionType];
		else
			return 0;
	}

	public void UpdateAmmoCountOfType(Weapon weapon)
	{
		if (!ammoCount.ContainsKey(weapon.AmmoType)) { return; }

		uint ammoOfType = ammoCount[weapon.AmmoType];
		uint ammoCountToFullMagazine = weapon.MagazineSize - weapon.MagazineAmmo;

		if (ammoCountToFullMagazine > ammoOfType)
		{
			ammoCount[weapon.AmmoType] = 0;
			weapon.MagazineAmmo = ammoOfType;
		}
		else
		{
			ammoCount[weapon.AmmoType] -= ammoCountToFullMagazine;
			weapon.MagazineAmmo = weapon.MagazineSize;
		}
	}

	public void IncreaseAmmunition(SerializableDictionary<AmmunitionType, uint> storedAmmo)
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
