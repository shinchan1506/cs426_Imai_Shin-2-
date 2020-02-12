using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class Timer : NetworkBehaviour
{
    float currentTime = 600.0f;
    // public Text text;
    string textToReturn = "";
    
    GameObject[] players;

    public override void OnStartServer()
    {
        for (float i = currentTime; i > 0; i--) {
            currentTime--;
            string min = Mathf.Floor(currentTime/60).ToString("0");
            string sec = (currentTime % 60).ToString("00");
            textToReturn = "" + min + ":" + sec;
            RpcUpdateText(textToReturn);
        }        
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag ("Player");
    }

    public string updateText() {
        return textToReturn;
    }

    [ClientRpc]
    public void RpcUpdateText(string text) {
        foreach (GameObject p in players) {
            p.GetComponent<Canvas>();
        }
    }


}
