using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllEnemiesGameMode : GameModeBase
{
	public override void CharacterKilled(Object characterKilled)
	{
		base.CharacterKilled(characterKilled);

		if(characterKilled is ZombieAI)
		{
			foreach (var enemy in FindObjectsOfType<ZombieAI>())
			{
				if(!enemy.IsDead)
				{
					return;
				}
			}

		EndGame(true);
		}
	}
}
