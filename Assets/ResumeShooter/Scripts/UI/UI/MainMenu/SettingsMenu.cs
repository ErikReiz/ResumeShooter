using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace ResumeShooter.UI
{

    public class SettingsMenu : MonoBehaviour
    {
		#region SERIALIZE FIELDS
		[Header("Display")]
		[SerializeField] private TMP_Dropdown displayModeMenu;
		[SerializeField] private TMP_Dropdown resolutionMenu;

		[Header("Quality")]
		[SerializeField] private TMP_Dropdown anisotropicTexturesMenu;
		[SerializeField] private TMP_Dropdown antiAliasingMenu;
		[SerializeField] private TMP_Dropdown textureQualityMenu;
		[SerializeField] private TMP_Dropdown shadowsMenu;
		[SerializeField] private TMP_Dropdown shadowResolutionMenu;
		[SerializeField] private TMP_Dropdown shadowCascadesMenu;

		[Header("Other")]
        [SerializeField] private Button backButton;
		[SerializeField] private Button applyButton;
		#endregion

		#region FIELDS
		private Resolution[] availableResolutions;
		#endregion

		private void Awake()
		{
			ReestablishSettings(); 
		}

		private void OnEnable()
		{
			applyButton.onClick.AddListener(ApplySettings);
		}

		private void OnDisable()
		{
			applyButton.onClick.RemoveListener(ApplySettings);
		}

		private void ApplySettings()
		{
			Screen.fullScreen = displayModeMenu.value == 0 ? true : false;

			Resolution currentResolution = availableResolutions[resolutionMenu.value];
			Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);

			QualitySettings.anisotropicFiltering = (AnisotropicFiltering)anisotropicTexturesMenu.value;
			QualitySettings.antiAliasing = antiAliasingMenu.value * 2;
			QualitySettings.masterTextureLimit = textureQualityMenu.value;
			QualitySettings.shadows = (ShadowQuality)shadowsMenu.value;
			QualitySettings.shadowResolution = (ShadowResolution)shadowResolutionMenu.value;
			QualitySettings.shadowCascades = shadowCascadesMenu.value;
		}

		private void SetupResolutionDropdown()
		{
			List<string> options = new();
			System.Text.StringBuilder stringBuilder = new();
			int currentResolutionIndex = 0;

			availableResolutions = Screen.resolutions;
			resolutionMenu.ClearOptions();

			for(int i = 0; i < availableResolutions.Length; i++)
			{
				stringBuilder.Clear();
				stringBuilder.Append(availableResolutions[i].width);
				stringBuilder.Append("x");
				stringBuilder.Append(availableResolutions[i].height);

				options.Add(stringBuilder.ToString());

				if (availableResolutions[i].Equals(Screen.currentResolution))
					currentResolutionIndex = i;

			}
			
			resolutionMenu.AddOptions(options);
			resolutionMenu.value = currentResolutionIndex;
			resolutionMenu.RefreshShownValue();
		}

		private void ReestablishSettings()
		{
			SetupResolutionDropdown();
			displayModeMenu.value = (int)Screen.fullScreenMode;
			displayModeMenu.RefreshShownValue();

			anisotropicTexturesMenu.value = (int)QualitySettings.anisotropicFiltering;
			anisotropicTexturesMenu.RefreshShownValue();

			antiAliasingMenu.value = QualitySettings.antiAliasing / 2;
			antiAliasingMenu.RefreshShownValue();

			textureQualityMenu.value = QualitySettings.masterTextureLimit;
			textureQualityMenu.RefreshShownValue();

			shadowsMenu.value = (int)QualitySettings.shadows;
			shadowsMenu.RefreshShownValue();

			shadowResolutionMenu.value = (int)QualitySettings.shadowResolution;
			shadowResolutionMenu.RefreshShownValue();

			shadowCascadesMenu.value = QualitySettings.shadowCascades;
			shadowCascadesMenu.RefreshShownValue();
		}
		
		public void Initialize(UnityAction backButtonDelegate)
		{
			backButton.onClick.RemoveAllListeners();
			backButton.onClick.AddListener(backButtonDelegate);
		}
	}
}

public static class TMP_DropdownExtension
{
	public static int FindOption(this TMP_Dropdown dropdown, string match)
	{
		for (int i = 0; i < dropdown.options.Count; i++)
		{
			if (dropdown.options[i].text == match)
				return i;
		}

		return -1;
	}
}