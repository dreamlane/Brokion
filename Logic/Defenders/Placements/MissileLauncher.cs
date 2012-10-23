using UnityEngine;
using System.Collections;

/*
 * The missile launcher launches missiles that home onto enemy ships.
 * 
 * upgrades:
 * reloadTime
 * projectileSpeed
 * damage
 * 
 * Upgrades into HarvestingMissileLauncher or SmartMissileLauncher
 * 
 */ 
public class MissileLauncher : MonoBehaviour {

	//Turret attributes set externally
	public int health;
	public int maxHealth;
	public float reloadTime;
	public int damage;
	public float projectileSpeed;
	
	//Internally set attributes
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
		//Only attack if there is something to attack
		if (Targets.enemyTargets.Count > 0)
		{
			timeSinceLastShot = 0.0f;
			//fire a player projectile towards an enemy ship
			GameObject missile = OT.CreateObject("PlayerMissile");
			myProjectile = missile.GetComponent<OTSprite>();
			myProjectile.renderer.enabled = true;
							
			// Pick a target, and fire at it
			Vector2 target = Targets.PickRandomEnemyTarget().position;
			target = new Vector2(target.x,target.y);
			myProjectile.position = sprite.position;
			//myProjectile.RotateTowards(target);		//start the missiles off facing straight up
					
			//Assign the projectile its atributes.
			PlayerMissile pm = (PlayerMissile)myProjectile.GetComponent(typeof(PlayerMissile));
			pm.damage = damage;
			pm.speed = projectileSpeed;
			pm.isHarvester = false;
			pm.isSmart = false;
		}
	}
}
