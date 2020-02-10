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
    float speed = 0.01f;
    float jumpSpeed = 8.0f;
    float gravity = 20.0f;
    const int SENSITIVITY = 1;
    const float DEFAULT_RADIUS = 1.5f;
    const float MIN_RADIUS = 1;
    const float MAX_RADIUS = 3;
    float radius;

    float oldAngle = 0;
	float oldAngleY = 45;
    private float forward;
    private Vector3 moveDirection = Vector3.zero;

    private bool camLocked = true;
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

        // set parameters for blend tree animations
        float move = Input.GetAxis ("Vertical");
        float moveHoriz = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", move);
        anim.SetFloat("HorizSpeed", moveHoriz);
        

        // movement
        if (characterController.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButton("Jump")) {
                moveDirection.y = jumpSpeed;
            }

            if (Input.GetKey(KeyCode.E)) {
                // interact with bird / egg
            }

            if (Input.GetKeyUp(KeyCode.Escape)) {
                if (camLocked == true){
                    camLocked = false;
                    Cursor.lockState = CursorLockMode.None;
                }
                else{
                    camLocked = true;
                    Cursor.lockState = CursorLockMode.Locked;
                }
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
            Camera.main.transform.LookAt(this.transform.position);
            Camera.main.transform.parent = this.transform;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller
        characterController.Move(moveDirection);
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