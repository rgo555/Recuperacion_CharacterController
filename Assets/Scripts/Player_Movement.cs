using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    [SerializeField]float speed = 5f;
    [SerializeField]Transform cam;
    private float gravity = -9.81f;
    private Vector3 playerVelocity;
    [SerializeField]float jump;
    [SerializeField]bool isGrounded;
    [SerializeField]Transform sensorGround;
    [SerializeField]float sensorRadius;
    [SerializeField]LayerMask ground;

    [SerializeField]float forceBox;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponentInChildren<Animator>();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        anim.SetFloat("VelX", x);
        float z = Input.GetAxis("Vertical");
        anim.SetFloat("VelZ", z);
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            Vector3 direcMove = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(direcMove.normalized * speed * Time.deltaTime);
        }

    }

    void Jump()
    {
        isGrounded = Physics.CheckSphere(sensorGround.position, sensorRadius, ground);
        
        
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            anim.SetBool("Jump", false);
        }
        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jump * -2 * gravity); 
            anim.SetBool("Jump", true);

        }


        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Update()
    {
        Movement();
        Jump();
    }
}
