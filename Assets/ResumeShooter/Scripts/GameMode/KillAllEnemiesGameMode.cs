public class KillAllEnemiesGameMode : GameModeBase
{
	public override void CharacterKilled(bool isPlayer)
	{
		base.CharacterKilled(isPlayer);

		if (!isPlayer)
		{
			foreach (var enemy in FindObjectsOfType<HealthComponent>())
			{
				if (enemy.IsPlayer)
					continue;

				if (!enemy.IsDead)
					return;
			}
			EndGame(true);
		}
	}
}
