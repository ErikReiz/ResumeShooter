using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[Header("Ammo")]
	[SerializeField] private TMP_Text currentAmmoText;
	[SerializeField] private TMP_Text ganeralAmmoText;

	[Header("Health")]
	[SerializeField] private Slider healthBar;
	#endregion

	#region FIELDS
	private FPCharacter player;
	#endregion

	private void Start()
	{
		player = ServiceManager.GetPlayer();
	}

	private void Update()
	{
		UpdateHealthBar();
		UpdateAmmoUI();
	}

	private void UpdateHealthBar()
	{
		healthBar.value = player.HealthPercents;
	}

	private void UpdateAmmoUI()
	{
		currentAmmoText.text = player.WeaponMagazineAmmo.ToString();
		ganeralAmmoText.text = player.WeaponGeneralAmmo.ToString();
	}
}
