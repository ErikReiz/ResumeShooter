using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Setup")]
public class WaveSetup : ScriptableObject
{
	#region SERIALIZE FIELDS
	[Header("General")]
	[Tooltip("Set number of enemies when spawn method will be called")]
	[SerializeField] public uint EnemyCountToSpawn = 5;
	[SerializeField] public uint WaveCount = 5;
	[Tooltip("Sets wave size")]
	[SerializeField] public uint WaveSize = 10;
	[Tooltip("Every wave will be increased by that percent")]
	[SerializeField] public uint WaveIncreasePercent = 10;
	[SerializeField] public float TimeDelayBetweenWaves = 10f;
	[Tooltip("If true, will be increased by waveIncreasePercent")]
	[SerializeField] public bool IncreaseTimeDelayBetweenWaves = true;

	[Header("Spawn Info")]
	[Tooltip("Enemies and chances to be spawned")]
	[SerializeField] public EnemyPoolPrefab EnemyPool;
	[Tooltip("Delay between enemy spawns")]
	[SerializeField] public float SpawnDelay = 0.1f;
	#endregion

	#region PROPERTIES
	public List<SpawnPoint> SpawnPoints { get; private set; }
	#endregion

	public void FindSpawnPoints()
	{
		SpawnPoints.Clear();

		foreach(var spawnPoint in FindObjectsOfType<SpawnPoint>())
		{

			if (spawnPoint.IsPlayerSpawn)
				continue;

			Debug.Log(spawnPoint.gameObject);
			SpawnPoints.Add(spawnPoint);
		}
	}

}