/*PlanetDecimator.cs - Brokion project
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
 * Very slow, metal armor, drops 4 bombs,mid-high, 
 * Big ship that blows the crap out of everything... moves into a position and stops, drops bombs there and only there
 * 
 */ 
public class PlanetDecimator : EnemyShip {
	
	OTSprite myProjectile;
	OTSprite sprite;
	
	//Plant Decimator creation attributes
	//projectile speed
	public static float projectileSpeedBase = 2.0f;
	public static float projectileSpeedMultiplier = 0.25f;
	//projectile damage
	public static int projectileDamageBase = 30;
	public static float projectileDamageMultiplier = 8.0f;
	//reload time
	public static float reloadTimeBase = 10.0f;
	public static float reloadTimeMultiplier = 0.15f;
	//moveSpeed
	public static float moveSpeedBase = 0.5f;
	public static float moveSpeedMultiplier = .12f;
	//health
	public static int healthBase = 50;
	public static float healthMultiplier = 7.00f;
	
	//PlanetDecimator's attributes
	public float projectileSpeed;
	public int projectileDamage;
	public float reloadTime; //How long of a delay between shots, in seconds
	public AttackType attackType = AttackType.Unassigned;
	public int shotCoefficient; //The probability of taking a shot after cooldown
	
	float timeSinceLastShot; //How long it's been since the last shot was fired, in seconds
	
	public bool stationary;
	float targetX;
	
	GameState gameState;
	// Use this for initialization
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		attackType = AttackType.Bomb;
		//Get the sprite
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		stationary = false;
		targetX = Random.value*90.0f-45.0f;
		
		//stunned
		stunned = false;
		stunnedTime = 0.0f;
		stunDuration = 0.0f;
		
		//If the cruiser's health has not been set, then we have an erroneous creation of this ship:
		if (this.health == 0)
			Debug.LogError("Erroneous creation of a planet decimator");
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
			if (!stunned)
			{
				//update the cooldown timer
				timeSinceLastShot += Time.deltaTime;
				//If the ship is at its position and has a shot available, make an attack.
				if (stationary && timeSinceLastShot > reloadTime && !(sprite.outOfView))
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
		if (stationary == false)
			{
				if(sprite.position.x >= targetX)
				{
					sprite.position = new Vector2(targetX,sprite.position.y);
					stationary = true;
				}
				else
					sprite.position += (Vector2)sprite.xVector	* moveSpeed * Time.deltaTime;
			}
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
			for (int i = -1; i<3;i++)
			{
				//Generate a projectile
				float offsetX = i*1.4f - 0.7f;
				GameObject bullet = OT.CreateObject("EnemyProjectile");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = true;
						
				Vector2	target = new Vector2(sprite.position.x+offsetX,-50.0f);
				myProjectile.position = new Vector2(sprite.position.x+offsetX,sprite.position.y);
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
	}
	
	public void OnCollision(OTObject owner)
	{
		//print ("something hit "+owner.name);	
	}	
}