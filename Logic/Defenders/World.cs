/*World.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 13 2012
 * @Updated Aug 13 2012
 * 
 */ 

using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	
	public int health;
	public int regenRate; //health regeneration rate per second
	OTSprite sprite;
	// Use this for initialization
	void Start () {
		health = 100;
		sprite = GetComponent<OTSprite>();
		sprite.onInput = OnInput;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnInput(OTObject owner)
	{
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			Debug.Log("world was touched");	
	}
}
