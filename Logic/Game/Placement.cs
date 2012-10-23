/*Placement.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 4 2012
 * @Updated Oct 6 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

/*
 * This class handles the construction of objects on placements
 * 
 * WARNING: This class is going to potentially grow into a "God Class"... 
 * I've acknowledged this fact, and determined to leave it be, instead of using polymorphism, due to strict deadlines.
 * The code will be split into as many small chunks as possible within this file.
 * If I were to do it all over again, Placement would be an Interface, and all types of turrets would implement Placement.
 */
public class Placement : MonoBehaviour {
	
	OTSprite myTurret;
	OTSprite sprite;
	
	GameState gameState;
	bool isUIActive;
	
	//Type of placement determination variables, only one of these can be true at any given time
	bool isEmptyPlacement;
	bool isBeamCannon;
	bool isBurstCannon;
	bool isFlakCannon;
	bool isHarvestingMissileLauncher;
	bool isLaserCannon;
	bool isMineralDrill;
	bool isMissileLauncher;
	bool isSlugCannon;
	bool isSmartMissileLauncher;
	bool isStunCannon;
	bool isSuperFlakCannon;
	bool isWorldRegenerator;
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<OTSprite>();
		sprite.onInput = OnInput;
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		
		//Initialize the type of turret state to be empty (buildable)
		isEmptyPlacement = true;
		isBeamCannon = false;
		isBurstCannon = false;
		isFlakCannon = false;
		isHarvestingMissileLauncher = false;
		isLaserCannon = false;
		isMineralDrill = false;
		isMissileLauncher = false;	
		isSlugCannon = false;
		isSmartMissileLauncher = false;
		isStunCannon = false;
		isSuperFlakCannon = false;
		isWorldRegenerator = false;
		
