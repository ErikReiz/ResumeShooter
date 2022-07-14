using UnityEngine;
using ResumeShooter.UI;
using ResumeShooter.Player;

namespace ResumeShooter.Services
{

	public static class ServiceManager
	{
		#region PROPERTIES
		public static AudioManager AudioSpawner { get { return audioSpawner; } }
		#endregion

		#region FIELDS
		private static GameModeBase gameMode;
		private static FPCharacter player;
		private static HUDBase hud;
		private static AudioManager audioSpawner;
		private static GameObject gameModeObject;
		private static NonMonoBehaviourCorotine nonMonoBehaviourCorotine;
		#endregion

		#region INITIALIZE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			if (!GetGameMode())
				InitializeGameMode();

			if (!GetHUD())
				InitializeHUD();

			if (!GetNonMonoBehaviourCorotine())
				InitializeNonMonoBehaviourCorotine();

			InitializeAudioManager();
		}

		private static void InitializeGameMode()
		{
			if (!gameModeObject)
				gameModeObject = new GameObject("Game Mode");

			gameMode = gameModeObject.AddComponent<GameModeBase>();
		}

		private static void InitializeHUD()
		{
			if (!gameModeObject)
				gameModeObject = new GameObject("Game Mode");

			hud = gameModeObject.AddComponent<HUDBase>();
		}

		private static void InitializeAudioManager()
		{
			GameObject audioManagerObject = new GameObject("Audio Manager");
			Object.DontDestroyOnLoad(audioManagerObject);

			audioSpawner = audioManagerObject.AddComponent<AudioManager>();
		}

		private static void InitializeNonMonoBehaviourCorotine()
		{
			if (!gameModeObject)
				gameModeObject = new GameObject("Game Mode");

			nonMonoBehaviourCorotine = gameModeObject.AddComponent<NonMonoBehaviourCorotine>();
		}
		#endregion

		#region GET
		public static GameModeBase GetGameMode()
		{
			if (!gameMode)
				gameMode = Object.FindObjectOfType<GameModeBase>();

			return gameMode;
		}

		public static FPCharacter GetPlayer()
		{
			if (!player)
				player = Object.FindObjectOfType<FPCharacter>();

			return player;
		}

		public static HUDBase GetHUD()
		{
			if (!hud)
				hud = Object.FindObjectOfType<HUDBase>();

			return hud;
		}

		public static NonMonoBehaviourCorotine GetNonMonoBehaviourCorotine()
		{
			if (!nonMonoBehaviourCorotine)
				nonMonoBehaviourCorotine = Object.FindObjectOfType<NonMonoBehaviourCorotine>();

			return nonMonoBehaviourCorotine;
		}
		#endregion
	}
}