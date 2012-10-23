/*WaveGenerator.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 4 2012
 * @Updated Aug 4 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

/*
 * This class will be used to control the generation of enemy attack waves
 */
public class WaveGenerator : MonoBehaviour {
	
	public int currentWave;
	public bool waveChanged;
	public float spawnTimer;
	//Fighters
	private int numFighters;
	private int fightersLevel;
	//Cruiser
	private int numCruisers;
	private int cruisersLevel;
	//Frigate
	private int numFrigates;
	private int frigatesLevel;
	//Planet Decimator
	private int numPlanetDecimators;
	private int planetDecimatorsLevel;
	//Carrier
	private int numCarriers;
	private int carriersLevel;
	//Battleship
	private int numBattleships;
	private int battleshipsLevel;
	
	//this spawn pool will be used to randomly choose which ship gets spawned
	public ArrayList spawnPool = new ArrayList();
	private float timeSinceLastSpawn; //time since the last ship was spawned in seconds
	
	OTSprite myShip;
	
	GameState gameState;
	
	// Use this for initialization
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		currentWave = gameState.currentWave; // In most cases we'll start at wave 1
		waveChanged = true; //we need to set this true so that the first wave gets its rolls
		timeSinceLastSpawn = 100.0f;// This starts at a high number to insure the first wave starts instantly
	}
	
	// Update is called once per frame
	void Update () {
		//Check to see if the wave has changed
		if (currentWave != gameState.currentWave)
		{
			waveChanged = true;
			currentWave = gameState.currentWave;
		}
		//If the wave has changed, then lets roll the values for the next wave
		if (waveChanged)
		{
			spawnTimer = 10.0f/((currentWave*0.1f)+1.0f);
			
			//roll the number of ships
			numFighters = Random.Range(currentWave/2,currentWave+1)+1;
			numCruisers = Random.Range(currentWave/3,currentWave)-3;
			numFrigates = Random.Range(currentWave/4,currentWave)-5;
			numPlanetDecimators = Random.Range(currentWave/6,currentWave)-10;
			numCarriers = Random.Range(currentWave/6,currentWave)-10;
			numBattleships = Random.Range(currentWave/10,currentWave)-15;
			
			//Set the levels of the ships
			fightersLevel = (currentWave/2) +1; //never want a zero here
			cruisersLevel = (currentWave/3) +1;
			frigatesLevel = (currentWave/4) +1;
			planetDecimatorsLevel = (currentWave/6) +1;
			carriersLevel = (currentWave/6)+1;
			battleshipsLevel = (currentWave/10)+1;
			
			//modify the level of the ships based on how many extra beyond the max got rolled
			if (numFighters > Constants.MAX_FIGHTERS)
			{
				fightersLevel+=(numFighters/Constants.MAX_FIGHTERS);
				numFighters = Constants.MAX_FIGHTERS;
			}
			if (numCruisers > Constants.MAX_CRUISERS)
			{
				cruisersLevel+=(numCruisers/Constants.MAX_CRUISERS);
				numCruisers = Constants.MAX_CRUISERS;
			}
			if (numFrigates > Constants.MAX_FRIGATES)
			{
				frigatesLevel+=(numFrigates/Constants.MAX_FRIGATES);
				numFrigates = Constants.MAX_FRIGATES;
			}
			if (numPlanetDecimators > Constants.MAX_PLANET_DECIMATORS)
			{
				planetDecimatorsLevel+=(numPlanetDecimators/Constants.MAX_PLANET_DECIMATORS);
				numPlanetDecimators = Constants.MAX_PLANET_DECIMATORS;
			}
			if (numCarriers > Constants.MAX_CARRIERS)
			{
				carriersLevel+=(numCarriers/Constants.MAX_CARRIERS);
				numCarriers = Constants.MAX_CARRIERS;
			}
			if (numBattleships > Constants.MAX_BATTLESHIPS)
			{
				battleshipsLevel+=(numBattleships/Constants.MAX_BATTLESHIPS);
				numBattleships = Constants.MAX_BATTLESHIPS;
			}
			
			//build the spawn pool
			for (int i = 0; i < numFighters; i++)
				spawnPool.Add(EnemyShipType.Fighter);
			for (int i = 0; i < numCruisers; i++)
				spawnPool.Add(EnemyShipType.Cruiser);
			for (int i = 0; i < numFrigates; i++)
				spawnPool.Add(EnemyShipType.Frigate);
			for (int i = 0; i < numPlanetDecimators; i++)
				spawnPool.Add(EnemyShipType.PlanetDecimator);
			for (int i = 0; i < numCarriers; i++)
				spawnPool.Add(EnemyShipType.Carrier);
			for (int i = 0; i < numBattleships; i++)
				spawnPool.Add(EnemyShipType.Battleship);
			
			//set the wavechanged flag so we don't reroll
			waveChanged = false;
		}
		
		if (!gameState.betweenRounds)
		{
			//For now lets just spit out a basic enemy ship every 5 seconds
			timeSinceLastSpawn += Time.deltaTime;
			if (timeSinceLastSpawn > spawnTimer)
			{
				if (spawnPool.Count > 0)
				SpawnShip();
			}
		}
	}
	
	void SpawnShip()
	{
		//determine which ship to spawn
		int index = Random.Range(0,spawnPool.Count); //The index of the chosen ship in the spawn pool
		EnemyShipType typeToSpawn = (EnemyShipType)spawnPool[index];
		spawnPool.RemoveAt(index); //remove the ship so we dont pick it again
		
		//Handle each ship type
		switch(typeToSpawn)
		{
		case EnemyShipType.Fighter:
			SpawnFighter();
			break;
		case EnemyShipType.Cruiser:
			SpawnCruiser();
			break;
		case EnemyShipType.Frigate:
			SpawnFrigate();
			break;
		case EnemyShipType.PlanetDecimator:
			SpawnPlanetDecimator();
			break;
		case EnemyShipType.Carrier:
			SpawnCarrier();
			break;
		case EnemyShipType.Battleship:
			SpawnBattleship();
			break;
		default:
			print("Error in WaveGenerator.cs switch statement, note that this will break the gamestate count of active ships");
			break;
		}
		
		
		//reset the spawn timer
		timeSinceLastSpawn = 0.0f;
		
		//update game state
		gameState.activeShipCount += 1;
	}
	
	void SpawnFighter()
	{
		//create the game object
		GameObject ship = OT.CreateObject("EnemyShipFighter");
		myShip = ship.GetComponent<OTSprite>();
		
		//place it near the edge of the screen and in the bottom-middle zone
		float shipY = Random.value * (int)(SpaceZone.Middle-SpaceZone.Bottom)+(int)SpaceZone.Bottom;
		myShip.position = new Vector2(-50.0f,shipY);
		myShip.renderer.enabled = true;
		
		//Adjust the new ship's attributes based on level
		EnemyShip shipLogic = (EnemyShip)myShip.GetComponent(typeof(EnemyShip));
		shipLogic.moveSpeed = Fighter.moveSpeedBase+(Fighter.moveSpeedMultiplier*fightersLevel);
		shipLogic.health = Fighter.healthBase+(int)(Fighter.healthMultiplier*fightersLevel);
		
		Fighter fighterLogic = (Fighter)myShip.GetComponent(typeof(Fighter));
		fighterLogic.reloadTime = Fighter.reloadTimeBase-(Fighter.reloadTimeMultiplier*fightersLevel);
		fighterLogic.projectileDamage = Fighter.projectileDamageBase+(int)(Fighter.projectileDamageMultiplier*fightersLevel);
		fighterLogic.projectileSpeed = Fighter.projectileSpeedBase+(Fighter.projectileSpeedMultiplier*fightersLevel);
		fighterLogic.stationary = false;
		fighterLogic.resourceReward = Constants.FRIGATE_RESOURCE_REWARD;
		//Add the Fighter to the targets list
		Targets.enemyTargets.Add(myShip);
		print(Targets.enemyTargets.Count);
	}
	
	void SpawnCruiser()
	{
		//create the game object
		GameObject ship = OT.CreateObject("EnemyShipCruiser");
		myShip = ship.GetComponent<OTSprite>();
		
		//place it near the edge of the screen and in the Low-High zone, minus 4
		float shipY = Random.value * (int)(SpaceZone.High-SpaceZone.Low)+(int)SpaceZone.Low-4.0f;
		myShip.position = new Vector2(-50.0f,shipY);
		myShip.renderer.enabled = true;
		
		//Adjust the new ship's attributes based on level
		EnemyShip shipLogic = (EnemyShip)myShip.GetComponent(typeof(EnemyShip));
		shipLogic.moveSpeed = Cruiser.moveSpeedBase+(Cruiser.moveSpeedMultiplier*cruisersLevel);
		shipLogic.health = Cruiser.healthBase+(int)(Cruiser.healthMultiplier*cruisersLevel);
		
		Cruiser cruiserLogic = (Cruiser)myShip.GetComponent(typeof(Cruiser));
		cruiserLogic.reloadTime = Cruiser.reloadTimeBase-(Cruiser.reloadTimeMultiplier*cruisersLevel);
		cruiserLogic.projectileDamage = Cruiser.projectileDamageBase+(int)(Cruiser.projectileDamageMultiplier*cruisersLevel);
		cruiserLogic.resourceReward = Constants.CRUISER_RESOURCE_REWARD;
		
		//Add the Cruiser to the targets list
		Targets.enemyTargets.Add(myShip);
		print(Targets.enemyTargets.Count);
	}
	
	void SpawnFrigate()
	{
		//create the game object
		GameObject ship = OT.CreateObject("EnemyShipFrigate");
		myShip = ship.GetComponent<OTSprite>();
		
		//place it near the edge of the screen and in the high zone
		float shipY = Random.value * (int)(SpaceZone.High-SpaceZone.Middle)+(int)SpaceZone.Middle;
		myShip.position = new Vector2(50.0f,shipY);
		myShip.renderer.enabled = true;
		
		//Adjust the new ship's attributes based on level
		EnemyShip shipLogic = (EnemyShip)myShip.GetComponent(typeof(EnemyShip));
		shipLogic.moveSpeed = Frigate.moveSpeedBase+(Frigate.moveSpeedMultiplier*frigatesLevel);
		shipLogic.health = Frigate.healthBase+(int)(Frigate.healthMultiplier*frigatesLevel);
		
		Frigate frigateLogic = (Frigate)myShip.GetComponent(typeof(Frigate));
		frigateLogic.reloadTime = Frigate.reloadTimeBase-(Frigate.reloadTimeMultiplier*frigatesLevel);
		frigateLogic.projectileDamage = Frigate.projectileDamageBase+(int)(Frigate.projectileDamageMultiplier*frigatesLevel);
		frigateLogic.projectileSpeed = Frigate.projectileSpeedBase+(Frigate.projectileSpeedMultiplier*frigatesLevel);
		frigateLogic.resourceReward = Constants.FRIGATE_RESOURCE_REWARD;
		Targets.enemyTargets.Add(myShip);
		print(Targets.enemyTargets.Count);
	}
	
	void SpawnPlanetDecimator()
	{
		//create the game object
		GameObject ship = OT.CreateObject("EnemyShipPlanetDecimator");
		myShip = ship.GetComponent<OTSprite>();
		
		//place it near the edge of the screen and in the high zone
		float shipY = Random.value * (int)(SpaceZone.Top-SpaceZone.High)+(int)SpaceZone.High;
		myShip.position = new Vector2(-50.0f,shipY);
		myShip.renderer.enabled = true;
		
		//Adjust the new ship's attributes based on level
		EnemyShip shipLogic = (EnemyShip)myShip.GetComponent(typeof(EnemyShip));
		shipLogic.moveSpeed = PlanetDecimator.moveSpeedBase+(PlanetDecimator.moveSpeedMultiplier*planetDecimatorsLevel);
		shipLogic.health = PlanetDecimator.healthBase+(int)(PlanetDecimator.healthMultiplier*planetDecimatorsLevel);
		
		PlanetDecimator planetDecimatorLogic = (PlanetDecimator)myShip.GetComponent(typeof(PlanetDecimator));
		planetDecimatorLogic.reloadTime = PlanetDecimator.reloadTimeBase-(PlanetDecimator.reloadTimeMultiplier*planetDecimatorsLevel);
		planetDecimatorLogic.projectileDamage = PlanetDecimator.projectileDamageBase+(int)(PlanetDecimator.projectileDamageMultiplier*planetDecimatorsLevel);
		planetDecimatorLogic.projectileSpeed = PlanetDecimator.projectileSpeedBase+(PlanetDecimator.projectileSpeedMultiplier*planetDecimatorsLevel);
		Targets.enemyTargets.Add(myShip);
		planetDecimatorLogic.resourceReward = Constants.PD_RESOURCE_REWARD;
		planetDecimatorLogic.stationary = false;
		print(Targets.enemyTargets.Count);
	}
	
	void SpawnCarrier()
	{
		//create the game object
		GameObject ship = OT.CreateObject("EnemyShipCarrier");
		myShip = ship.GetComponent<OTSprite>();
		
		//place it near the edge of the screen and in the high zone
		float shipY = Random.value * (int)(SpaceZone.Top-SpaceZone.Middle)+(int)SpaceZone.Middle;
		myShip.position = new Vector2(-50.0f,shipY);
		myShip.renderer.enabled = true;
		
		//Adjust the new ship's attributes based on level
		EnemyShip shipLogic = (EnemyShip)myShip.GetComponent(typeof(EnemyShip));
		shipLogic.moveSpeed = Carrier.moveSpeedBase+(Carrier.moveSpeedMultiplier*carriersLevel);
		shipLogic.health = Carrier.healthBase+(int)(Carrier.healthMultiplier*carriersLevel);
		
		Carrier carrierLogic = (Carrier)myShip.GetComponent(typeof(Carrier));
		carrierLogic.reloadTime = Carrier.reloadTimeBase-(Carrier.reloadTimeMultiplier*carriersLevel);
		carrierLogic.projectileDamage = Carrier.projectileDamageBase+(int)(Carrier.projectileDamageMultiplier*carriersLevel);
		carrierLogic.projectileSpeed = Carrier.projectileSpeedBase+(Carrier.projectileSpeedMultiplier*carriersLevel);
		carrierLogic.resourceReward = Constants.CARRIER_RESOURCE_REWARD;
		Targets.enemyTargets.Add(myShip);
		print(Targets.enemyTargets.Count);
	}
	
	void SpawnBattleship()
	{
		//create the game object
		GameObject ship = OT.CreateObject("EnemyShipBattleship");
		myShip = ship.GetComponent<OTSprite>();
		
		//place it near the edge of the screen and in the high zone
		float shipY = Random.value * (int)(SpaceZone.Top-SpaceZone.High)+(int)SpaceZone.High;
		myShip.position = new Vector2(-50.0f,shipY);
		myShip.renderer.enabled = true;
		
		//Adjust the new ship's attributes based on level
		EnemyShip shipLogic = (EnemyShip)myShip.GetComponent(typeof(EnemyShip));
		shipLogic.moveSpeed = Battleship.moveSpeedBase+(Battleship.moveSpeedMultiplier*battleshipsLevel);
		shipLogic.health = Battleship.healthBase+(int)(Battleship.healthMultiplier*battleshipsLevel);
		
		Battleship battleshipLogic = (Battleship)myShip.GetComponent(typeof(Battleship));
		battleshipLogic.reloadTimeBeam = Battleship.reloadTimeBeamBase-(Battleship.reloadTimeBeamMultiplier*battleshipsLevel);
		battleshipLogic.reloadTimeBomb= Battleship.reloadTimeBombBase-(Battleship.reloadTimeBombMultiplier*battleshipsLevel);
		battleshipLogic.beamDamage = Battleship.beamDamageBase+(int)(Battleship.beamDamageMultiplier*battleshipsLevel);
		battleshipLogic.bombDamage = Battleship.bombDamageBase+(int)(Battleship.bombDamageMultiplier*battleshipsLevel);
		battleshipLogic.projectileSpeed = Battleship.bombSpeedBase+(Battleship.bombSpeedMultiplier*battleshipsLevel);
		battleshipLogic.resourceReward = Constants.BS_RESOURCE_REWARD;
		Targets.enemyTargets.Add(myShip);
		print(Targets.enemyTargets.Count);
	}
}
