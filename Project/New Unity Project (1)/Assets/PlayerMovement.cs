using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    Animator anim;
    int jumpHash = Animator.StringToHash("Jump");
    int runStateHash = Animator.StringToHash("Base Layer.Run");
    float defaultspeed = 0.01f;

    void Start ()
    {
        anim = GetComponent<Animator>();
    }

    public GameObject bulletPrefab;

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer)
            return;

        float move = Input.GetAxis ("Vertical");
        float moveHoriz = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", move);
        anim.SetFloat("HorizSpeed", moveHoriz);
        var x = Input.GetAxis("Horizontal")* defaultspeed;
        var z = Input.GetAxis("Vertical")* defaultspeed;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        // if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == runStateHash)
        // {
        //     anim.SetTrigger (jumpHash);
        // }
        
        Camera.main.transform.position = this.transform.position - this.transform.forward * 1.5f
        + this.transform.up*1.25f;
        Camera.main.transform.LookAt(this.transform.position);
        Camera.main.transform.parent = this.transform;

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Fire();
        // }

        // if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D)) {
        //     Animation walkinganim = 
        // }

        transform.Translate(x, 0, z);
    }

    void Fire()
    {
        // create the bullet object from the bullet prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            transform.position - transform.forward,
            Quaternion.identity);

        // make the bullet move away in front of the player
        bullet.GetComponent<Rigidbody>().velocity = -transform.forward*4;
        
        // make bullet disappear after 2 seconds
        Destroy(bullet, 2.0f);        
    }
}