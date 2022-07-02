using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Setup")]
public class WaveSetup : ScriptableObject
{ //TODO заменить все на свойства
	#region SERIALIZE FIELDS
	[Header("General")]
	[Tooltip("Set number of enemies when spawn method will be called")]
	[SerializeField] public uint enemyCountToSpawn = 5;
	[SerializeField] public uint waveCount = 5;
	[Tooltip("Sets wave size")]
	[SerializeField] public uint waveSize = 10;
	[Tooltip("Every wave will be increased by that percent")]
	[SerializeField] public uint waveIncreasePercent = 10;
	[SerializeField] public float timeDelayBetweenWaves = 10f;
	[Tooltip("If true, will be increased by waveIncreasePercent")]
	[SerializeField] public bool increaseTimeDelayBetweenWaves = true;

	[Header("Spawn Info")]
	[Tooltip("Enemies and chances to be spawned")]
	[SerializeField] public EnemyPoolPrefab enemyPool;
	[Tooltip("Delay between enemy spawns")]
	[SerializeField] public float spawnDelay = 0.1f;
	#endregion

	#region PROPERTIES
	public List<SpawnPoint> SpawnPoints { get; private set; }
	#endregion

	public void FindSpawnPoints()
	{
		foreach(var spawnPoint in FindObjectsOfType<SpawnPoint>())
		{

			if (spawnPoint.IsPlayerSpawn)
				continue;

			SpawnPoints.Add(spawnPoint);
		}
	}

}