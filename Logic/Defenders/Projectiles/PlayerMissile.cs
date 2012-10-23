/*PlayerCannonPojectile.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 15 2012
 * @Updated Aug 15 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

/*
 * FUTURE WORK: Consider reworking the behaviour of a missile when it's target is not visible.
 * 
 */ 
public class PlayerMissile : MonoBehaviour {

	public float speed; //Speed in units per second
	public int damage;
	public bool isHarvester; //Harvester missiles return an increased number of resources per kill
	public bool isSmart; //Smart missiles dodge enemy attacks and only hit high value targets
	
	OTSprite sprite;
	OTSprite target;
	
	public float harvestPercentage;
	
	private float rotationMax = 2.0f;
	
	GameState gameState;
	// Use this for initialization
	void Start () {
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		damage = 10;
		isHarvester= false;
		isSmart = false;
		sprite = GetComponent<OTSprite>();
		sprite.onCollision = OnCollision;
		harvestPercentage = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//Make sure the round hasn't ended
		if (!gameState.betweenRounds)
		{
			//make sure the target is still alive
			if (Targets.enemyTargets.Contains(target))
			{
				//rotate towards the target
					float origRotation = sprite.rotation;
					sprite.RotateTowards(target.position);
					float newRotation = sprite.rotation;
					if (!((360 - Mathf.Abs(origRotation-newRotation)) < rotationMax || Mathf.Abs(origRotation-newRotation) < rotationMax))
					{
						//the rotation is too sharp, so lets go the max increment
						//if the original rotation is less than the new rotation, then 360-abs is left, and abs is right
						if (origRotation < newRotation)
						{
							//If the shortest distance is to the right, then add the maxRotation (turn right)
							if ((360 - Mathf.Abs(origRotation-newRotation)) > Mathf.Abs(origRotation-newRotation))
							{
								sprite.rotation = origRotation+rotationMax;
							}
							else
							{
								sprite.rotation = origRotation-rotationMax;
							}
						}
						else //if the original rotation is larger than the new rotation, then 360-abs is right, and abs is left
						{
							//If the shortest distance is to the left, then subtract the maxRotation (turn left)
							if ((360 - Mathf.Abs(origRotation-newRotation)) > Mathf.Abs(origRotation-newRotation))
							{
								sprite.rotation = origRotation-rotationMax;
							}
							else
							{
								sprite.rotation = origRotation+rotationMax;
							}
						}
					}
					//If it's not a bigger rotation than possible, just leave it be...
					//NOTE that equal rotations are accurately handled here
					sprite.position += (Vector2)sprite.yVector	* speed * Time.deltaTime;
				//remove it if it leaves the view
				if (sprite.outOfView) 
          			OT.DestroyObject(sprite);
			}
			else //The target has been destroyed
			{
				//print ("Target destroyed");
				//If there are active ships, lets find a new one
				if (gameState.activeShipCount > 0)
				{
					//find a new target
					target = Targets.PickRandomEnemyTarget();
					//rotate towards the target
					float origRotation = sprite.rotation;
					sprite.RotateTowards(target.position);
					float newRotation = sprite.rotation;
					if (!((360 - Mathf.Abs(origRotation-newRotation)) < rotationMax || Mathf.Abs(origRotation-newRotation) < rotationMax))
					{
						//the rotation is too sharp, so lets go the max increment
						//if the original rotation is less than the new rotation, then 360-abs is left, and abs is right
						if (origRotation < newRotation)
						{
							//If the shortest distance is to the right, then add the maxRotation (turn right)
							if ((360 - Mathf.Abs(origRotation-newRotation)) > Mathf.Abs(origRotation-newRotation))
							{
								sprite.rotation = origRotation+rotationMax;
							}
							else
							{
								sprite.rotation = origRotation-rotationMax;
							}
						}
						else //if the original rotation is larger than the new rotation, then 360-abs is right, and abs is left
						{
							//If the shortest distance is to the left, then subtract the maxRotation (turn left)
							if ((360 - Mathf.Abs(origRotation-newRotation)) > Mathf.Abs(origRotation-newRotation))
							{
								sprite.rotation = origRotation-rotationMax;
							}
							else
							{
								sprite.rotation = origRotation+rotationMax;
							}
						}
					}
					//If it's not a bigger rotation than possible, just leave it be...
					//NOTE that equal rotations are accurately handled here
					sprite.position += (Vector2)sprite.yVector	* speed * Time.deltaTime;
				}
				else
				{
					//print("no targets");
					//continue in a forward direction
					sprite.position += (Vector2)sprite.yVector	* speed * Time.deltaTime;
				}
			}
		}
		else //If the round has ended, remove the missile
		{
			OT.DestroyObject(sprite);
		}
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
		
		//Handle harvest missiles
		if (isHarvester)
		{
			//TODO: Add consideration for armor
			if (enemy.health <= damage) //it's going to be a killing blow
				gameState.playerScore += (int)(enemy.resourceReward*harvestPercentage);
			enemy.health -= damage;
			//Remove this projectile
			OT.DestroyObject(sprite);
		}
		//Handle smart missiles
		else if (isSmart)
		{
			//Check to see if the enemy is our target
			if (owner.collisionObject.Equals(target))
			{
				enemy.health -= damage;
				//Remove this projectile
				OT.DestroyObject(sprite);
			}
			//not our target? Ignore collision
		}
		else //normal missile, just deal damage and remove the sprite
		{
			enemy.health -= damage;
			//Remove this projectile
			OT.DestroyObject(sprite);
		}	
	}
}
