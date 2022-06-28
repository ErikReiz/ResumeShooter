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
	private FPCharacter player = null;
	#endregion

	protected virtual void Awake()
	{
		CheckIsSingleton();
	}

	protected virtual void Start()
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

	public virtual void CharacterKilled(Object characterKilled)
	{
		if (characterKilled is FPCharacter)
		{
			EndGame(false);
		}
	}

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

	public FPCharacter GetPlayer()
	{
		if (!player)
			player = FindObjectOfType<FPCharacter>();

		return player;
	}
}
