using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGameMode : FPSGameMode
{
	#region SERIALIZE FIELDS
	[Header("General")]
	[Tooltip("Set number of enemies when spawn method will be called")]
	[SerializeField] private uint enemyCountToSpawn = 5;
	[SerializeField] private uint waveCount = 5;
	[Tooltip("Sets wave size")]
	[SerializeField] private uint waveSize = 10;
	[Tooltip("Every wave will be increased by that percent")]
	[SerializeField] private uint waveIncreasePercent = 10;
	[SerializeField] private float timeDelayBetweenWaves = 10f;
	[Tooltip("If true, will be increased by waveIncreasePercent")]
	[SerializeField] private bool increaseTimeDelayBetweenWaves = true;

	[Header("Spawn Info")]
	[Tooltip("Enemies and chances to be spawned")]
	[SerializeField] private WavePrefab enemyPool;
	[SerializeField] private Transform[] spawnPoints;
	[Tooltip("Delay between enemy spawns")]
	[SerializeField] private float spawnDelay = 0.1f;

	#endregion

	#region FIELDS
	private uint enemyCounter = 0;
	private bool isWaveSpawnerActive = false;
	#endregion

	protected override void Start()
	{
		base.Start();

		SetFirstWave();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void SetFirstWave()
	{
		if (spawnPoints.Length == 0)
		{
			waveSize = 0;
			return;
		}

		if (waveSize < enemyCountToSpawn)
			waveSize = enemyCountToSpawn + 1;

		enemyCounter = (uint)FindObjectsOfType<ZombieAI>().Length;
		if (enemyCounter <= enemyCountToSpawn)
			StartCoroutine(EnemySpawner());
	}

	public override void CharacterKilled(Object characterKilled)
	{
		base.CharacterKilled(characterKilled);

		if(characterKilled is ZombieAI)
		{
			enemyCounter--;
			if(waveCount == 0) { return; }

			if (enemyCounter <= enemyCountToSpawn && !isWaveSpawnerActive)
				StartCoroutine(EnemySpawner());
		}

	}

	private IEnumerator EnemySpawner()
	{
		isWaveSpawnerActive = true;
		yield return new WaitForSeconds(timeDelayBetweenWaves);
		waveCount--;

		for(int i = 0; i < waveSize; i++)
		{
			Transform spawnPointTransform = spawnPoints[Random.Range(0, spawnPoints.Length)];
			GameObject enemyToSpawn = enemyPool.GenerateEnemy();

			if (enemyToSpawn)
			{
				Instantiate(enemyToSpawn, spawnPointTransform);
				enemyCounter++;
			}

			yield return new WaitForSeconds(spawnDelay);
		}
		
		SetNewWave();
	}

	private void SetNewWave()
	{
		isWaveSpawnerActive = false;
		float increasePercent = 1 + (float)waveIncreasePercent / 100;

		waveSize = (uint)Mathf.FloorToInt(waveSize * increasePercent);
		if (increaseTimeDelayBetweenWaves)
			timeDelayBetweenWaves *= increasePercent;

	}
}
