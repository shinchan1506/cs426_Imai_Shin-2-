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

    public NetworkManager networkManager;
    public override void OnStartServer()
    {
        for (float i = currentTime; i > 0; i-=Time.deltaTime) {
            currentTime-= Time.deltaTime;
            ProgressMsg pmr = new ProgressMsg();
            pmr.type = (int)i;
            // networkManager.client.Send( ProgressMsg.notif, pmr);
            NetworkServer.SendToAll(ProgressMsg.timer, pmr);
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
