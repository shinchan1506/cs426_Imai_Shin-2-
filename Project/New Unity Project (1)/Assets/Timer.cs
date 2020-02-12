using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : NetworkBehaviour
{
    public TextMesh timer;
    float currentTime = 300.0f;
    // public Text text;
    string textToReturn = "";
    
    GameObject[] players;

    public NetworkManager networkManager;
    void Update()
    {
        currentTime -= Time.deltaTime;
        string min = Mathf.Floor(currentTime/60).ToString("0");
        string sec = (currentTime % 60).ToString("00");
        textToReturn = "" + min + ":" + sec;
        timer.text = textToReturn;

        if (currentTime <= 0.0f) {
            SceneManager.LoadScene("bargain");
            SceneManager.UnloadScene("desert");
            SceneManager.UnloadScene("efwef");
            SceneManager.UnloadScene("level 1");
        }
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
