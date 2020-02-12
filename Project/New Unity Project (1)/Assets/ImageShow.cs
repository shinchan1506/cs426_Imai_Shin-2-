 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;
 
 public class ImageShow : MonoBehaviour {
 
     public bool isImgOn;
     public Image img;
 
     void Start () {
 
         img.enabled = false;
         isImgOn = false;
     }
 
     void Update () {
     
         if (Input.GetKeyDown ("y")) {
 
             if (isImgOn == false) {
 
                 img.enabled = true;
                 isImgOn = true;
             }
 
             else {
 
                 img.enabled = false;
                 isImgOn = false;
             }
         }
     }
 }