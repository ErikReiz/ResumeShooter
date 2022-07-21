using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResumeShooter.UI
{
	public class MainMenuHUD : HUDBase
	{
		#region SERIALIZE FIELDS
		[SerializeField] private MainMenu mainMenu;
		[SerializeField] private SettingsMenu settingsMenu;
		#endregion

		protected override void AfterAwake()
		{
			mainMenu = CreateWidget<MainMenu>(mainMenu);
			mainMenu.Initialize(OpenSettingsMenu);

			settingsMenu = CreateWidget<SettingsMenu>(settingsMenu);
			settingsMenu.Initialize(CloseSettingsMenu);

			mainMenu.gameObject.SetActive(true);
			settingsMenu.gameObject.SetActive(false);
		}

		private void OpenSettingsMenu()
		{
			mainMenu.gameObject.SetActive(false);
			settingsMenu.gameObject.SetActive(true);
		}

		private void CloseSettingsMenu()
		{
			mainMenu.gameObject.SetActive(true);
			settingsMenu.gameObject.SetActive(false);
		}
	}
}