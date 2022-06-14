using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdater : MonoBehaviour
{
	#region FIELDS
	private Slider healthBarSlider;
	private FPCharacter player;
	#endregion

	private void Start()
	{
		healthBarSlider = GetComponent<Slider>();
		player = FindObjectOfType<FPCharacter>();
		player.OnPlayerDamaged += UpdateHealthBar;
	}

	private void UpdateHealthBar(float heatlhPercents)
	{
		healthBarSlider.value = heatlhPercents;
	}
}
