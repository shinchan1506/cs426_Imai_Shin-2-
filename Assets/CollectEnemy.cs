using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CollectEnemy : NetworkBehaviour
{
    // Start is called before the first frame update
    public int EnemyType;
    public GameObject parent;
    public GameObject progressManager;

    public int getEnemyType() {
        return EnemyType;
    }

    public GameObject getGameObject() {
        return parent;
    }
    

    // private void OnCollisionEnter(Collision collision) {
    //     Debug.Log("Collision Detected " + collision.gameObject.name);
    //     if ((collision.gameObject.name.Contains("bird") && EnemyType == 1) ||
    //         (collision.gameObject.name.Contains("Egg") && EnemyType == 0))  {
    //             CmdaddToScore(EnemyType);
    //     }
    // }

    // [Command]
    // void CmdaddToScore(int EnemyType) {
    //     progressManager.GetComponent<Progress>().AddDigit(EnemyType);
    //     Debug.Log("Collision is bullet");
    //     Destroy(gameObject);
    // }

}
