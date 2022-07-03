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

		if (waveSetup.WaveSize < waveSetup.EnemyCountToSpawn)
			waveSetup.WaveSize = waveSetup.EnemyCountToSpawn + 1;

		enemyCounter = (uint)FindObjectsOfType<ZombieAI>().Length;
		if (enemyCounter <= waveSetup.EnemyCountToSpawn)
			StartCoroutine(EnemySpawner());
	}

	public override void CharacterKilled(Object characterKilled)
	{
		base.CharacterKilled(characterKilled);

		if(characterKilled is ZombieAI)
		{
			enemyCounter--;
			if(waveSetup.WaveCount == 0) { return; }

			if (enemyCounter <= waveSetup.EnemyCountToSpawn && !isWaveSpawnerActive)
				StartCoroutine(EnemySpawner());
		}
	}

	private IEnumerator EnemySpawner()
	{
		isWaveSpawnerActive = true;
		yield return new WaitForSeconds(waveSetup.TimeDelayBetweenWaves);
		waveSetup.WaveCount--;

		for(int i = 0; i < waveSetup.WaveSize; i++)
		{
			int spawnPointsCount = waveSetup.SpawnPoints.Count;

			Transform spawnPointTransform = waveSetup.SpawnPoints[Random.Range(0, spawnPointsCount)].transform;
			GameObject enemyToSpawn = waveSetup.EnemyPool.GenerateEnemy();

			if (enemyToSpawn)
			{
				Instantiate(enemyToSpawn, spawnPointTransform);
				enemyCounter++;
			}

			yield return new WaitForSeconds(waveSetup.SpawnDelay);
		}
		
		SetNewWave();
	}

	private void SetNewWave()
	{
		isWaveSpawnerActive = false;
		float increasePercent = 1 + (float)waveSetup.WaveIncreasePercent / 100;

		waveSetup.WaveSize = (uint)Mathf.FloorToInt(waveSetup.WaveSize * increasePercent);
		if (waveSetup.IncreaseTimeDelayBetweenWaves)
			waveSetup.TimeDelayBetweenWaves *= increasePercent;

	}
}
