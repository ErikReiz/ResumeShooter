using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveSetup
{ 
	#region SERIALIZE FIELDS
	[Header("General")]
	[Tooltip("If true, will be increased by waveIncreasePercent")]
	[SerializeField] public bool IncreaseTimeDelayBetweenWaves;

	[Tooltip("Set number of enemies when spawn method will be called")]
	[SerializeField] public uint EnemyCountToSpawn;

	[SerializeField] public uint WaveCount;

	[Tooltip("Sets wave size")]
	[SerializeField] public uint WaveSize;

	[Tooltip("Every wave will be increased by that percent")]
	[SerializeField] public uint WaveIncreasePercent;

	[SerializeField] public float TimeDelayBetweenWaves;

	[Header("Spawn Info")]
	[Tooltip("Enemies and chances to be spawned")]
	[SerializeField] public EnemyPoolPrefab EnemyPool;

	[Tooltip("Delay between enemy spawns")]
	[SerializeField] public float SpawnDelay;
	#endregion

	#region PROPERTIES
	public List<SpawnPoint> SpawnPoints { get; private set; }
	#endregion

	public void FindSpawnPoints()
	{
		if (SpawnPoints == null)
			SpawnPoints = new();

		SpawnPoints.Clear();

		foreach (var spawnPoint in Object.FindObjectsOfType<SpawnPoint>())
		{

			if (spawnPoint.IsPlayerSpawn)
				continue;

			SpawnPoints.Add(spawnPoint);
		}
	}

}