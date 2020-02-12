using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    // Start is called before the first frame update
    public int EnemyType;

    // Update is called once per frame
    void Update()
    {
        
    }

    public int destroyBody() 
    {
        Destroy(gameObject);
        return EnemyType;
    }
}
