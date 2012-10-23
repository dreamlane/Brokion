/*MineralDrill.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 14 2012
 * @Updated Aug 15 2012
 */ 
using UnityEngine;
using System.Collections;

/*
 * Gives the player extra resources per wave
 * 
 * upgrades:
 * resources per wave.
 */ 
public class MineralDrill : MonoBehaviour {
	
	//Turret attributes set externally
	public int health;
	public int maxHealth;
	public int resourceRate;
	private int currentWave;
	OTSprite sprite;
	
	GameState gameState;
	// Use this for initialization
	void Start () {
		sprite = GetComponent<OTSprite>();
		gameState = (GameState)GameObject.Find("GameLogic").GetComponent(typeof(GameState));
		currentWave = gameState.currentWave;
	}
	
	// Update is called once per frame
	void Update () {
		//Determine if the wave has ended
		if (gameState.currentWave != currentWave)
		{
			//the wave has changed, feed the player resources
			gameState.currentResources += resourceRate;
			//update the drills knowledge
			currentWave = gameState.currentWave;
		}
	}
}
