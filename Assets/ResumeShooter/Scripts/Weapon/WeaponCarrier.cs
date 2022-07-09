using System.Collections.Generic;
using UnityEngine;

public class WeaponCarrier : MonoBehaviour
{
	#region SERIALIZE FILEDS
	[Tooltip("Weapon attach parent")]
	[SerializeField] private Transform weaponsParentTransform;

	[SerializeField] private List<GameObject> weaponObjects;
	#endregion

	#region PROPERTIES
	public Weapon CurrentWeapon { get { return weaponObjects[currentIndex].GetComponent<Weapon>(); } }
	#endregion

	#region FIELDS
	private readonly int maxWeaponCount = 2;

	private int currentIndex = 0;
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
			weaponObjects[i] = Instantiate(weaponObjects[i], weaponsParentTransform);
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

	public void PickUpWeapon(WeaponPickUp weaponPickUp)
	{
		if (weaponObjects.Count == maxWeaponCount)
		{
			SpawnWeaponPickUp(weaponPickUp.transform);

			Destroy(weaponObjects[currentIndex]);
			weaponObjects[currentIndex] = Instantiate(weaponPickUp.Weapon, weaponsParentTransform).gameObject;
		}
		else
		{
			GameObject weaponObject = Instantiate(weaponPickUp.Weapon, weaponsParentTransform).gameObject;
			weaponObject.SetActive(false);
			weaponObjects.Add(weaponObject);

			SwitchWeapon(weaponObjects.Count - 1);
		}

		CurrentWeapon.MagazineAmmo = weaponPickUp.MagazineAmmo;
		Destroy(weaponPickUp.gameObject);
	}

	private void SpawnWeaponPickUp(Transform pickUpTransform)
	{
		Weapon currentWeapon = CurrentWeapon;

		GameObject PickUpGameObject = Instantiate(currentWeapon.WeaponPickUp, pickUpTransform.position, pickUpTransform.rotation);
		WeaponPickUp spawnedEquipmnetPickUp = PickUpGameObject.GetComponent<WeaponPickUp>();
		if (spawnedEquipmnetPickUp)
			spawnedEquipmnetPickUp.MagazineAmmo = currentWeapon.MagazineAmmo;
	}

	public bool CanChangeWeapon(int inputIndex)
	{
		if (weaponObjects.Count <= 1)
			return false;
		if (inputIndex == currentIndex)
			return false;

		return true;
	}
}
