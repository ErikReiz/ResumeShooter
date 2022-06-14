using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private SerializableDictionary<AmmunitionType, int> ammoCount;
	#endregion

	#region FIELDS
	private Dictionary<AmmunitionType, int> ammoCountDictionary;
	#endregion

	private void Awake()
	{
		ammoCountDictionary = ammoCount.GetDictionary;
	}

	public bool HasAmmunitionOfType(AmmunitionType ammunitionType)
	{
		if (ammoCountDictionary[ammunitionType] > 0)
			return true;
		else
			return false;
	}

	public int GetAmmoCountOfType(AmmunitionType ammunitionType)
	{
		return ammoCountDictionary[ammunitionType];
	}

	public int UpdateAmmoCountOfType(AmmunitionType ammunitionType, int magazineSize, int ammoInMagazine)
	{
		int ammoOfType = ammoCountDictionary[ammunitionType];
		int ammoCountToFullMagazine = magazineSize - ammoInMagazine;

		if (ammoCountToFullMagazine > ammoOfType)
		{
			ammoCountDictionary[ammunitionType] = 0;
			return ammoOfType;
		}
		else
		{
			ammoCountDictionary[ammunitionType] -= ammoCountToFullMagazine;
			return magazineSize;
		}
	}
}
