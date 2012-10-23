/*Constants.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 4 2012
 * @Updated Oct 5 2012
 * 
 */ 

using System;
using UnityEngine;

public enum ArmorType
{
	Metal = 0,
	Energy = 1
}

public enum AttackType
{
	Unassigned = -1,
	Bomb = 0,
	Beam = 1,
	Slug = 2
}

public enum SpaceZone
{
	Bottom = -10,
	Low = -5,
	Middle = 8,
	High = 18,
	Top = 27
}

public enum EnemyShipType
{
	Fighter = 1,
	Cruiser = 2,
	Frigate = 3,
	PlanetDecimator = 4,
	Carrier = 5,
	Battleship = 6
}

public class Constants
{
	//Enemy Ship number constants
	public const int MAX_FIGHTERS = 15;
	public const int MAX_CRUISERS = 8;
	public const int MAX_FRIGATES = 6;
	public const int MAX_PLANET_DECIMATORS = 2;
	public const int MAX_CARRIERS = 4;
	public const int MAX_BATTLESHIPS = 1;
	
	//Enemy Ship Score and Resource reward constants
	public const int BS_RESOURCE_REWARD = 50;
	public const int CARRIER_RESOURCE_REWARD = 25;
	public const int CRUISER_RESOURCE_REWARD = 10;
	public const int FIGHTER_RESOURCE_REWARD = 5;
	public const int FRIGATE_RESOURCE_REWARD = 20;
	public const int PD_RESOURCE_REWARD = 20;
	
	
	//Game time logic constants
	public const float TIME_BETWEEN_ROUNDS = 1.0f; //TODO: Give accurate number here after done with preliminary tests
	
	//Turret Construction costs (TODO: Balance)
	public const int BC_CONSTRUCTION_COST = 10;
	public const int ML_CONSTRUCTION_COST = 10;
	public const int SLUG_CONSTRUCTION_COST = 10;
	public const int MD_CONSTRUCTION_COST = 10;
	//Turret Default Starting Attributes Constants
	//BeamCannon (TODO: Balance)
	public const int DEFAULT_BC_HEALTH = 1;
	public const int DEFAULT_BC_MAX_HEALTH = 1;
	public const float DEFAULT_BC_RELOAD_TIME = 1.0f;
	public const int DEFAULT_BC_DAMAGE = 1;
	public const float DEFAULT_BC_FLIGHT_TIME = 1.0f;
	//MissileLauncher (TODO: Balance)
	public const int DEFAULT_ML_HEALTH = 1;
	public const int DEFAULT_ML_MAX_HEALTH = 1;
	public const float DEFAULT_ML_RELOAD_TIME = 1.0f;
	public const int DEFAULT_ML_DAMAGE = 1;
	public const float DEFAULT_ML_PROJECTILE_SPEED = 5.0f;
	//SlugCannon (TODO: Balance)
	public const int DEFAULT_SLUG_HEALTH = 1;
	public const int DEFAULT_SLUG_MAX_HEALTH = 1;
	public const float DEFAULT_SLUG_RELOAD_TIME = 1.0f;
	public const int DEFAULT_SLUG_DAMAGE = 1;
	public const float DEFAULT_SLUG_PROJECTILE_SPEED = 5.0f;
	//MineralDrill (TODO: Balance)
	public const int DEFAULT_MD_HEALTH = 1;
	public const int DEFAULT_MD_MAX_HEALTH = 1;
	public const int DEFAULT_MD_RESOURCE_RATE = 10;
	
	public Constants ()
	{
	}
	
	/* Example constant method... this is actually out of place sort of.
	//Returns the prefab identification string, given an ObstacleKind
	public static string GetObstaclePrefabString(ObstacleKind kind)
	{
		string returnString = "FAIL"; 
		switch (kind)
		{
			case ObstacleKind.Cactus:
				returnString = "Cactus";
				break;
			case ObstacleKind.Bird:
				returnString = "Bird";
				break;
			case ObstacleKind.Quicksand:
				returnString = "Quicksand";
				break;
			default:
				Debug.LogError("ERROR, ObstacleKind not represented in getObstaclePrefabString");
				break;
		}
		return returnString;
	}*/
}

