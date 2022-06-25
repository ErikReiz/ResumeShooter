using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCarrier : MonoBehaviour
{
	#region SERIALIZE FILEDS
	[SerializeField] private List<GameObject> weaponObjects;
	#endregion

	#region PROPERTIES
	public Weapon CurrentWeapon { get { return weaponObjects[currentIndex].GetComponent<Weapon>(); } }
	#endregion

	#region FIELDS
	private int currentIndex = 0;
	private readonly int maxWeaponCount = 2;
	#endregion

	private void Awake()
	{
		InitializeWeapons();
	}

	private void InitializeWeapons()
	{
		if (weaponObjects.Count > maxWeaponCount)
			weaponObjects.RemoveRange(maxWeaponCount, weaponObjects.Count - maxWeaponCount);

		for (int i = 0; i < weaponObjects.Count; i++)
		{
			weaponObjects[i] = Instantiate(weaponObjects[i], transform);
			weaponObjects[i].SetActive(false);
		}
		weaponObjects[currentIndex].SetActive(true);
	}

	public void SwitchWeapon(bool isMouseScrollUp)
	{
		int index = isMouseScrollUp ? currentIndex + 1 : currentIndex - 1;
		SwitchWeapon(index);
	}

	public void SwitchWeapon(int index = 0)
	{
		if (index < 0)
			index = weaponObjects.Count - 1;
		else if (index >= weaponObjects.Count)
			index = 0;

		weaponObjects[currentIndex].SetActive(false);
		weaponObjects[index].SetActive(true);
		currentIndex = index;
	}

	public void PickUpEquipment(WeaponPickUp equipmentPickUp)
	{
		if (weaponObjects.Count == maxWeaponCount)
		{
			SpawnEquipmentPickUp(equipmentPickUp);

			Destroy(weaponObjects[currentIndex]);
			weaponObjects[currentIndex] = Instantiate(equipmentPickUp.Equipment, transform);
		}
		else
		{
			GameObject weaponObject = Instantiate(equipmentPickUp.Equipment, transform);
			weaponObject.SetActive(false);
			weaponObjects.Add(weaponObject);
			SwitchWeapon(weaponObjects.Count - 1);
		}

		CurrentWeapon.CurrentAmmo = equipmentPickUp.MagazineAmmo;
		Destroy(equipmentPickUp.gameObject);
	}

	private void SpawnEquipmentPickUp(WeaponPickUp equipmentPickUp)
	{
		Transform equipmentTransform = equipmentPickUp.transform;
		Weapon currentWeapon = CurrentWeapon;
		 
		GameObject PickUpGameObject = Instantiate(currentWeapon.WeaponPickUp, equipmentTransform.position, equipmentTransform.rotation);
		WeaponPickUp spawnedEquipmnetPickUp = PickUpGameObject.GetComponent<WeaponPickUp>();
		if (spawnedEquipmnetPickUp)
			spawnedEquipmnetPickUp.MagazineAmmo = currentWeapon.CurrentAmmo;
	}

	public bool CanChangeWeapon()
	{
		if (weaponObjects.Count <= 1) { return false; }
		return true;
	}
}
