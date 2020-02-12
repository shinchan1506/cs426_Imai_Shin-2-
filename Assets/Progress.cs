using System.Collections;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Progress : NetworkBehaviour
{

    private static string binary = "";
    private int targetInt;
    public static Progress Instance;
    public NetworkManager networkManager;
    public TextMesh sign;
    
    void Start() {
        binary = "";
        targetInt = -1;
    }

    public override void OnStartServer() {
        NetworkServer.RegisterHandler(ProgressMsg.id, ReceiveBit);
    }

    private void ReceiveBit(NetworkMessage pm) {
        binary = pm.ReadMessage<ProgressMsg>().type.ToString() + binary;
        print("my progress so far is: " + binary); 
        ProgressMsg pmr = new ProgressMsg();
        pmr.type = Convert.ToInt32(binary, 10);
        if (Convert.ToInt32(binary,2) == 25) {
            SceneManager.LoadScene("main menu");
            SceneManager.UnloadScene("desert");
            SceneManager.UnloadScene("efwef");
            SceneManager.UnloadScene("level 1");

        }
        // networkManager.client.Send( ProgressMsg.notif, pmr);
        // NetworkServer.SendToAll( ProgressMsg.notif, pmr);
        sign.text = binary;
    }       

    public void setTargetInt(int num) {
        targetInt = num;
    }

    [Client]
    public void AddDigit(int digit, NetworkIdentity networkIdentity) {
        this.GetComponent<NetworkIdentity>().AssignClientAuthority(networkIdentity.connectionToClient);
        CmdAddDigit(digit);
        this.GetComponent<NetworkIdentity>().RemoveClientAuthority(networkIdentity.connectionToClient);
    }

    [Command]
    public void CmdAddDigit(int digit) {
        binary = digit.ToString() + binary;
        print("my progress so far is: " + binary);
        if(Convert.ToInt32(binary,2) == targetInt) {
            // win
        }
        if (Convert.ToInt32(binary,2) > targetInt) {
            // lose
        }
    }

    [Command] 
    void CmdClientAuthority(NetworkIdentity neti)
    {
        neti.AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToServer);
    }

    [Command]
    void CmdRemoveClientAuthority(NetworkIdentity neti) {
        neti.RemoveClientAuthority(this.GetComponent<NetworkIdentity>().connectionToServer);
    }

    public string getBinary() {
        return binary;
    }

    private void Awake()
    {
        // skip if not the local player
        if(!isLocalPlayer) return;

        // set the static instance
        Instance = this;
    }
}
