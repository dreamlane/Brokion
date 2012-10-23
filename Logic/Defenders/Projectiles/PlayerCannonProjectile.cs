/*PlayerCannonPojectile.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 7 2012
 * @Updated Aug 15 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

public class PlayerCannonProjectile : MonoBehaviour {
	
	//upgradable values
	public float speed; //Speed in units per second
	public int damage;
	public float shellFlightTime;
	
	
	public bool isFlak;
 	public bool isSuperFlak;
	public bool isFlakShell;
	public Vector2 target;

	
	OTSprite sprite;
	OTSprite myProjectile;
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
	}
	
	// Update is called once per frame
	void Update () {
		sprite.position += (Vector2)sprite.yVector	* speed * Time.deltaTime;
		
		//check to see if we've reached our target (or within a short distance from it... may need to use ForceUpdate() for this)
		if (Mathf.Abs(sprite.position.x-target.x) < 0.5 && Mathf.Abs(sprite.position.y-target.y) < 0.5)
			HandleTargetReached();
		//remove it if it leaves the view
		if (sprite.outOfView) 
          OT.DestroyObject(sprite);
	}
	
	void Explode()
	{
		//Apply exploding effect
		
		
		//Remove from game
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
		//Handle flak projectiles
		if (isFlak)
		{
			//make an explosion of slugs generate 5 slugs that fly in random directions... OLD BEHAVIOUR
			/*
			for (int i = 0; i < 5; i++)
			{
				GameObject bullet = OT.CreateObject("PlayerCannonProjectile");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = true;
								
				// fire in a random direction
				Vector2 target = new Vector2(Random.value*100-50,Random.value*100-50);
				myProjectile.position = sprite.position;
				myProjectile.RotateTowards(target);			
						
				//Assign the projectile its atributes.
				PlayerCannonProjectile pcp = (PlayerCannonProjectile)myProjectile.GetComponent(typeof(PlayerCannonProjectile));
				pcp.damage = damage;
				pcp.speed = speed;
				pcp.isFlak = false;
			}
			*/
			//Now we do nothing special here
		}
		//Handle super flaks
		if (isSuperFlak)
		{
			/*
			//make an explosion of slugs generate 5 slugs that fly in random directions
			for (int i = 0; i < 5; i++)
			{
				GameObject bullet = OT.CreateObject("PlayerCannonProjectile");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = true;
								
				// fire in a random direction
				Vector2 target = new Vector2(Random.value*100-50,Random.value*100-50);
				myProjectile.position = sprite.position;
				myProjectile.RotateTowards(target);			
						
				//Assign the projectile its atributes.
				PlayerCannonProjectile pcp = (PlayerCannonProjectile)myProjectile.GetComponent(typeof(PlayerCannonProjectile));
				pcp.damage = damage;
				pcp.speed = speed;
				pcp.isFlak = true;
				pcp.isSuperFlak = false;
			}
			*/
			// Now we do nothing special here
		}
		//Remove this projectile
		OT.DestroyObject(sprite);
	}
	
	//This is called when the projectile has reached its target. not important for anything but flak type projectiles
	private void HandleTargetReached()
	{
		//print ("target reached");
		if (isSuperFlak)
		{
			//generate x number of targets nearby to fire flak projectiles at
			//fire the projectiles
			//destroy this projectile
			for (int i = 0; i < 3; i++)
			{
				GameObject bullet = OT.CreateObject("PlayerCannonProjectile");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = true;
								
				// fire at a target at 0degrees 10degrees 20degrees... 350degrees
				myProjectile.position = sprite.position;
				myProjectile.rotation = i*120.0f;		
						
				//Assign the projectile its atributes.
				PlayerCannonProjectile pcp = (PlayerCannonProjectile)myProjectile.GetComponent(typeof(PlayerCannonProjectile));
				pcp.damage = damage;
				pcp.speed = speed;
				pcp.isFlak = true;
				pcp.isSuperFlak = false;
				pcp.isFlakShell = false;
				pcp.shellFlightTime = this.shellFlightTime;
				pcp.target = myProjectile.position + (Vector2)myProjectile.yVector*speed*shellFlightTime*2;
			}
		}
		else if (isFlak)
		{
			//generate 36 flakshells and fire them in 10 degree intervals around the ship
			for (int i = 0; i<36;i++)
			{
				GameObject bullet = OT.CreateObject("PlayerCannonProjectile");
				myProjectile = bullet.GetComponent<OTSprite>();
				myProjectile.renderer.enabled = true;
								
				// fire at a target at 0degrees 10degrees 20degrees... 350degrees
				myProjectile.position = sprite.position;
				myProjectile.rotation = i*10.0f;		
						
				//Assign the projectile its atributes.
				PlayerCannonProjectile pcp = (PlayerCannonProjectile)myProjectile.GetComponent(typeof(PlayerCannonProjectile));
				pcp.damage = damage;
				pcp.speed = speed;
				pcp.isFlak = false;
				pcp.isSuperFlak = false;
				pcp.isFlakShell = true;
				pcp.target = myProjectile.position + (Vector2)myProjectile.yVector*speed*shellFlightTime;
			}
		}
		
		OT.DestroyObject(sprite);
	}
}
