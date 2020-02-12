using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkObjectDestroyer : NetworkBehaviour
{
    public static NetworkObjectDestroyer Instance;

    // Called by the Player
    [Client]
    public void TellServerToDestroyObject(GameObject obj)
    {
        CmdDestroyObject(obj);
    }

    // Executed only on the server
    [Command]
    private void CmdDestroyObject(GameObject obj)
    {
        if(!obj) return;

        NetworkServer.Destroy(obj);
    }

    private void Awake()
    {
        // skip if not the local player
        if(!isLocalPlayer) return;

        // set the static instance
        Instance = this;
    }
}