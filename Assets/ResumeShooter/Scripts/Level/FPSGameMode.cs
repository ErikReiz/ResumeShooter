using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGameMode : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] GameObject gameHUD;
	[SerializeField] GameObject deathHUD;
	#endregion

	#region FIELDS
	private static FPSGameMode instance;
	#endregion

	private void Awake()
	{
		CheckIsSingleton();
	}

	private void Start()
	{
		gameHUD.SetActive(true);
		deathHUD.SetActive(false);
	}

	private void CheckIsSingleton()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	public virtual void CharacterKilled(Object characterKilled) { }

	protected void EndGame(bool isPlayerWinner)
	{
		Time.timeScale = 0f;
		gameHUD.SetActive(false);

		if (isPlayerWinner)
		{
			deathHUD.SetActive(true);
		}
		else
		{
			deathHUD.SetActive(true);
		}

	}
}
