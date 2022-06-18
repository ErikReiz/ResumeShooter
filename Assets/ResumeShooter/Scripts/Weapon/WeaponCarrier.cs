using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCarrier : MonoBehaviour
{
	#region SERIALIZE FILEDS
	[SerializeField] private GameObject[] weaponArray;
	#endregion

	#region PROPERTIES
	public Weapon GetCurrentWeapon { get { return weapons[currentIndex]; } }
	#endregion

	#region FIELDS
	private Weapon[] weapons;
	private int currentIndex = 0;
	private bool hasPrimaryWeapon = false;
	private bool hasSecondaryWeapon = false;
	#endregion

	private void Awake()
	{
		InitializeWeapons();
	}

	private void InitializeWeapons()
	{
		weapons = new Weapon[weaponArray.Length];

		for (int i = 0; i < weapons.Length; i++)
		{
			if(hasPrimaryWeapon && hasSecondaryWeapon) { return; }
			weaponArray[i] = Instantiate(weaponArray[i], transform);
			weaponArray[i].SetActive(false);

			weapons[i] = weaponArray[i].GetComponent<Weapon>();

			switch(weapons[i].WeaponType)
			{
				case WeaponType.Primary:
					hasPrimaryWeapon = true;
					break;
				case WeaponType.Secondary:
					hasSecondaryWeapon = true;
					break;
			}
		}
		SwitchWeapon();
	}

	public void SwitchWeapon(bool isMouseScrollUp)
	{
		if(weaponArray.Length <= 1) { return; }

		int index = isMouseScrollUp ? currentIndex + 1 : currentIndex - 1;

		SwitchWeapon(index);
	}

	public void SwitchWeapon(int index = 0)
	{
		if (index < 0)
			index = weaponArray.Length - 1;
		else if (index >= weaponArray.Length)
			index = 0;

		weaponArray[currentIndex].SetActive(false);
		weaponArray[index].SetActive(true);
		currentIndex = index;
	}
}
