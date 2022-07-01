using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHUD : HUDBase
{
	#region SERIALIZE FIELDS
	[SerializeField] PlayerHUD playerHUD;
	[SerializeField] EndGameMenu lossWidget;
	[SerializeField] EndGameMenu victoryWidget;
	#endregion

	#region FIELDS
	private GameModeBase gameMode;
	#endregion

	protected override void AfterAwake()
	{
		InitializeHUD();
		gameMode = ServiceManager.GetGameMode();
	}

	private void OnEnable()
	{
		gameMode.OnGameEnded += OnGameEnded;
	}

	private void OnDisable()
	{
		gameMode.OnGameEnded -= OnGameEnded;
	}

	private void InitializeHUD()
	{
		playerHUD = CreateWidget<PlayerHUD>(playerHUD);
		lossWidget = CreateWidget<EndGameMenu>(lossWidget);
		victoryWidget = CreateWidget<EndGameMenu>(victoryWidget);

		playerHUD.gameObject.SetActive(true);
		lossWidget.gameObject.SetActive(false);
		victoryWidget.gameObject.SetActive(false);
	}

	private void OnGameEnded(bool isPlayerWon)
	{
		Destroy(playerHUD);

		if (isPlayerWon)
			victoryWidget.enabled = true;
		else
			lossWidget.enabled = true;
	}
}
