/*Cruiser.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 13 2012
 * @Updated Aug 13 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

/*
 * The cruiser moves slowly and shoots a triple beam, flies in low-mid. Armor type = energy.
 * 
 */ 
public class Cruiser : EnemyShip {
	
	OTSprite myProjectile;
	OTSprite sprite;
	
	//Cruiser creation attributes
	//projectile damage
	public static int projectileDamageBase = 1;
	public static float projectileDamageMultiplier = 1.2f;
	//reload time
	public static float reloadTimeBase = 2.0f;
	public static float reloadTimeMultiplier = 0.15f;
	//moveSpeed
	public static float moveSpeedBase = 1.0f;
	public static float moveSpeedMultiplier = .18f;
	//health
	public static int healthBase = 15;
	public static float healthMultiplier = 2.25f;
	
	//Cruiser's attributes
	public int projectileDamage;
	public float reloadTime; //How long of a delay between shots, in seconds
	public AttackType attackType = AttackType.Unassigned;
	public int shotCoefficient; //The probability of taking a shot after cooldown
	
	public bool shotAvailable;
	float timeSinceLastShot; //How long it's been since the last shot was fired, in seconds
	int sequenceCount;
	Vector2 shot1;
	Vector2 shot2;
	
	GameState gameState;
	// Use this for initialization
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		//Assign the attack type
		attackType = AttackType.Beam;
		//Get the sprite
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		shotAvailable = true;
		stunned = false;
		stunnedTime = 0.0f;
		stunDuration = 0.0f;
		//If the cruiser's health has not been set, then we have an erroneous creation of this ship:
		if (this.health == 0)
			Debug.LogError("Erroneous creation of a cruiser");

		//Shot coefficients will be constant across all cruisers, this randomizes shot timings a little.
		shotCoefficient = 5; //the lower the int, the higher the randomization, the longer the average time between shots
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//check to see if this fighter has died
		if (health <= 0)
		{
			HandleDeath();	
		}
		else
		{
			//make sure it isnt' stunned
			if (!stunned)
			{
				//update the cooldown timer
				timeSinceLastShot += Time.deltaTime;
				if (timeSinceLastShot > reloadTime && !(sprite.outOfView))
				{
					shotAvailable = true;
					Attack();
				}	
				MoveMe();
			}
			else //The ship is stunned
			{
				//update the stunned timer
				stunnedTime += Time.deltaTime;
				if (stunnedTime >= stunDuration)
				{
					stunned = false;
					stunnedTime = 0.0f;
					stunDuration = 0.0f;
				}
			}
		}
	}
	
	void MoveMe()
	{
		//if the ship has reached the far right of the screen, lets send it around the world
		if (sprite.position.x > 50.0f)
			sprite.position = new Vector2(-51.0f,sprite.position.y);
		//Move this ship forward its movespeed*deltaTime
		else
			sprite.position += (Vector2)sprite.xVector	* moveSpeed * Time.deltaTime;
	}
	
	void HandleDeath()
	{
		//Remove the sprite from the targets list
		Targets.enemyTargets.Remove(sprite);
		
		//Make explosion effects
		
		//Remove the sprite
		OT.DestroyObject(sprite);
		//Give money to the player
		gameState.currentResources += this.resourceReward;
		//Update the score
		
		//Update the activeShipsCount so that if this was the last ship on the board, then the next wave will begin
		gameState.activeShipCount -= 1;
	}
	
	/*
	 * The Cruiser fires 3 beams, 1 directly below, and one 10-15 degrees to each side
	 */ 
	void Attack()
	{
		//Determine if firing by picking a random number
		if (Random.Range(0,101) <= shotCoefficient)
		{
			shotCoefficient = 100;
			//Generate a projectile
			GameObject beam = OT.CreateObject("EnemyBeam");
			myProjectile = beam.GetComponent<OTSprite>();
			myProjectile.renderer.enabled = false;//only render after a couple frames of animation
					
			// Pick a target random target
			// We should keep track of targets so that the cruiser targets 3 different targets each burst...
			Vector2 target = Targets.PickRandomTargetFromAll().position;
			if (sequenceCount == 1)
			{
				shot1 = target;
			}
			else if (sequenceCount == 2)
			{
				while (target.Equals(shot1))
					target = Targets.PickRandomTargetFromAll().position;
				shot2 = target;
			}
			else
			{
				while(target.Equals(shot1) || target.Equals(shot2))
					target = Targets.PickRandomTargetFromAll().position;
			}	
				//check to see if the target has already been picked
			myProjectile.position = sprite.position;
			myProjectile.RotateTowards(target);
					
			//Assign the projectile its atributes.
			EnemyBeam eb = (EnemyBeam)myProjectile.GetComponent(typeof(EnemyBeam));
			eb.damage = projectileDamage;
			eb.attackType = attackType;
			
			sequenceCount += 1;
			//Make shot unavailable if the 3 shot in the sequence was fired
			if (sequenceCount > 3)
			{
				sequenceCount = 1;
				shotAvailable = false;
				timeSinceLastShot = 0.0f;
				shotCoefficient = 5;
				//reset the shot1 and shot2 variables
				shot1 = Vector2.zero;
				shot2 = Vector2.zero;
			}
		}
	}
	
	public void OnCollision(OTObject owner)
	{
		//print ("something hit "+owner.name);	
	}	
}
