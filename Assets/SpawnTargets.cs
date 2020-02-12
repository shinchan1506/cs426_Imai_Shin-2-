using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnTargets : NetworkBehaviour
{
    public GameObject bird;
    public GameObject egg;

    GameObject instantiated;
    GameObject instantiatedEgg;

    Random rnd = new Random();

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        for (int i = 0; i < 5; i++) {
            instantiated = Instantiate(egg, GameObject.Find("Cube1").transform.transform.position, Quaternion.identity);
            instantiatedEgg = Instantiate(bird, GameObject.Find("Cube1").transform.position, Quaternion.identity);
            NetworkServer.Spawn(instantiated);
            NetworkServer.Spawn(instantiatedEgg);
        }
    }

    private void Awake() {
        DontDestroyOnLoad(this);
    }

}
