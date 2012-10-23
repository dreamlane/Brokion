/*Frigate.cs - Brokion project
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
 * Slow moving, metal armor, shoots five slugs at once quickly, flies mid-high from right to left
 */ 
public class Frigate : EnemyShip {
	
	OTSprite myProjectile;
	OTSprite sprite;
	
	//Frigate creation attributes
	//projectile speed
	public static float projectileSpeedBase = 5.0f;
	public static float projectileSpeedMultiplier = 0.95f;
	//projectile damage
	public static int projectileDamageBase = 3;
	public static float projectileDamageMultiplier = 1.2f;
	//reload time
	public static float reloadTimeBase = 8.0f;
	public static float reloadTimeMultiplier = 0.15f;
	//moveSpeed
	public static float moveSpeedBase = 0.5f;
	public static float moveSpeedMultiplier = .18f;
	//health
	public static int healthBase = 25;
	public static float healthMultiplier = 3.00f;
	
	//Frigate's attributes
	public float projectileSpeed;
	public int projectileDamage;
	public float reloadTime; //How long of a delay between shots, in seconds
	public AttackType attackType = AttackType.Unassigned;
	public int shotCoefficient; //The probability of taking a shot after cooldown
	
	public bool shotAvailable;
	float timeSinceLastShot; //How long it's been since the last shot was fired, in seconds
	int shotCount = 1;
	// Use this for initialization
	GameState gameState;
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		//Frigates have metal armor, and shoot slug projectiles
		armor = ArmorType.Metal;
		attackType = AttackType.Slug;
		
		//Get the sprite
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		shotAvailable = true;
		
		//stunned states
		stunned = false;
		stunnedTime = 0.0f;
		stunDuration = 0.0f;
		//If the frigate's health has not been set, then we have an erroneous creation of this ship:
		if (this.health == 0)
			Debug.LogError("Erroneous creation of a frigate");
		
		//Shot coefficients will be constant across all frigates, this randomizes shot timings a little.
		shotCoefficient = 5; //the lower the int, the higher the randomization, the longer the average time between shots
		
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
		//if the ship has reached the far left of the screen, lets send it around the world
			if (sprite.position.x < -50.0f)
				sprite.position = new Vector2(51.0f,sprite.position.y);
			//Move this ship forward its movespeed*deltaTime
			else
				sprite.position -= (Vector2)sprite.xVector	* moveSpeed * Time.deltaTime;
	}
	
	void HandleDeath()
	{
		//Remove the ship from the targets list
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
	
	void Attack()
	{
		//Determine if firing by picking a random number
		if (Random.Range(0,101) <= shotCoefficient)
		{
			//This ship shoots 5 slugs almost at once, so set the shot coefficient to 100 to garauntee that
			shotCoefficient = 100;
			//Generate a projectile
			GameObject bullet = OT.CreateObject("EnemyProjectile");
			myProjectile = bullet.GetComponent<OTSprite>();
			myProjectile.renderer.enabled = true;
					
			// Pick a random target, and fire at it
			// TODO add target picking code
			float targetX = Random.value*90.0f-45.0f;
			Vector2 target = new Vector2(targetX,-40.0f);
			myProjectile.position = sprite.position;
			myProjectile.RotateTowards(target);			
					
			//Assign the projectile its atributes.
			EnemyProjectile ep = (EnemyProjectile)myProjectile.GetComponent(typeof(EnemyProjectile));
			ep.speed = projectileSpeed;
			ep.damage = projectileDamage;
			ep.attackType = attackType;
			
			shotCount++;
			if (shotCount > 5)
			{
				shotCount = 1;
				shotAvailable = false;
				timeSinceLastShot = 0.0f;
				shotCoefficient = 5;
			}
		}
	}
	
	public void OnCollision(OTObject owner)
	{
		//print ("something hit "+owner.name);	
	}	
}