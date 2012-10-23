/*Targets.cs - Brokion project
 * Benjamin Johnson
 * Copyright Blank Sketch Studios, LLC 2012
 * 
 * @Since Aug 13 2012
 * @Updated Aug 14 2012
 * 
 */ 
using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour {
	
	private static OTSprite placement1;
	private static OTSprite placement2;
	private static OTSprite placement3;
	private static OTSprite placement4; 
	private static OTSprite placement5; 
	private static OTSprite placement6;
	private static OTSprite city;
	
	//Target lists
	private static ArrayList allTargets = new ArrayList();
	private static ArrayList noCity = new ArrayList();
	public static ArrayList enemyTargets = new ArrayList();
		
	// Use this for initialization
	void Start () 
	{
		placement1 = GameObject.Find("Placement1").GetComponent<OTSprite>();
		placement2 = GameObject.Find("Placement2").GetComponent<OTSprite>();
		placement3 = GameObject.Find("Placement3").GetComponent<OTSprite>();
		placement4 = GameObject.Find("Placement4").GetComponent<OTSprite>();
		placement5 = GameObject.Find("Placement5").GetComponent<OTSprite>();
		placement6 = GameObject.Find("Placement6").GetComponent<OTSprite>();
		city = GameObject.Find("City").GetComponent<OTSprite>();
		
		allTargets.Add(placement1);
		allTargets.Add(placement2);
		allTargets.Add(placement3);
		allTargets.Add(placement4);
		allTargets.Add(placement5);
		allTargets.Add(placement6);
		allTargets.Add(city);
		
		noCity.Add(placement1);
		noCity.Add(placement2);
		noCity.Add(placement3);
		noCity.Add(placement4);
		noCity.Add(placement5);
		noCity.Add(placement6);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static OTSprite PickRandomTargetFromAll()
	{
		int choiceIndex = Random.Range(0,allTargets.Count);
		OTSprite choice = (OTSprite)allTargets[choiceIndex];
		return choice;
	}
	public static OTSprite PickRandomTargetNoCity()
	{
		int choiceIndex = Random.Range(0,noCity.Count);
		OTSprite choice = (OTSprite)noCity[choiceIndex];
		return choice;
	}
	
	public static OTSprite PickRandomEnemyTarget()
	{
		int choiceIndex = Random.Range(0,enemyTargets.Count);
		OTSprite choice = (OTSprite)enemyTargets[choiceIndex];
		return choice;
	}
}
