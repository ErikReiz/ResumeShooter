using UnityEngine;
using UnityEngine.Events;
using ResumeShooter.Player;

namespace ResumeShooter.Services
{

	public class GameModeBase : MonoBehaviour
	{
		#region FIELDS
		[HideInInspector]
		public UnityEvent<bool> OnGameEnded;

		private static GameModeBase instance;
		protected IDamageable playerDamagableComponent;
		#endregion

		private void Awake()
		{
			CheckIsSingleton();
			InitializePlayer();
		}

		private void Start()
		{
			BeginPlay();
		}

		private void CheckIsSingleton()
		{
			if (instance == null)
				instance = this;
			else
				Destroy(gameObject);
		}

		private void InitializePlayer()
		{
			FPCharacter player = ServiceManager.GetPlayer();

			if(player)
				playerDamagableComponent = player.GetComponent<IDamageable>();
		}

		protected virtual void BeginPlay() { }

		public virtual void CharacterKilled(bool isPlayer)
		{
			if (isPlayer)
			{
				EndGame(false);
			}
		}

		protected void EndGame(bool isPlayerWinner)
		{
			Time.timeScale = 0f;
			OnGameEnded.Invoke(isPlayerWinner);
		}
	}
}