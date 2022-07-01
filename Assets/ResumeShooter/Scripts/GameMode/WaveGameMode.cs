using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGameMode : GameModeBase
{
	#region SERIALIZE FIELDS
	[SerializeField] private WaveSetup waveSetup;
	#endregion

	#region FIELDS
	private GameObject[] spawnPoints;
	private uint enemyCounter = 0;
	private bool isWaveSpawnerActive = false;
	#endregion

	protected override void BeginPlay()
	{
		FindSpawnPoints();
		SetFirstWave();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void FindSpawnPoints()
	{// заменить на поиск по классу
		spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
	}

	private void SetFirstWave()
	{
		if (spawnPoints.Length == 0)
		{
			waveSetup.waveSize = 0;
			return;
		}

		if (waveSetup.waveSize < waveSetup.enemyCountToSpawn)
			waveSetup.waveSize = waveSetup.enemyCountToSpawn + 1;

		enemyCounter = (uint)FindObjectsOfType<ZombieAI>().Length;
		if (enemyCounter <= waveSetup.enemyCountToSpawn)
			StartCoroutine(EnemySpawner());
	}

	public override void CharacterKilled(Object characterKilled)
	{
		base.CharacterKilled(characterKilled);

		if(characterKilled is ZombieAI)
		{
			enemyCounter--;
			if(waveSetup.waveCount == 0) { return; }

			if (enemyCounter <= waveSetup.enemyCountToSpawn && !isWaveSpawnerActive)
				StartCoroutine(EnemySpawner());
		}
	}

	private IEnumerator EnemySpawner()
	{
		isWaveSpawnerActive = true;
		yield return new WaitForSeconds(waveSetup.timeDelayBetweenWaves);
		waveSetup.waveCount--;

		for(int i = 0; i < waveSetup.waveSize; i++)
		{
			Transform spawnPointTransform = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
			GameObject enemyToSpawn = waveSetup.enemyPool.GenerateEnemy();

			if (enemyToSpawn)
			{
				Instantiate(enemyToSpawn, spawnPointTransform);
				enemyCounter++;
			}

			yield return new WaitForSeconds(waveSetup.spawnDelay);
		}
		
		SetNewWave();
	}

	private void SetNewWave()
	{
		isWaveSpawnerActive = false;
		float increasePercent = 1 + (float)waveSetup.waveIncreasePercent / 100;

		waveSetup.waveSize = (uint)Mathf.FloorToInt(waveSetup.waveSize * increasePercent);
		if (waveSetup.increaseTimeDelayBetweenWaves)
			waveSetup.timeDelayBetweenWaves *= increasePercent;

	}
}
