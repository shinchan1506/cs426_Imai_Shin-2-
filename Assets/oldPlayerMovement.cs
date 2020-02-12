using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// AUTHOR: Shin Imai
// Movement with character controller
// Camera controls w/ mouse delta



public class oldPlayerMovement : NetworkBehaviour
{    private int numBirds = 0;
    private int numEggs = 0;
    CharacterController characterController;
    Animator anim;
    public GameObject explosion;
    float speed = 0.02f;
    float jumpSpeed = 0.08f;
    float gravity = 0.098f;
    const int SENSITIVITY = 1;
    const float DEFAULT_RADIUS = 1.5f;
    const float MIN_RADIUS = 1;
    const float MAX_RADIUS = 3;
    float radius;

    float oldAngle = 0;
	float oldAngleY = 45;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection;
    private Vector3 viewOffset = new Vector3(0,0.6f,0);
    private bool camLocked = true;
    private bool qDebounce = false;
    public Text numBirdsTxt;
    public Text numEggsTxt;
    public Text timerText;

    public Text binaryText;

    public GameObject Canvas;

    public GameObject progressManager;

    NetworkClient me;
    // public GameObject timer = GameObject.Find("timer");
    void Start ()
    {
        if(!isLocalPlayer)
            return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        radius = DEFAULT_RADIUS;
        numBirdsTxt.text = "Birds: 0";
        numEggsTxt.text = "Eggs: 0";
        Canvas.SetActive(true);
    }

    public override void OnStartLocalPlayer()
    {
        NetworkClient client= new NetworkClient();
        GetComponent<MeshRenderer>().material.color = Color.red;
        print ("client started");
        client.RegisterHandler(ProgressMsg.notif, ReceiveBit);
        client.RegisterHandler(ProgressMsg.timer, ReceiveTime);
        client.Connect("localhost", 7777);
    }

    void ReceiveBit(NetworkMessage n) {
        print("recieved bit");
        String binarySoFar = n.ReadMessage<ProgressMsg>().type.ToString();
        binaryText.text = binarySoFar;
    }

    void ReceiveTime(NetworkMessage n) {
        print ("received time");
        int currentTime = n.ReadMessage<ProgressMsg>().type;
        string min = Mathf.Floor(currentTime/60).ToString("0");
        string sec = (currentTime % 60).ToString("00");
        string textToReturn = "" + min + ":" + sec;
        timerText.text = "Time: " + textToReturn;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer)
            return;
        
        // timerText.text = timer.GetComponent<Timer>().updateText();

        // set parameters for blend tree animations
        float move = Input.GetAxis ("Vertical");
        float moveHoriz = Input.GetAxis("Horizontal");        
        anim.SetFloat("multiplier", 1f);
        // print ("eggs: " + numEggs + " birds: " + numBirds);
        // movement
        if (characterController.isGrounded) {
            moveDirection = Vector3.zero + Camera.main.transform.forward * move + Camera.main.transform.right * moveHoriz;
            if (moveDirection.magnitude > 0) {
                lookDirection = moveDirection;
                anim.SetFloat("Speed", 1);
            } else {
                anim.SetFloat("Speed", 0);
            }
            
            if (Input.GetKey(KeyCode.LeftShift)) {
                moveDirection *= speed * 1.25f;
                anim.SetFloat("multiplier", 4f);
            } else {
                moveDirection *= speed;
            }
            
            if (Input.GetButton("Jump")) {
                moveDirection.y = jumpSpeed;
            }

            if (Input.GetKey(KeyCode.E)) {
                // interact with bird / egg
                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1);
                int i = 0;
                bool foundenemy = false;
                while (i < hitColliders.Length)
                {
                    Collect c = hitColliders[i].GetComponent<Collect>();
                    NetworkIdentity neti = hitColliders[i].GetComponent<NetworkIdentity>();

                    if (neti != null && !isServer) {
                        CmdClientAuthority(neti);
                    }

                    if ( c != null) {
                        int enemyType = c.getEnemyType();
                        this.GetComponent<NetworkObjectDestroyer>().TellServerToDestroyObject(hitColliders[i].gameObject);
                        CmdDestroyBody(hitColliders[i].gameObject);
                        if (isServer) {
                            RpcDestroyBody(hitColliders[i].gameObject);
                        }
                        Destroy(hitColliders[i].gameObject);

                        if (enemyType == 0) {
                            numEggs++;
                            numEggsTxt.text = "Eggs: " + numEggs;
                        } else {
                            numBirds++;
                            numBirdsTxt.text = "Birds: " + numBirds;
                        }
                        foundenemy = true;
                        
                    }
                    
                    i++;
                }
                if (foundenemy) { 
                    explosion.GetComponent<ParticleSystem>().Emit(100);
                }
            }

