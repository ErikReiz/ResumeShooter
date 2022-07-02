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
	private SpawnPoint playerStart;
	#endregion

	private void Awake()
	{
		CheckIsSingleton();
		SpawnPlayer();
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

	private void SpawnPlayer()
	{
		FPCharacter character = ServiceManager.GetPlayer();

		if (character)
			player = character;
		else
		{
			playerStart = FindPlayerStart();

			try
			{
				player = playerStart.SpawnCharacter(player);
			}
			catch
			{
				player = Instantiate(player, Vector3.zero, Quaternion.identity);
				throw;
			}
		}
	}

	private SpawnPoint FindPlayerStart()
	{
		foreach(var point in FindObjectsOfType<SpawnPoint>())
		{
			if (point.IsPlayerSpawn)
				return point;
		}

		return null;
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
