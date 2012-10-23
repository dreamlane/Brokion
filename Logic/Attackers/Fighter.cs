/*Fighter.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 8 2012
 * @Updated Aug 14 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

public class Fighter : EnemyShip {
	
	OTSprite myProjectile;
	OTSprite sprite;
	
	//Fighter creation attributes
	//projectile speed
	public static float projectileSpeedBase = 4.0f;
	public static float projectileSpeedMultiplier = 0.85f;
	//projectile damage
	public static int projectileDamageBase = 2;
	public static float projectileDamageMultiplier = 1.5f;
	//reload time
	public static float reloadTimeBase = 5.0f;
	public static float reloadTimeMultiplier = 0.15f;
	//moveSpeed
	public static float moveSpeedBase = 2.0f;
	public static float moveSpeedMultiplier = .25f;
	//health
	public static int healthBase = 8;
	public static float healthMultiplier = 1.5f;
	
	
	//Fighter instance attributes
	public float projectileSpeed;
	public int projectileDamage;
	public float reloadTime; //How long of a delay between shots, in seconds
	public AttackType attackType = AttackType.Unassigned;
	public int shotCoefficient; //The probability of taking a shot after cooldown
	
	//Bomber specific attributes
	private float targetX;
	public bool stationary;
	
	public bool shotAvailable;
	float timeSinceLastShot; //How long it's been since the last shot was fired, in seconds
	
	GameState gameState; 
	// Use this for initialization
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		//Assign the attack type
		if (attackType == AttackType.Unassigned) // The attack type has not been set externally, choose a random one
		{
			attackType = (AttackType)Random.Range((int)AttackType.Bomb, (int)AttackType.Slug +1);
		}
		//Get the sprite
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		shotAvailable = true;
		
		//stunned states
		stunned = false;
		stunnedTime = 0.0f;
		stunDuration = 0.0f;
		//If the fighter's health has not been set, then set it to it's default:
		if (this.health == 0)
			Debug.LogError("Erroneous creation of a fighter");
		//Set up the attack attributes for this fighter
		if (attackType == AttackType.Beam) //Mk2
		{
			//Beam attacks reload much faster and deal much less damage
			projectileDamage-=projectileDamage/2;
			reloadTime/=3;
			//They also travel much faster
			projectileSpeed*=3;
			//The beam fighter moves at a faster rate and has armor type shield
			armor = ArmorType.Energy;
		}
		else if (attackType == AttackType.Bomb) //Mk1
		{
			//Bombs move slower and deal more damage
			projectileSpeed/=2;
			reloadTime*=1.25f;
			projectileDamage*=2;
			
			//Bombers move to a target x position between -45.0 and 45.0 and then stop
			targetX = Random.value*90.0f-45.0f;
			stationary = false;
		}
		else //Mk3 - slug
		{
			moveSpeed-=1.0f;
			//Make Mk3 travel right to left
		}
		
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
		//If the ship is a bomber fighter, then we need to check to see if it has reached or passed it's x position target
		if (attackType == AttackType.Bomb)
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
		else if (attackType == AttackType.Beam) //Beam fighters move left to right
		{
			//if the ship has reached the far right of the screen, lets send it around the world
			if (sprite.position.x > 50.0f)
				sprite.position = new Vector2(-51.0f,sprite.position.y);
			//Move this ship forward its movespeed*deltaTime
			else
				sprite.position += (Vector2)sprite.xVector	* moveSpeed * Time.deltaTime;
		}
		else //Slug fighters move right to left
		{
			//if the ship has reached the far left of the screen, lets send it around the world
			if (sprite.position.x < -50.0f)
				sprite.position = new Vector2(51.0f,sprite.position.y);
			//Move this ship forward its movespeed*deltaTime
			else
				sprite.position -= (Vector2)sprite.xVector	* moveSpeed * Time.deltaTime;
		}
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
	
	void Attack()
	{
		//Determine if firing by picking a random number
		if (Random.Range(0,101) <= shotCoefficient)
		{
			if (attackType == AttackType.Slug)
			{
				//Generate a projectile
				GameObject bullet = OT.CreateObject("EnemyProjectile");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = true;
						
				// Pick a target, and fire at it
				// TODO add target picking code for slug
				Vector2 target = GameObject.Find("City").GetComponent<OTSprite>().position;
				myProjectile.position = sprite.position;
				myProjectile.RotateTowards(target);			
						
				//Assign the projectile its atributes.
				EnemyProjectile ep = (EnemyProjectile)myProjectile.GetComponent(typeof(EnemyProjectile));
				ep.speed = projectileSpeed;
				ep.damage = projectileDamage;
				ep.attackType = attackType;
						
				//Make shot unavailable
				shotAvailable = false;
				timeSinceLastShot = 0.0f;
			}
			else if (attackType == AttackType.Beam)
			{
				//Generate a projectile
				GameObject beam = OT.CreateObject("EnemyBeam");
				myProjectile = beam.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = false; //Beams don't get rendered until they are in flight for a little bit
					
				// TODO add target picking code for beam
				Vector2 target = Targets.PickRandomTargetFromAll().position;
				myProjectile.position = sprite.position;
				myProjectile.RotateTowards(target);			
						
				//Assign the projectile its atributes.
				EnemyBeam eb = (EnemyBeam)myProjectile.GetComponent(typeof(EnemyBeam));
				eb.damage = projectileDamage;
				eb.attackType = attackType;
						
				//Make shot unavailable
				shotAvailable = false;
				timeSinceLastShot = 0.0f;
			}
			else //bomber
			{
				//Generate a projectile... TODO: Create EnemyBomb
				GameObject bullet = OT.CreateObject("EnemyProjectile");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = true;
				
				//Bombs drop straight down
				Vector2	target = new Vector2(sprite.position.x,-50.0f);
				myProjectile.position = sprite.position;
				myProjectile.RotateTowards(target);			
						
				//Assign the projectile its atributes.
				EnemyProjectile ep = (EnemyProjectile)myProjectile.GetComponent(typeof(EnemyProjectile));
				ep.speed = projectileSpeed;
				ep.damage = projectileDamage;
				ep.attackType = attackType;
						
				//Make shot unavailable
				shotAvailable = false;
				timeSinceLastShot = 0.0f;
			}
		}
	}
	
	public void OnCollision(OTObject owner)
	{
		//print ("something hit "+owner.name);	
	}	
}
