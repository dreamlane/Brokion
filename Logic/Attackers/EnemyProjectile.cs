/*EnemyProjectile.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 8 2012
 * @Updated Aug 8 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {
	
	public AttackType attackType;
	public float speed; //Speed in units per second
	public int damage;
	bool targetable; //this needs to be considered in the future
	OTSprite sprite;
	// Use this for initialization
	void Start () {
		damage = 10;
		targetable = false;
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
	}
	
	// Update is called once per frame
	void Update () {
		sprite.position += (Vector2)sprite.yVector * speed * Time.deltaTime;
		//remove it if it leaves the view
		if (sprite.outOfView) 
          OT.DestroyObject(sprite);
	}
	
	void Explode()
	{
		//Apply exploding effect
		
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
