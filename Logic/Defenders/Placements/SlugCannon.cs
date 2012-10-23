/*SlugCannon.cs - Brokion project
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
 * Fires slugs towards enemies:
 * 
 * upgrades into a FlakCannon or a BurstCannon
 * 
 * standard upgrades:
 * reloadTime
 * projectileSpeed
 * damage
 * 
 */ 
public class SlugCannon : MonoBehaviour {

	//Turret attributes set externally
	public int health;
	public int maxHealth;
	public float reloadTime;
	public int damage;
	public float projectileSpeed;
	
	//Internally set attributes;
	public float timeSinceLastShot;
	public bool dead;
	
	//a handle on the gameState
	GameState gameState;
		
	//a handle on the sprites
	OTSprite sprite;
	OTSprite myProjectile;
	
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		sprite = GetComponent<OTSprite>();
		
		timeSinceLastShot = 0.0f;
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
		timeSinceLastShot = 0.0f;
		//fire a player projectile towards an enemy ship
		GameObject bullet = OT.CreateObject("PlayerCannonProjectile");
		myProjectile = bullet.GetComponent<OTSprite>();
		myProjectile.renderer.enabled = true;
						
		// Pick a target, and fire at it
		Vector2 target = Targets.PickRandomEnemyTarget().position;
		//We don't want the slugs to be very accurate, so offset the x and y by between (-4,4)
		float offsetX = Random.value*8.0f-4.0f;
		float offsetY = Random.value*8.0f-4.0f;
		target = new Vector2(target.x+offsetX,target.y+offsetY);
		myProjectile.position = sprite.position;
		myProjectile.RotateTowards(target);			
				
		//Assign the projectile its atributes.
		PlayerCannonProjectile pcp = (PlayerCannonProjectile)myProjectile.GetComponent(typeof(PlayerCannonProjectile));
		pcp.damage = damage;
		pcp.speed = projectileSpeed;
	}
}
