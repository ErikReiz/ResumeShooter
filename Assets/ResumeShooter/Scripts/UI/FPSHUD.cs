using UnityEngine;
using ResumeShooter.Services;

namespace ResumeShooter.UI
{

	public class FPSHUD : HUDBase
	{
		#region SERIALIZE FIELDS
		[SerializeField] private PlayerHUD playerHUD;
		[SerializeField] private EndGameMenu lossWidget;
		[SerializeField] private EndGameMenu victoryWidget;
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

		private void OnGameEnded(bool isPlayerWinner)
		{
			playerHUD.gameObject.SetActive(false);

			if (isPlayerWinner)
				victoryWidget.gameObject.SetActive(true);
			else
				lossWidget.gameObject.SetActive(true);
		}
	}
}