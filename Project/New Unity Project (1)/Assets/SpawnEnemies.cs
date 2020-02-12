using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnEnemies : NetworkBehaviour
{
    public GameObject zeroEnemy;
    public GameObject oneEnemy;

    public GameObject ProgressManager;
    GameObject instantiatedZero;
    GameObject instantiatedOne;

    Random rnd = new Random();
    // Start is called before the first frame update
    public override void OnStartServer()
    {
        // GameObject pm = Instantiate(ProgressManager);
        // NetworkServer.Spawn(pm);

        for (int i = 1; i < 6; i++) {
            instantiatedZero = Instantiate(zeroEnemy, GameObject.Find("Cube" + i.ToString()).transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);
            instantiatedOne = Instantiate(oneEnemy, GameObject.Find("Cube" + i.ToString()).transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);
            NetworkServer.Spawn(instantiatedZero);
            NetworkServer.Spawn(instantiatedOne);
        }
    }

    private void Awake() {
        DontDestroyOnLoad(this);
    }

}
