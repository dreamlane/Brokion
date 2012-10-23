/*EnemyShip.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 8 2012
 * @Updated Aug 8 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour {
	
	//Attributes that all Enemy Ships will have
	public int health;
	public ArmorType armor;
	public float moveSpeed;
	public int resourceReward;
	public int scoreReward;
	//Stunning
	public bool stunned;
	public float stunnedTime;
	public float stunDuration;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	

}
