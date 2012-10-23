/*PlayerBeam.cs - Brokion project
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
 * This class handles the attributes and behaviour of beams shot from player owned cannons
 * 
 * 
 */ 
public class PlayerBeam : MonoBehaviour {

	
	
	//externally set values
	public float maxFlightTime;
	public int damage;
	public bool isStun = false;
	public bool isLaser = false;
	public float stunDuration;
	
	public float flightTime;
	public float speed = 90.0f; //Speed in units per second
	
	OTSprite sprite;
	
	// Use this for initialization
	void Start () {
		damage = 10;
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		flightTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//update the flightTime, and determine if the beam has reached the end of its life
		flightTime += Time.deltaTime;
		if (flightTime >= 0.04f)
			sprite.renderer.enabled = true;
		if (flightTime >= maxFlightTime)
			HandleFizzle();
		else
			sprite.position += (Vector2)sprite.yVector	* speed * Time.deltaTime;
	}
	
	void HandleFizzle()
	{
		OT.DestroyObject(sprite);
	}
	
	public void OnCollision(OTObject owner)
	{
		//Check to see if the collision is with an enemy ship
		if (owner.collisionObject.name.ToLower().Contains("enemyship"))
		{
			HandleHit(owner);	
		}
	}
	
	private void HandleHit(OTObject owner)
	{
		//Get a hold of this enemy ship's data and adjust it
		EnemyShip enemy = owner.collisionObject.GetComponent<EnemyShip>();
		enemy.health -= damage;
		
		//If this beam is a stun beam then stun the enemyship
		if (isStun)
		{
			enemy.stunned = true;
			enemy.stunnedTime = 0.0f;
			enemy.stunDuration = stunDuration;
		}
		
		//Remove this projectile if it is not a laser
		if (!(isLaser))
			HandleFizzle();
	}
}
