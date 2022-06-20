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

	public int UpdateAmmoCountOfType(AmmunitionType ammunitionType, int magazineSize, int ammoInMagazine)
	{
		if (!ammoCount.ContainsKey(ammunitionType)) { return 0; }

		int ammoOfType = ammoCount[ammunitionType];
		int ammoCountToFullMagazine = magazineSize - ammoInMagazine;

		if (ammoCountToFullMagazine > ammoOfType)
		{
			ammoCount[ammunitionType] = 0;
			return ammoOfType;
		}
		else
		{
			ammoCount[ammunitionType] -= ammoCountToFullMagazine;
			return magazineSize;
		}
	}

	public void IncreaseAmmunition(Dictionary<AmmunitionType, int> storedAmmo)
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
