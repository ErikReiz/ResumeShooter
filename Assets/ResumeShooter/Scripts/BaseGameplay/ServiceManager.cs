using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceManager
{
	#region FIELDS
	private static GameModeBase gameMode;
	private static FPCharacter player;
	private static HUDBase hud;
	#endregion

	public static GameModeBase GetGameMode()
	{
		if(!gameMode)
			gameMode = Object.FindObjectOfType<GameModeBase>();

		return gameMode;
	}

	public static FPCharacter GetPlayer()
	{
		if (!player)
			player = Object.FindObjectOfType<FPCharacter>();

		return player;
	}

	public static HUDBase GetHUD()
	{
		if (!hud)
			hud = Object.FindObjectOfType<HUDBase>();

		return hud;
	}
}
