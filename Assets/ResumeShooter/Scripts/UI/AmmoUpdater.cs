using UnityEngine;
using TMPro;

public class AmmoUpdater : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private TMP_Text currentAmmoText;
	[SerializeField] private TMP_Text ganeralAmmoText;
	#endregion

	#region FIELDS
	private FPCharacter player;
	#endregion

	private void Start()
	{
		player = FindObjectOfType<FPCharacter>();
	}

	private void Update()
	{
		UpdateAmmoUI();
	}

	private void UpdateAmmoUI()
	{
		currentAmmoText.text = player.WeaponCurrentAmmo.ToString();
		ganeralAmmoText.text = player.WeaponGeneralAmmo.ToString();
	}
}
