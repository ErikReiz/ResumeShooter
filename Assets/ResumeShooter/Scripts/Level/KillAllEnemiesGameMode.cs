using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllEnemiesGameMode : FPSGameMode
{
	public override void CharacterKilled(Object characterKilled)
	{
		if (characterKilled is FPCharacter)
		{
			EndGame(false);
		}
		else if(characterKilled is ZombieAI)
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
