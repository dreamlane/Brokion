/*Battleship.cs - Brokion project
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
 * 
 * Super slow, highest orbit, energy armor, shoots 2 beams, drops 3 bombs, (maybe) ground units
 * 
 */ 
public class Battleship : EnemyShip {
	
	OTSprite myProjectile;
	OTSprite sprite;
	
	//Battleship creation attributes
	//projectile speed
	public static float bombSpeedBase = 4.0f;
	public static float bombSpeedMultiplier = 0.25f;
	//projectile damage
	public static int bombDamageBase = 30;
	public static float bombDamageMultiplier = 8.0f;
	public static int beamDamageBase = 6;
	public static float beamDamageMultiplier = 1.0f;
	//reload time
	public static float reloadTimeBombBase = 4.0f;
	public static float reloadTimeBombMultiplier = 0.15f;
	public static float reloadTimeBeamBase = 1.0f;
	public static float reloadTimeBeamMultiplier = 0.05f;
	//moveSpeed
	public static float moveSpeedBase = 2.5f;
	public static float moveSpeedMultiplier = .22f;
	//health
	public static int healthBase = 120;
	public static float healthMultiplier = 20.00f;
	
	//Battleship's attributes
	public float projectileSpeed;
	public int beamDamage;
	public int bombDamage;
	public float reloadTimeBeam; //How long of a delay between shots, in seconds
	public float reloadTimeBomb;
	public int shotCoefficient; //The probability of taking a shot after cooldown
	public int level;
	
	float timeSinceLastBombShot; //How long it's been since the last shot was fired, in seconds
	float timeSinceLastBeamShot;
	
	GameState gameState;
	// Use this for initialization
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		//Get the sprite
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		
		//If the cruiser's health has not been set, then we have an erroneous creation of this ship:
		if (this.health == 0)
			Debug.LogError("Erroneous creation of a Battleship");
		
		//Shot coefficients will be constant across all cruisers, this randomizes shot timings a little.
		shotCoefficient = 9; //the lower the int, the higher the randomization, the longer the average time between shots
		
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
			
			
			//Act if the ship is not stunned
			if (!stunned)
			{
				//update and check the reload timers
				timeSinceLastBeamShot += Time.deltaTime;
				if (timeSinceLastBeamShot > reloadTimeBeam && !(sprite.outOfView))
				{
					BeamAttack();
				}
				timeSinceLastBombShot += Time.deltaTime;
				if (timeSinceLastBombShot > reloadTimeBomb && !(sprite.outOfView))
				{
					BombAttack();
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
		
		//Remove Battleship from targets list
		Targets.enemyTargets.Remove(sprite);
		//Remove the sprite
		OT.DestroyObject(sprite);
		//Give money to the player
		gameState.currentResources += this.resourceReward;
		//Update the score
		
		//Update the activeShipsCount so that if this was the last ship on the board, then the next wave will begin
		gameState.activeShipCount -= 1;
	}
	
	void BombAttack()
	{
		//Determine if firing by picking a random number
		if (Random.Range(0,101) <= shotCoefficient)
		{
			for (int i = 0; i<3;i++)
			{
				//Generate a projectile
				float offsetX = i*1.4f - 0.7f;
				GameObject bullet = OT.CreateObject("EnemyProjectile");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = true;
						
				Vector2	target = new Vector2(sprite.position.x+offsetX,-50.0f);
				myProjectile.position = new Vector2(sprite.position.x+offsetX,sprite.position.y-1.0f);
				myProjectile.RotateTowards(target);			
						
				//Assign the projectile its atributes.
				EnemyProjectile ep = (EnemyProjectile)myProjectile.GetComponent(typeof(EnemyProjectile));
				ep.speed = projectileSpeed;
				ep.damage = bombDamage;
				ep.attackType = AttackType.Bomb;
						
				//Make shot unavailable
				timeSinceLastBombShot = 0.0f;
			}
		}
	}
	
	void BeamAttack()
	{
		//Determine if firing by picking a random number
		if (Random.Range(0,101) <= shotCoefficient)
		{
			for (int i = 0; i<3;i++)
			{
				//Generate a projectile
				GameObject bullet = OT.CreateObject("EnemyBeam");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = false;
						
				Vector2	target = Targets.PickRandomTargetFromAll().position;
				myProjectile.position = sprite.position;
				myProjectile.RotateTowards(target);			
						
				//Assign the projectile its atributes.
				EnemyBeam ep = (EnemyBeam)myProjectile.GetComponent(typeof(EnemyBeam));
				ep.damage = beamDamage;
				ep.attackType = AttackType.Beam;
						
				//Make shot unavailable
				timeSinceLastBeamShot = 0.0f;
			}
		}
	}
	
	public void OnCollision(OTObject owner)
	{
		//print ("something hit "+owner.name);	
	}	
}