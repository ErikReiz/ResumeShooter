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
	}

	private void Update()
	{
		UpdateHealthBar();
	}

	private void UpdateHealthBar()
	{
		healthBarSlider.value = player.HealthPercents;
	}
}
