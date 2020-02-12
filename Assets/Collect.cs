using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Collect : NetworkBehaviour
{
    // Start is called before the first frame update
    public int EnemyType;
    public GameObject parent;

    public int getEnemyType() {
        return EnemyType;
    }

    public GameObject getGameObject() {
        return parent;
    }
}
