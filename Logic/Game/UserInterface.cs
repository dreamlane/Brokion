using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void onPlacementTapped() {
		if (GUI.Button (new Rect (10,10,150,100), "I am a button")) {
			print ("You clicked the button!");
		}	
	}
}
