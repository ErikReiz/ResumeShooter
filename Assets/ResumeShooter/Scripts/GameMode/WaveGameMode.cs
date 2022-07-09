using System.Collections;
using UnityEngine;

public class WaveGameMode : GameModeBase
{
	#region SERIALIZE FIELDS
	[SerializeField] private WaveSetup waveSetup;
	#endregion

	#region FIELDS
	private bool isWaveSpawnerActive = false;

	private float tempWaveSize;
	private uint enemyCounter = 0;
	#endregion

	protected override void BeginPlay()
	{
		waveSetup.FindSpawnPoints();
		tempWaveSize = waveSetup.WaveSize;

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

		StartCoroutine(EnemySpawner());
	}

	public override void CharacterKilled(bool isPlayer)
	{
		base.CharacterKilled(isPlayer);

		if (!isPlayer)
		{
			enemyCounter--;
			WinAndContinueGameCondition();
		}

		void WinAndContinueGameCondition()
		{
			if (enemyCounter == 0 && waveSetup.WaveCount == 0)
			{
				EndGame(true);
			}
			else if (enemyCounter <= waveSetup.EnemyCountToSpawn && !isWaveSpawnerActive)
			{
				StartCoroutine(EnemySpawner());
			}
		}
	}

	private IEnumerator EnemySpawner()
	{
		isWaveSpawnerActive = true;

		yield return new WaitForSeconds(waveSetup.TimeDelayBetweenWaves);

		waveSetup.WaveCount--;

		for (int i = 0; i < waveSetup.WaveSize; i++)
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

		tempWaveSize = tempWaveSize * increasePercent;
		int intTempWaveSize = Mathf.RoundToInt(tempWaveSize);

		if (intTempWaveSize > waveSetup.WaveSize)
		{
			waveSetup.WaveSize = (uint)intTempWaveSize;
			tempWaveSize = intTempWaveSize;
		}

		if (waveSetup.IncreaseTimeDelayBetweenWaves)
			waveSetup.TimeDelayBetweenWaves *= increasePercent;

	}
}
