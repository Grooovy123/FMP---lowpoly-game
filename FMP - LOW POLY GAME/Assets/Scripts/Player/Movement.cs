using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController characterController;

    float xMove;
    float zMove;
        
    public float speed = 6f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;
            
    private Vector3 moveDirection;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {        
        xMove = Input.GetAxis("Horizontal") * speed;
        zMove = Input.GetAxis("Vertical") * speed;
        
        PlayerMovement();        
    }

    void PlayerMovement()
    {
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(xMove, 0, zMove);

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }    
}
