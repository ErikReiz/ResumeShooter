using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ResumeShooter.Services;
using ResumeShooter.Player;

namespace ResumeShooter.UI
{

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
		private HealthComponent playerHealthComponent;
		private FPCharacter player;
		#endregion

		private void Start()
		{
			player = ServiceManager.GetPlayer();
			playerHealthComponent = player.HealthComponent;
		}

		private void Update()
		{
			UpdateHealthBar();
			UpdateAmmoUI();
		}

		private void UpdateHealthBar()
		{
			healthBar.value = playerHealthComponent.HealthPercents;
		}

		private void UpdateAmmoUI()
		{
			currentAmmoText.text = player.WeaponMagazineAmmo.ToString();
			ganeralAmmoText.text = player.WeaponGeneralAmmo.ToString();
		}
	}
}