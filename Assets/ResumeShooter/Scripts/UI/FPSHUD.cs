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
			gameMode.OnGameEnded.AddListener(OnGameEnded);
		}

		private void OnDisable()
		{
			gameMode.OnGameEnded.RemoveListener(OnGameEnded);
		}

		private void InitializeHUD()
		{
			playerHUD = CreateWidget<PlayerHUD>(playerHUD);

			playerHUD.gameObject.SetActive(true);
		}

		private void OnGameEnded(bool isPlayerWinner)
		{
			playerHUD.gameObject.SetActive(false);

			if (isPlayerWinner)
				CreateWidget<EndGameMenu>(victoryWidget);
			else
				CreateWidget<EndGameMenu>(lossWidget);
		}
	}
}