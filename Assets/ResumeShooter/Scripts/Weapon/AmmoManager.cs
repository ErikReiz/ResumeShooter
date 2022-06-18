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
		if (!ammoCountDictionary.ContainsKey(ammunitionType)) { return false; }

		if (ammoCountDictionary[ammunitionType] > 0)
			return true;
		else
			return false;
	}

	public int GetAmmoCountOfType(AmmunitionType ammunitionType)
	{
		if(ammoCountDictionary.ContainsKey(ammunitionType))
			return ammoCountDictionary[ammunitionType];
		else
			return 0;
	}

	public int UpdateAmmoCountOfType(AmmunitionType ammunitionType, int magazineSize, int ammoInMagazine)
	{
		if (!ammoCountDictionary.ContainsKey(ammunitionType)) { return 0; }

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