            if (Input.GetKey(KeyCode.Q) && qDebounce == false) {
                qDebounce = true;
                if (camLocked == true){
                    camLocked = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else{
                    camLocked = true;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                qDebounce = false;
            }
        }
        if (Input.GetButtonDown("Fire1") && numBirds > 0)
        {
            // CmdSpawnBullet(chickenBullet);  
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1);
            int i = 0;
            bool foundenemy = false;
            while (i < hitColliders.Length)
            {
                NetworkIdentity neti = hitColliders[i].GetComponent<NetworkIdentity>();
                CollectEnemy c = hitColliders[i].GetComponent<CollectEnemy>();
                if (neti != null && !isServer) {
                    CmdClientAuthority(neti);
                }

                if ( c != null) {
                    int enemyType = c.getEnemyType();
                    if (enemyType == 1) {
                        numBirds --;
                        numBirdsTxt.text = "Birds: " + numBirds;
                        this.GetComponent<NetworkObjectDestroyer>().TellServerToDestroyObject(hitColliders[i].gameObject);
                        CmdDestroyBody(hitColliders[i].gameObject);
                        if (isServer) {
                            RpcDestroyBody(hitColliders[i].gameObject);
                        }
                        Destroy(hitColliders[i].gameObject);
                        foundenemy = true;  
                        //CmdClientAuthority(progressManager.GetComponent<Progress>().GetComponent<NetworkIdentity>());
                        ProgressMsg pm = new ProgressMsg();
                        pm.type = enemyType;
                        NetworkManager.singleton.client.Send(ProgressMsg.id, pm);
                    }               
                }
                if (foundenemy) { 
                    explosion.GetComponent<ParticleSystem>().Emit(100);
                }
                i++;
            }
        }

        if (Input.GetButtonDown("Fire2") && numEggs > 0)
        {
            // CmdSpawnBullet(eggBullet);
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1);
            int i = 0;
            bool foundenemy = false;
            while (i < hitColliders.Length)
            {
                NetworkIdentity neti = hitColliders[i].GetComponent<NetworkIdentity>();
                CollectEnemy c = hitColliders[i].GetComponent<CollectEnemy>();
                if (neti != null && !isServer) {
                    CmdClientAuthority(neti);
                }

                if ( c != null) {
                    int enemyType = c.getEnemyType();
                    if (enemyType == 0) {
                        numEggs --;
                        numEggsTxt.text = "Eggs: " + numEggs;
                        this.GetComponent<NetworkObjectDestroyer>().TellServerToDestroyObject(hitColliders[i].gameObject);
                        CmdDestroyBody(hitColliders[i].gameObject);
                        if (isServer) {
                            RpcDestroyBody(hitColliders[i].gameObject);
                        }
                        Destroy(hitColliders[i].gameObject);
                        foundenemy = true;   
                        ProgressMsg pm = new ProgressMsg();
                        pm.type = enemyType;
                        NetworkManager.singleton.client.Send(ProgressMsg.id, pm);
                    }               
                }
                if (foundenemy) { 
                    explosion.GetComponent<ParticleSystem>().Emit(100);
                }
                i++;
            }
        }

        

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        // zoom in or zoom out based on scroll wheel
        float deltaScroll = Input.mouseScrollDelta.y;
        if (deltaScroll > 0f) {
            onZoomIn();
        }
        if (deltaScroll < 0f) {
            onZoomOut();
        }

        if (camLocked == true) {
            //camera positioning based on mouse delta
            float deltaX = -Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");
            

            float newAngleY = oldAngleY + (float)(deltaY/SENSITIVITY * Math.PI);

            if (newAngleY < 0) {
                    newAngleY = 0;
            } else if (newAngleY > 80) {
                    newAngleY = 80;
            }

            Vector3 cf = new Vector3(
                        this.transform.position.x + (float)(radius * Math.Cos(DegreeToRadian(oldAngle + (deltaX/SENSITIVITY * Math.PI)))),
                        this.transform.position.y + (float)(radius * Math.Cos(DegreeToRadian(newAngleY))),
                        this.transform.position.z + (float)(radius* Math.Sin(DegreeToRadian(oldAngle + (deltaX/SENSITIVITY * Math.PI))))
                        );

            // update old values
            oldAngle = oldAngle + (float)(deltaX/SENSITIVITY * Math.PI);
            oldAngleY = newAngleY;
            Camera.main.transform.position = cf;
            Camera.main.transform.LookAt(this.transform.position + viewOffset);
        }
        moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z), Vector3.up), Time.deltaTime * 8);
        
        characterController.Move(moveDirection);
    }

    double DegreeToRadian(double angle)
    {
        return Math.PI * angle / 180.0;
    }
    
    void onZoomOut() {
		if (radius < MAX_RADIUS)
			radius = radius + 0.5f;
    }

    void onZoomIn() {
		if (radius > MIN_RADIUS)
			radius = radius - 0.5f;
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

    [ClientRpc]
    public void RpcDestroyBody(GameObject c) {
        NetworkServer.Destroy(c);
        Destroy(c);
    }

    [Command]
    public void CmdDestroyBody(GameObject c) 
    {   
        RpcDestroyBody(c);
        Destroy(c);
    }

}