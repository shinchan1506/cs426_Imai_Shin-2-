using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 using UnityEngine.UI;

public class Bargain : MonoBehaviour {

	public bool isYes;
	public bool isNo;

	public Image bad;
	public Image good;

	void Start(){
		bad.enabled = false;
		good.enabled = false;
	}

	void Update (){
		if (Input.GetKeyDown ("y")){
			bad.enabled = true;
			good.enabled = false;
		}

		if (Input.GetKeyDown ("n")){
			good.enabled = true;
			bad.enabled = false;
		}

	}
	// void OnMouseUp(){
	// 	if(isYes)
	// 	{
	// 		bad.enabled = true;
	// 		good.enabled = false;
	// 	}
	// 	if (isNo)
	// 	{
	// 		good.enabled = true;
	// 		bad.enabled = false;
	// 	}
	// }

}

