using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGameMode : GameModeBase
{
	#region SERIALIZE FIELDS
	[SerializeField] private WaveSetup waveSetup;
	#endregion

	#region FIELDS
	private uint enemyCounter = 0;
	private bool isWaveSpawnerActive = false;
	#endregion

	protected override void BeginPlay()
	{
		waveSetup.FindSpawnPoints();
		SetFirstWave();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void SetFirstWave()
	{
		if (waveSetup.SpawnPoints.Count == 0)
			return;

		Debug.Log("ds");

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
			int spawnPointsCount = waveSetup.SpawnPoints.Count;

			Transform spawnPointTransform = waveSetup.SpawnPoints[Random.Range(0, spawnPointsCount)].transform;
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
