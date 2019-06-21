using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    //All Boolean values 
    private bool isGrounded;               //Boolean to check if the player is touching the ground
    private bool canJump;                  //Bool to check if the player can jump
    private bool isWallSliding;            // Bool to check if the player is wall sliding 
    private bool isTouchingWall;            //Bool to see if the player is touching the wall
    private bool isLookingRight = true;          //Boolean to check if the player is facing right


    //All Float values 
    public float runSpeed = 10.0f;               //Running speed for the player
    public float dashSpeed = 20.0f;              //Speed for small dashes
    private float dashTime;               //Time the dash lasts 
    public float wallRadius;                //Radius of wall check 
    public float jumpsAmount = 1;           //Setting the amount of jumps the player can make
    private float jumpsLeft;                //Private float to calculate how many jumps are left in the air
    public float wallSlidingSpeed;         //Float value to set wall sliding speed
    public float groundRadius;             //Float to determine the size of the ground check object
    private float startDashTime = 0.1f;            //Setting the startdashtime to a really small amount equal to delta time
    private float movementInput;                 //Float to get the movement 
    public float jumpForce = 10.0f;              //Setting the jump height 

    //All Game Objects 
    public LayerMask isGround;            // Layer mask to determine what is ground 
    public Transform wallCheck;             //Get the transform of the wall check object
    public Transform groundCheck;          //Get the transform of the ground check object
    public Rigidbody2D rb;                       //Assigning the player a rigidbody property 
    public GameObject dashEffect;                //Particle effect for dash




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        //Getting the rigidbody at the start 
        dashTime = startDashTime;                //Set dash time to the start dash time when the application starts
        jumpsLeft = jumpsAmount;                 //Set jumps left to jumps amount at the start 
    }
    void Update()
    {
        getInput();                            //Get the input from the player
        checkDirection();                      //Check the direction of the player
        dash();                                //Check for dash
        checkJumpUpdate();                     //Check if the player can jump
        checkWallSliding();                    //Check if the player is wall sliding
    }
    public void FixedUpdate()
    {
        movementUpdate();                      //Update movement only when there is player input
        checkSurroundings();                   //CHeck for surroundings only when there is player input
        
    }

    private void movementUpdate()                        //Adding the velocity to the player object 
    {
        rb.velocity = new Vector2(movementInput * runSpeed, rb.velocity.y);
        if(isWallSliding && rb.velocity.y < -wallSlidingSpeed)                    //Check if the player is sliding and moving faster than the wall sliding speed
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);                  //Change the y velocity if the player is wall sliding
        }
    }



    private void getInput()                          //Getting the input from the player 
    {
        movementInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();                                //Only jump if the player presses the jump button 
        }
    }



    private void Jump()                             //Adding the jump force to the velocity of the player
    {
        if(canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsLeft--;                                      //Reduce the amount by one each time we jump
        }
        
    }



    private void flip()                            //Function to check if the player chages their facing direction 
    {
        transform.Rotate(0.0f, 180.0f, 0.0f);
        isLookingRight = !isLookingRight;               //Change the bool value if they change direction
    }




    private void checkDirection()                           //Only flip when looking one way but trying to move the other way
    {
        if (isLookingRight && movementInput < 0)                        
        {
            flip();
        }
        else if (!isLookingRight && movementInput > 0)
        {
            flip();
        }
    }




    private void checkWallSliding()
    {
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0)             //Only set wall sliding to true if the player is touching the wall, not on the ground
                                                                           // and is moving downwards 
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }



    private void checkSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, isGround);           //Check to see if the ground check overlaps with ground

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallRadius, isGround);        //Check if the player is touching a wall to their right
    }



    private void dash()
    {
        
        if (dashTime <= 0)
        {
            dashTime = startDashTime;             //Set dashtime to the startDashTime 
            runSpeed = runSpeed * (1/dashSpeed);
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))           //Only dash if the left shift key was pressed 
            {
                dashTime -= Time.time;             //Reduce dashtime to less than zero each time we increase speed
                runSpeed = dashSpeed * runSpeed;
                Instantiate(dashEffect, transform.position, Quaternion.identity);      //Spawn the effect
            }
            
        }

    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);                //Draw the ground check gizmo mesh
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallRadius, wallCheck.position.y, wallCheck.position.z));
    }



    private void checkJumpUpdate()
    {
        if(isGrounded && rb.velocity.y <=0)        //Only let them jump if the player is grounded and not already jumping
        {
            jumpsLeft = jumpsAmount;               //Reset the jumps left each time the player lands on the ground
        }
        else if(jumpsLeft <=0)                     //Set jump bool to false if no jumps left
        {
            canJump = false;
        }
        else
        {
            canJump = true;                        //Set jump bool to true by default
        }
    }
}
