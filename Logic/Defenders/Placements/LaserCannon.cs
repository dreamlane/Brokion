/*LaserCannon.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 14 2012
 * @Updated Aug 15 2012
 */ 
using UnityEngine;
using System.Collections;

/*
 * The LaserCannon is the same as the BeamCannon, but it's beams are laser mode and travel through all targets;
 * 
 * upgrades:
 * reloadTime
 * flightTime
 * damage
 * 
 */ 

public class LaserCannon : MonoBehaviour {

	//Turret attributes set externally
	public int health;
	public int maxHealth;
	public float reloadTime;
	public int damage;
	public float flightTime;
	
	//Internally set attributes
	public int constructionCost;
	public float timeSinceLastShot;
	public bool dead;
	
	//a handle on the gameState
	GameState gameState;
		
	//a handle on the sprites
	OTSprite sprite;
	OTSprite myProjectile;
	// Use this for initialization
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
		//fire a player beam towards an enemy ship
		GameObject beam = OT.CreateObject("PlayerBeam");
		myProjectile = beam.GetComponent<OTSprite>();
		myProjectile.renderer.enabled = false; //Beams appear after a short time in flight
						
		// Pick a target, and fire at it
		// TODO add target picking code for slug
		Vector2 target = Targets.PickRandomEnemyTarget().position;
		//We don't want the beams to be perfectly accurate, so offset the x and y by between (-1,1)
		float offsetX = Random.value*2.0f-1.0f;
		float offsetY = Random.value*2.0f-1.0f;
		target = new Vector2(target.x+offsetX,target.y+offsetY);
		myProjectile.position = sprite.position;
		myProjectile.RotateTowards(target);			
				
		//Assign the projectile its atributes.
		PlayerBeam pb = (PlayerBeam)myProjectile.GetComponent(typeof(PlayerBeam));
		pb.damage = damage;
		pb.maxFlightTime = flightTime;
		pb.flightTime = 0.0f;
		pb.isStun = false;
		pb.isLaser = true;
	}
}
