using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameModeBase : MonoBehaviour
{
	#region SERIALIZE FIELDS
	[SerializeField] private FPCharacter player;
	#endregion

	#region FIELDS
	public UnityAction<bool> OnGameEnded;

	private static GameModeBase instance;
	private PlayerStart playerStart;
	private HUDBase hudClass;
	#endregion

	private void Awake()
	{
		CheckIsSingleton();
		InitializeGameMode();
	}

	private void Start()
	{
		BeginPlay();
	}

	private void InitializeGameMode()
	{
		SpawnPlayer();
		SpawnHUD();
	}

	private void CheckIsSingleton()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void SpawnHUD()
	{
		HUDBase hud = FindObjectOfType<HUDBase>();

		if (hud)
			hudClass = hud;
		else
			hud = gameObject.AddComponent<HUDBase>();
	}

	private void SpawnPlayer()
	{
		FPCharacter character = FindObjectOfType<FPCharacter>();

		if (character)
			player = character;
		else
		{
			playerStart = FindObjectOfType<PlayerStart>();

			try
			{
				player = playerStart.SpawnPlayer();
			}
			catch
			{
				player = Instantiate(player, Vector3.zero, Quaternion.identity);
				throw;
			}
		}
	}

	protected virtual void BeginPlay() { }

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
		OnGameEnded.Invoke(isPlayerWinner);
	}
}