		//Make sure the UI is inactive at first
		isUIActive = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnInput(OTObject owner)
	{	
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			//make sure that another placement's gui is not already active
			if (!gameState.isPlacementGUIActive) {
				//Active the gui
				isUIActive = true;
				gameState.isPlacementGUIActive = true; //make sure other objects know about the active GUI.
				//pause the game until the menu is finished
				Time.timeScale = 0.0F;
			}
		}
	}
	
	/*
	 * The gui handling function
	 * 
	 * Each state of turret should be handled differently, therefore each state has its own function
	 **/
	void OnGUI()
	{
		//print("OnGui");
		if (isUIActive)
		{
			if (isEmptyPlacement)
				HandleBuildMenu();
			else
				HandleUpgradeMenu();
		}	
	}	
	
	
	/******* GUI Creation Methods ********/
	/*
	 * This is the menu that pops up when the user wants to place a turret on an empty placement location
	 */
	void HandleBuildMenu()
	{
		// Make a background box
		GUI.Box(new Rect(10,10,800,400), "Build Menu");
		// Set up all of the new construction buttons
		if(GUI.Button(new Rect(20,50,240,100), "Beam Cannon")) {
			ConstructBeamCannon();
		}
		if(GUI.Button(new Rect(20,175,240,100), "Missile Launcher")) {
			ConstructMissileLauncher();
		}
		if(GUI.Button(new Rect(20,300,240,100), "Slug Cannon")) {
			ConstructSlugCannon();
		}
		if(GUI.Button(new Rect(300,50,240,100), "Mineral Drill")) {
			ConstructMineralDrill();
		}
		if(GUI.Button(new Rect(300,175,240,100), "World Regenerator")) {
			ConstructWorldRegenerator();
		}
		if(GUI.Button(new Rect(300,300,240,100), "Cancel Build")) {
			Time.timeScale = 1.0f;
			isUIActive = false;
			gameState.isPlacementGUIActive = false;
		}
	}
	/*
	 * If something is already constructed in this place, then we need to provide an upgrade menu
	 * 
	 */
	void HandleUpgradeMenu()
	{
		string button1;
		string button2;
		string button3;
		string button4;
		string button5;
		
		//Set the label for button 1 correctly
		if (isBeamCannon || isBurstCannon || isFlakCannon || isHarvestingMissileLauncher 
			|| isLaserCannon || isMissileLauncher || isSlugCannon 
			|| isSmartMissileLauncher || isStunCannon || isSuperFlakCannon)
			button1 = "Upgrade Damage";
		else if (isMineralDrill)
			button1 = "Upgrade Harvest Rate";
		else
			button1 = "Upgrade Regen Rate";
		
		//Set the label for button 2 correctly
		if (isBeamCannon || isBurstCannon || isFlakCannon || isHarvestingMissileLauncher 
			|| isLaserCannon || isMissileLauncher || isSlugCannon 
			|| isSmartMissileLauncher || isStunCannon || isSuperFlakCannon)
			button2 = "Upgrade Rate of Fire";
		else
			button2 = "Upgrade Max Health";
		
		// Make a background box
		GUI.Box(new Rect(10,10,800,400), "Build Menu");
		// Set up all of the new construction buttons
		if(GUI.Button(new Rect(20,50,240,100), button1)) {
			HandleUpgradeButton1();
		}
		if(GUI.Button(new Rect(20,175,240,100), button2)) {
			HandleUpgradeButton2();
		}
		if(GUI.Button(new Rect(20,300,240,100), button3)) {
			HandleUpgradeButton3();
		}
		if(GUI.Button(new Rect(300,50,240,100), button4)) {
			HandleUpgradeButton4();
		}
		if(GUI.Button(new Rect(300,175,240,100), button5)) {
			HandleUpgradeButton5();
		}
		if(GUI.Button(new Rect(300,300,240,100), "Cancel")) {
			Time.timeScale = 1.0f;
			isUIActive = false;
			gameState.isPlacementGUIActive = false;
		}
	}

	/******* Turret Construction Methods ********/
	void ConstructBeamCannon()
	{
		//Check to make sure the player has sufficient resources
		if(gameState.currentResources >= Constants.BC_CONSTRUCTION_COST)
		{
			//Get a handle on the beamCannon's sprite, and make sure it shows up in the view
			GameObject beamCannon = OT.CreateObject("BeamCannon");
			myTurret = beamCannon.GetComponent<OTSprite>();
			myTurret.position = sprite.position;
			myTurret.renderer.enabled = true;
			
			//Assign the beamCannon its default atributes.
			BeamCannon turret = (BeamCannon)myTurret.GetComponent(typeof(BeamCannon));
			turret.damage = Constants.DEFAULT_BC_DAMAGE;
			turret.reloadTime = Constants.DEFAULT_BC_RELOAD_TIME;
			turret.maxHealth = Constants.DEFAULT_BC_MAX_HEALTH;
			turret.health = Constants.DEFAULT_BC_HEALTH;
			turret.flightTime = Constants.DEFAULT_BC_FLIGHT_TIME;
			
			//Spend the resources
			gameState.currentResources -= Constants.BC_CONSTRUCTION_COST;
			gameState.spentResources += Constants.BC_CONSTRUCTION_COST;
			
			//Set this placement to be a beamCannon, instead of empty
			isBeamCannon = true;
			isEmptyPlacement = false;
			
			//Drop the UI and unpause the game
			Time.timeScale = 1.0f;
			isUIActive = false;
			gameState.isPlacementGUIActive = false;
		}
		else //Not enough resources
		{
			//do nothing....
			//DESIGN CHOICE: Insufficient resources will make options greyed out.	
		}
	}
	void ConstructMissileLauncher()
	{
		//Check to make sure the player has sufficient resources
		if(gameState.currentResources >= Constants.ML_CONSTRUCTION_COST)
		{
			//Get a handle on the beamCannon's sprite, and make sure it shows up in the view
			GameObject missileLauncher = OT.CreateObject("MissileLauncher");
			myTurret = missileLauncher.GetComponent<OTSprite>();
			myTurret.position = sprite.position;
			myTurret.renderer.enabled = true;
			
			//Assign the beamCannon its default atributes.
			MissileLauncher turret = (MissileLauncher)myTurret.GetComponent(typeof(MissileLauncher));
			turret.damage = Constants.DEFAULT_ML_DAMAGE;
			turret.reloadTime = Constants.DEFAULT_ML_RELOAD_TIME;
			turret.maxHealth = Constants.DEFAULT_ML_MAX_HEALTH;
			turret.health = Constants.DEFAULT_ML_HEALTH;
			turret.projectileSpeed = Constants.DEFAULT_ML_PROJECTILE_SPEED;
			
			//Spend the resources
			gameState.currentResources -= Constants.ML_CONSTRUCTION_COST;
			gameState.spentResources += Constants.ML_CONSTRUCTION_COST;
			
			//Set this placement to be a beamCannon, instead of empty
			isMissileLauncher = true;
			isEmptyPlacement = false;
			
			//Drop the UI and unpause the game
			Time.timeScale = 1.0f;
			isUIActive = false;
			gameState.isPlacementGUIActive = false;
		}
		else //Not enough resources
		{
			//do nothing....
			//DESIGN CHOICE: Insufficient resources will make options greyed out.	
		}
	}
	void ConstructSlugCannon()
	{
		//Check to make sure the player has sufficient resources
		if(gameState.currentResources >= Constants.SLUG_CONSTRUCTION_COST)
		{
			//Get a handle on the beamCannon's sprite, and make sure it shows up in the view
			GameObject slugCannon = OT.CreateObject("SlugCannon");
			myTurret = slugCannon.GetComponent<OTSprite>();
			myTurret.position = sprite.position;
			myTurret.renderer.enabled = true;
			
			//Assign the beamCannon its default atributes.
			SlugCannon turret = (SlugCannon)myTurret.GetComponent(typeof(SlugCannon));
			turret.damage = Constants.DEFAULT_SLUG_DAMAGE;
			turret.reloadTime = Constants.DEFAULT_SLUG_RELOAD_TIME;
			turret.maxHealth = Constants.DEFAULT_SLUG_MAX_HEALTH;
			turret.health = Constants.DEFAULT_SLUG_HEALTH;
			turret.projectileSpeed = Constants.DEFAULT_SLUG_PROJECTILE_SPEED;
			
			//Spend the resources
			gameState.currentResources -= Constants.SLUG_CONSTRUCTION_COST;
			gameState.spentResources += Constants.SLUG_CONSTRUCTION_COST;
			
			//Set this placement to be a beamCannon, instead of empty
			isSlugCannon = true;
			isEmptyPlacement = false;
			
			//Drop the UI and unpause the game
			Time.timeScale = 1.0f;
			isUIActive = false;
			gameState.isPlacementGUIActive = false;
		}
		else //Not enough resources
		{
			//do nothing....
			//DESIGN CHOICE: Insufficient resources will make options greyed out.	
		}
	}
	void ConstructMineralDrill()
	{
		//Check to make sure the player has sufficient resources
		if(gameState.currentResources >= Constants.MD_CONSTRUCTION_COST)
		{
			//Get a handle on the beamCannon's sprite, and make sure it shows up in the view
			GameObject mineralDrill = OT.CreateObject("MineralDrill");
			myTurret = mineralDrill.GetComponent<OTSprite>();
			myTurret.position = sprite.position;
			myTurret.renderer.enabled = true;
			
			//Assign the beamCannon its default atributes.
			MineralDrill drill = (MineralDrill)myTurret.GetComponent(typeof(MineralDrill));
			drill.maxHealth = Constants.DEFAULT_SLUG_MAX_HEALTH;
			drill.health = Constants.DEFAULT_SLUG_HEALTH;
			drill.resourceRate = Constants.DEFAULT_MD_RESOURCE_RATE;
			
			//Spend the resources
			gameState.currentResources -= Constants.MD_CONSTRUCTION_COST;
			gameState.spentResources += Constants.MD_CONSTRUCTION_COST;
			
			//Set this placement to be a beamCannon, instead of empty
			isMineralDrill = true;
			isEmptyPlacement = false;
			
			//Drop the UI and unpause the game
			Time.timeScale = 1.0f;
			isUIActive = false;
			gameState.isPlacementGUIActive = false;
		}
		else //Not enough resources
		{
			//do nothing....
			//DESIGN CHOICE: Insufficient resources will make options greyed out.	
		}
	}
	void ConstructWorldRegenerator()
	{
		//TODO	
	}
	
	/******* Upgrade Turret Handlers ********/
	void HandleUpgradeButton1()
	{
		//TODO	
	}
	void HandleUpgradeButton2()
	{
		//TODO	
	}
	void HandleUpgradeButton3()
	{
		//TODO	
	}
	void HandleUpgradeButton4()
	{
		//TODO	
	}
	void HandleUpgradeButton5()
	{
		//TODO	
	}
	void HandleUpgradeButton6()
	{
		//TODO	
	}
}
