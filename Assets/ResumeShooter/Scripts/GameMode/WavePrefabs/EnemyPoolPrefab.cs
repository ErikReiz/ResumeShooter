using System.Collections.Generic;
using UnityEngine;
using ResumeShooter.Services;
using ResumeShooter.AI;

[CreateAssetMenu(fileName = "Enemy Pool Prefab")]
public class EnemyPoolPrefab : ScriptableObject
{
	#region SERIALIZE FIELDS
	[Tooltip("Sets chance and enemy to spawn")]
	[SerializeField] private SerializableDictionary<ZombieAI, uint> enemyList;

	[Tooltip("If true generate enemy method will always return enemy")]
	[SerializeField] private bool alwaysGenerateEnemy = true;
	#endregion

	#region FIELDS
	private KeyValuePair<ZombieAI, uint> defaultEnemy; // This enemy will be spawned if chance smaller than minimal in enemyList
	#endregion

	public ZombieAI GenerateEnemy()
	{
		if (!defaultEnemy.Key)
			SetDefaultEnemy();

		uint chance = (uint)Random.Range(0, 100);

		if (alwaysGenerateEnemy)
		{
			if (chance > defaultEnemy.Value)
				return defaultEnemy.Key;
		}

		int keyIndex = Random.Range(0, enemyList.Count - 1);

		if (enemyList.ValuesArray[keyIndex] >= chance)
			return enemyList.KeysArray[keyIndex];

		return null;
	}

	private void SetDefaultEnemy()
	{
		foreach (var enemy in enemyList)
		{
			if (enemy.Value > defaultEnemy.Value)
				defaultEnemy = enemy;
		}
	}

}
