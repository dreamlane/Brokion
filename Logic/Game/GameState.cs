/*GameState.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 8 2012
 * @Updated Aug 13 2012
 * 
 */ 

using UnityEngine;
using System.Collections;

/*
 * This class will be used to handle the state of the game, things like score, paused state, game over, title screen...
 */ 
public class GameState : MonoBehaviour {
	
	//Wave states
	public int activeShipCount;
	public int currentWave;
	private float noShipDuration;
	public bool betweenRounds;
	public float timeToNextRoundStart;
	private const float WAVE_CHANGE_DELAY = 6.0f;
	
	//player states
	public int playerScore;
	public int currentResources;
	public int spentResources;
	
	//placement state
	public bool isPlacementGUIActive;
	// Use this for initialization
	void Start () {
		activeShipCount = 0;
		currentWave = 1;
		noShipDuration = 0.0f;
		betweenRounds = false;
		playerScore = 0;
		currentResources = 10;
		isPlacementGUIActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!betweenRounds)
		{
			if (activeShipCount == 0 && noShipDuration > WAVE_CHANGE_DELAY)
			{
				//There have been no ships on the board for awhile, time to progress to the next wave
				currentWave += 1;
				noShipDuration = 0.0f;
				betweenRounds = true;
				timeToNextRoundStart = Constants.TIME_BETWEEN_ROUNDS;
			}
			else
			{
				noShipDuration += Time.deltaTime;
			}
		}
		else //we're between rounds
		{
			timeToNextRoundStart -= Time.deltaTime;
			if (timeToNextRoundStart <= 0.0f)
				betweenRounds = false;
		}
	}
	
	/*
	 * Here we handle the game state gui
	 *  Stuff like resources, score, current wave
	 **/
	void OnGUI()
	{
		GUI.Box(new Rect(780,10,120,20),string.Format("Resources: {0}",currentResources));
	}	
}
