/*EnemyBeam.cs - Brokion project
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
 * 
 */ 

public class EnemyBeam : MonoBehaviour {
	
	public AttackType attackType;
	public float speed; //Speed in units per second
	public int damage;

	private float flightTime = 0.0f;
	OTSprite sprite;
	// Use this for initialization
	void Start () {
		damage = 10;
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		flightTime = 0.0f;
		//Overwrite the speed always:
		speed = 90.0f;
	}
	
	// Update is called once per frame
	void Update () {
		flightTime += Time.deltaTime;
		if (flightTime > 0.05f)
			sprite.renderer.enabled = true;
		sprite.position += (Vector2)sprite.yVector * speed * Time.deltaTime;
		//remove it if it leaves the view
		if (sprite.outOfView) 
          OT.DestroyObject(sprite);
	}
	
	public void OnCollision(OTObject owner)
	{
		//Check to see if the collision is with an enemy ship
		if (owner.collisionObject.name.Equals("City"))
			HandleHitCity(owner);	
		if (owner.collisionObject.name.Contains("World"))
			HandleHitWorld(owner);
		if (owner.collisionObject.name.Equals("CityShield"))
			HandleHitCityShield(owner);
	}
	
	private void HandleHitCity(OTObject owner)
	{
		//Get a hold of the city data and adjust it
		//TODO implement damage reduction appropriately
		City city = owner.collisionObject.GetComponent<City>();
		city.health -= damage;
		//Remove this projectile
		OT.DestroyObject(sprite);
	}
	
	private void HandleHitWorld(OTObject owner)
	{
		//Get a hold of the city data and adjust it
		//TODO implement damage reduction appropriately
		World world = owner.collisionObject.GetComponent<World>();
		world.health -= damage;
		//Remove this projectile
		OT.DestroyObject(sprite);
	}
	
	private void HandleHitCityShield(OTObject owner)
	{
		//Get a hold of the cityshield data and adjust it
		//TODO implement damage reduction appropriately
		CityShield cityShield = owner.collisionObject.GetComponent<CityShield>();
		cityShield.health -= damage;
		//Remove this projectile
		OT.DestroyObject(sprite);
	}
}
