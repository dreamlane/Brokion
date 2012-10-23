/*FlakCannon.cs - Brokion project
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
 * This cannon is an upgrade from the SlugCannon.
 * It fires a Flak Slug, which is like a slug, except that when it impacts an enemy it does a lot of dmg,
 * however if it does not impact an enemy,and arrives at its destination point, then it explodes into a bunch of flak shells.
 * 
 * TODO: change targeting code, update the playercannonprojectile code as well
 * 
 * upgrades:
 * reloadTime
 * projectileSpeed
 * damage
 * accuracy
 * 
 * upgradeTurrets: SuperFlakCannon
 * 
 */ 
public class FlakCannon : MonoBehaviour {

	//Turret attributes set externally
	public int health;
	public int maxHealth;
	public float reloadTime;
	public int damage;
	public float projectileSpeed;
	
	//Internally set attributes
	public int constructionCost;
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
		//We don't want the slugs to be very accurate, so offset the x and y by between (-2,2)
		float offsetX = Random.value*4.0f-2.0f;
		float offsetY = Random.value*4.0f-2.0f;
		target = new Vector2(target.x+offsetX,target.y+offsetY);
		myProjectile.position = sprite.position;
		myProjectile.RotateTowards(target);	
				
		//Assign the projectile its atributes.
		PlayerCannonProjectile pcp = (PlayerCannonProjectile)myProjectile.GetComponent(typeof(PlayerCannonProjectile));
		pcp.damage = damage;
		pcp.speed = projectileSpeed;
		pcp.isFlak = true;
		pcp.target = target;
		pcp.isFlakShell = false;
		pcp.isSuperFlak = false;
		pcp.shellFlightTime = 0.5f;
	}
}
