using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float currentTime = 600.0f;
    // public Text text;
    string textToReturn = "";
    
    void Update()
    {
        currentTime -= Time.deltaTime;
        string min = Mathf.Floor(currentTime/60).ToString("0");
        string sec = (currentTime % 60).ToString("00");
        textToReturn = "" + min + ":" + sec;
    }

    public string updateText() {
        return textToReturn;
    }
}
