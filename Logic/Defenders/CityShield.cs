/*City.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 4 2012
 * @Updated Aug 14 2012
 * 
 */ 

using UnityEngine;
using System.Collections;

/*
 * The sheild is the primary protector for the city, it regenerates health faster than any other object in the game.
 * 
 * Behaviour: when the shield reaches 0, it drops and no longer blocks incoming attacks, it then recharges it's health to 50% of max and restarts
 * 
 * 
 */ 
public class CityShield : MonoBehaviour {
	
	public float maxHealth;
	public float health;
	public float regenRate; //health regeneration rate per second
	public bool shieldDown;
	OTSprite sprite;
	// Use this for initialization
	void Start () {
		maxHealth = 100;
		health = 100;
		regenRate = 2;
		shieldDown = false;
		sprite = GetComponent<OTSprite>();
		sprite.onInput = OnInput;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Actions to take when the shield is down
		if (shieldDown)
		{
			//check to see if it's time to bring the shield up
			if (maxHealth/2 <= health) //bring the shields back up when they reach 50%
			{
				sprite.collidable = true;
				sprite.renderer.enabled = true;
				shieldDown = false;
			}
			//Apply regeneration
			health+=regenRate*Time.deltaTime;
		}
		//Actions to take when the shield is up
		else
		{
			//Check to see if the shield has been depleted
			if (health <= 0)
			{
				sprite.collidable = false;
				sprite.renderer.enabled = false;
				shieldDown = true;
			}
			//Apply regeneration
			health+=regenRate*Time.deltaTime;
			if (health > maxHealth)
				health = maxHealth;
		}
	}

	void OnInput(OTObject owner)
	{
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			Debug.Log("City was touched");	
	}
}
