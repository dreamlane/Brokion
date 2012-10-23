/*BurstCannon.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 14 2012
 * @Updated Aug 14 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

/*
 * The BurstCannon is exactly the same as the SlugCannon except that is fires multiple shots at once
 * 
 * upgrades:
 * reloadTime
 * projectileSpeed
 * damage
 * burstCount (starts at 3, up to 10)
 * 
 */
public class BurstCannon : MonoBehaviour {

	//Turret attributes set externally
	public int health;
	public int maxHealth;
	public float reloadTime;
	public int damage;
	public float projectileSpeed;
	
	//Internally set attributes
	public int constructionCost;
	private float timeSinceLastShot;
	public bool dead;
	public int burstCount = 6;
	private int shotCount;
	private float shotInterval;
	private float recoilTime = 0.15f;
	private Vector2 target;
	
	//a handle on the gameState
	GameState gameState;
		
	//a handle on the sprites
	OTSprite sprite;
	OTSprite myProjectile;
	
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		sprite = GetComponent<OTSprite>();
		
		timeSinceLastShot = 0.0f;
		shotCount = 0;
		shotInterval = 0.0f;
		dead = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Check to see if the wave has changed
		if (gameState.betweenRounds)
		{
			//reset hp
			health = maxHealth;
			dead = false;
		}
		else 
		{
			//Check to see if we're alive
			if (health <= 0 || dead)
			{
				dead = true;
			}
			else //We're alive
			{
				timeSinceLastShot += Time.deltaTime;
				if (timeSinceLastShot >= reloadTime)//Fire!
					Attack();
			}
		}
	}
	
	void Attack()
	{
		shotInterval+= Time.deltaTime;
		if (shotCount == 0) //Get a new target each burst
		{
			// Pick a target, and fire at it
			target = Targets.PickRandomEnemyTarget().position;
			//We don't want the slugs to be very accurate, so offset the x and y by between (-4,4)
			float offsetX = Random.value*8.0f-4.0f;
			float offsetY = Random.value*8.0f-4.0f;
			target = new Vector2(target.x+offsetX,target.y+offsetY);
		}
		else //Give the burst some accuracy variance
		{
			float offsetX = Random.value*4.0f-2.0f;
			float offsetY = Random.value*4.0f-2.0f;
			target = new Vector2(target.x+offsetX,target.y+offsetY);
		}
		if (shotInterval > recoilTime)
		{
			//fire a player projectile towards an enemy ship
			GameObject bullet = OT.CreateObject("PlayerCannonProjectile");
			myProjectile = bullet.GetComponent<OTSprite>();
			myProjectile.renderer.enabled = true;
			myProjectile.position = sprite.position;
			myProjectile.RotateTowards(target);			
					
			//Assign the projectile its atributes.
			PlayerCannonProjectile pcp = (PlayerCannonProjectile)myProjectile.GetComponent(typeof(PlayerCannonProjectile));
			pcp.damage = damage;
			pcp.speed = projectileSpeed;
			
			//increment the shotCount
			shotCount++;
			shotInterval = 0.0f;
		}
		if (shotCount >= burstCount)
		{
			timeSinceLastShot = 0.0f;
			shotCount = 0;
		}
	}
}
