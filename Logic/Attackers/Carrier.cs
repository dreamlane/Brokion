/*Carrier.cs - Brokion project
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
 * Launches missiles at targets 1 at a time, high dmg, medium speed (faster than bombs), missiles are targetable. armor: energy,
 * medium speed, highest orbit, (maybe)launches fighters
 * 
 */ 
public class Carrier : EnemyShip {
	
	OTSprite myProjectile;
	OTSprite sprite;
	
	//Carrier creation attributes
	//projectile speed
	public static float projectileSpeedBase = 4.0f;
	public static float projectileSpeedMultiplier = 0.55f;
	//projectile damage
	public static int projectileDamageBase = 30;
	public static float projectileDamageMultiplier = 8.0f;
	//reload time
	public static float reloadTimeBase = 4.0f;
	public static float reloadTimeMultiplier = 0.15f;
	//moveSpeed
	public static float moveSpeedBase = 2.5f;
	public static float moveSpeedMultiplier = .22f;
	//health
	public static int healthBase = 80;
	public static float healthMultiplier = 10.00f;
	
	//Carrier's attributes
	public float projectileSpeed;
	public int projectileDamage;
	public float reloadTime; //How long of a delay between shots, in seconds
	public AttackType attackType = AttackType.Unassigned;
	public int shotCoefficient; //The probability of taking a shot after cooldown
	
	float timeSinceLastShot; //How long it's been since the last shot was fired, in seconds
	
	GameState gameState;
	// Use this for initialization
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		//Carrier fires projectiles that have Bomb damage type, but they are targeted instead of dropped
		attackType = AttackType.Bomb;
		armor = ArmorType.Energy;
		//Get the sprite
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		
			
		stunned = false;
		stunnedTime = 0.0f;
		stunDuration = 0.0f;
		//If the cruiser's health has not been set, then we have an erroneous creation of this ship:
		if (this.health == 0)
			Debug.LogError("Erroneous creation of a Carrier");
		
		//Shot coefficients will be constant across all cruisers, this randomizes shot timings a little.
		shotCoefficient = 8; //the lower the int, the higher the randomization, the longer the average time between shots
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//check to see if this frigate has died
		if (health <= 0)
		{
			HandleDeath();	
		}
		else
		{
			//If the ship is not stunned, Act
			if (!stunned)
			{
				//update the cooldown timer
				timeSinceLastShot += Time.deltaTime;
				if (timeSinceLastShot > reloadTime && !(sprite.outOfView))
				{
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
		//Make explosion effects
		
		//Remove battleship from targets list
		Targets.enemyTargets.Remove(sprite);
		//Remove the sprite
		OT.DestroyObject(sprite);
		//Give money to the player
		gameState.currentResources += this.resourceReward;
		//Update the score
		
		//Update the activeShipsCount so that if this was the last ship on the board, then the next wave will begin
		gameState.activeShipCount -= 1;
	}
	
	void Attack()
	{
		//Determine if firing by picking a random number
		if (Random.Range(0,101) <= shotCoefficient)
		{
			//Generate a projectile
			GameObject bullet = OT.CreateObject("EnemyProjectile");
			myProjectile = bullet.GetComponent<OTSprite>();
			myProjectile.renderer.enabled = true;
					
			// Pick a target, and fire at it
			Vector2 target = Targets.PickRandomTargetFromAll().position;
			myProjectile.position = sprite.position;
			myProjectile.RotateTowards(target);			
					
			//Assign the projectile its atributes.
			EnemyProjectile ep = (EnemyProjectile)myProjectile.GetComponent(typeof(EnemyProjectile));
			ep.speed = projectileSpeed;
			ep.damage = projectileDamage;
			ep.attackType = attackType;
					
			//Make shot unavailable
			timeSinceLastShot = 0.0f;
		}
	}
	
	public void OnCollision(OTObject owner)
	{
		//print ("something hit "+owner.name);	
	}	
}