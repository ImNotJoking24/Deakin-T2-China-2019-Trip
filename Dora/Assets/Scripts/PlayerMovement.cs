using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 10.0f;               //Running speed for the player
    private float movementInput;                 //Float to get the movement 
    public float jumpForce = 10.0f;              //Setting the jump height 
    private bool isLookingRight = true;          //Boolean to check if the player is facing right
    public Rigidbody2D rb;                       //Assigning the player a rigidbody property 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        //Getting the rigidbody at the start 
 
    }
    void Update()
    { 
        getInput();                            //Get the input from the player
        checkDirection();
    }
    public void FixedUpdate()
    {
        movementUpdate();                      //Update movement at a fixed rate regardless of frame rate 
    }

    private void movementUpdate()                        //Adding the velocity to the player object 
    {
        rb.velocity = new Vector2(movementInput * runSpeed, rb.velocity.y);
    }
    private void getInput()                          //Getting the input from the player 
    {
        movementInput = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            Jump();                                //Only jump if the player presses the jump button 
        }
    }
    private void Jump()                             //Adding the jump force to the velocity of the player
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void flip()                            //Function to check if the player chages their facing direction 
    { 
        transform.Rotate(0.0f, 180.0f, 0.0f);
        isLookingRight = !isLookingRight;               //Change the bool value if they change direction
    }
    private void checkDirection()
    {
        if (isLookingRight && movementInput < 0)
        {
            flip();
        }
        else if (!isLookingRight && movementInput > 0)
        {
            flip();
        }
        else
        {
            Debug.Log("Doing nothing");
        }
    }
}
