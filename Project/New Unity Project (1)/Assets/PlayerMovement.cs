using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// AUTHOR: Shin Imai
// Movement with character controller
// Camera controls w/ mouse delta

public class PlayerMovement : NetworkBehaviour
{
    CharacterController characterController;
    Animator anim;
    float speed = 3f;
    float jumpSpeed = 8.0f;
    float gravity = 20.0f;
    const int SENSITIVITY = 1;
    const float DEFAULT_RADIUS = 1.5f;
    const float MIN_RADIUS = 1;
    const float MAX_RADIUS = 3;
    float radius;
    Vector3 moveDirection = Vector3.zero;

    float oldAngle = 0;
    float oldAngleY = 45;
    private float forward;
    private bool camLocked = true;
    private bool horizontalPressed = false;
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        radius = DEFAULT_RADIUS;
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

        bool rotateCharacter = false;
        // set parameters for blend tree animations
        float move = Input.GetAxis ("Vertical");
        float moveHoriz = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", move);
        anim.SetFloat("HorizSpeed", moveHoriz);

        // movement
        if (characterController.isGrounded) {
            moveDirection = Vector3.zero + Camera.main.transform.forward * move + Camera.main.transform.right * moveHoriz;
            moveDirection *= speed;

            if (Input.GetButton("Jump")) {
                moveDirection.y = jumpSpeed;
            }

            if (Input.GetKey(KeyCode.E)) {
                // interact with bird / egg
            }

            if (Input.GetKeyUp(KeyCode.Tab)) {
                if (camLocked == true){
                    camLocked = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else{
                    camLocked = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            if (moveDirection.magnitude > 0) {
                rotateCharacter = true;
            }
            print(moveDirection);
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

            // primitive anti ground clipping measures
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
            Camera.main.transform.LookAt(this.transform.position + new Vector3(0,0.6f,0));
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        if (!(move == 0 && moveHoriz == 0))
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z), Vector3.up), Time.deltaTime * 360);
        
        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);
        //if (move != 0 || (moveHoriz != 0 && move != 0)) {        
        
    }

    private double DegreeToRadian(double angle)
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

}