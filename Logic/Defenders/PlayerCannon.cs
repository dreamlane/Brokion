/*PlayerCannon.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 6 2012
 * @Updated Aug 6 2012
 * 
 */ 

using UnityEngine;
using System.Collections;

/*
 * This class will handle all of the attacking, animation, damage health and etc related to the player controlled cannon
 */

public class PlayerCannon : MonoBehaviour {
	
	// Let's get a handle on the targetable area for the player cannon
	OTObject targetSpace;
	
	float timeSinceLastShot; //How long it's been since the last shot was fired, in seconds
	bool shotAvailable;
	OTSprite myProjectile; //A handle for the PlayerCannonProjectiles fired.
	OTSprite myBarrel;
	
	//Upgradable values:
	public float projectileSpeed;
	public int projectileDamage;
	public float reloadTime; //How long of a delay between shots, in seconds

	// Use this for initialization
	void Start () 
	{
		targetSpace = GameObject.Find("TargetSpace").GetComponent<OTSprite>();
		targetSpace.onInput = OnInput;
		
		myBarrel = OT.ObjectByName("PlayerCannonBarrel") as OTSprite;
		
		reloadTime = 1.0f;
		timeSinceLastShot = 0.0f;
		shotAvailable = true;
		projectileSpeed = 15.0f;
		projectileDamage = 5;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!(shotAvailable))
		{
			timeSinceLastShot += Time.deltaTime;
			if (timeSinceLastShot > reloadTime)
			{
				shotAvailable = true;
				timeSinceLastShot = 0.0f;
			}
		}
	}
	
	void OnInput(OTObject owner)
	{
		if (owner.name.Equals("TargetSpace"))
		{
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
			{
				//Debug.Log("Touched targetable space, FIRE CANNON!");
				//Rotate the barrel, (TODO: OTTween this eventually)
				Vector3 touchPoint3 = Camera.mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
				Vector2 touchPoint = new Vector2(touchPoint3.x,touchPoint3.y);
				myBarrel.RotateTowards(touchPoint);
				//print("barrel rot: "+myBarrel.rotation+" touch pos: "+Input.GetTouch(0).position+"barrel pos: "+myBarrel.position);
				
				//Get the coords of the touch, and fire a projectile in that direction, if the cannon is off cooldown
				if (shotAvailable)
				{
					//Generate a projectile
					GameObject bullet = OT.CreateObject("PlayerCannonProjectile");
					myProjectile = bullet.GetComponent<OTSprite>();
					myProjectile.renderer.enabled = true;
					
					// Put it at the end of the barrel, with the same rotation as the barrel
					myProjectile.rotation = myBarrel.rotation;
					myProjectile.position = myBarrel.position + (myBarrel.yVector * myBarrel.size.y);
					
					//Assign the projectile its atributes.
					PlayerCannonProjectile pcp = (PlayerCannonProjectile)myProjectile.GetComponent(typeof(PlayerCannonProjectile));
					pcp.speed = projectileSpeed;
					pcp.damage = projectileDamage;
					pcp.isFlak = true;
					pcp.target = touchPoint;
					pcp.shellFlightTime = 0.5f;
					//Make shot unavailable
					shotAvailable = false;

				}
			}
		}
	}
}
