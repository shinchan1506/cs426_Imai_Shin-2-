using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bargain : MonoBehaviour {

	public bool isYes;
	public bool isNo;

	void Start(){
		
	}

	void OnMouseUp(){
		if(isYes)
		{
			// create win screen for player
			// create losing screen for others
		}
		if (isNo)
		{
			// load next level
			// Application.LoadLevel(2);
		}
	}

}

