using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSprite;
    private BoxCollider2D collider;
    private Animator animator;


    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    
    private enum AnimationState { idle, running, jumping, falling }

    // Start is called before the first frame update
    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal"); // Specifies direction user wants to move, 1 will move player to the right and -1 we will move left
        playerRigidBody.velocity = new Vector2(dirX * movementSpeed, playerRigidBody.velocity.y);

        // Allows the for the players input to jump 
        if (Input.GetButtonDown("Jump") && isGrounded()) {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
        }

        // Updating Animation States
        updateAnimations();
    }
    
    // Update/Refresh animation states for our player
    private void updateAnimations() {
        AnimationState state;

        // Changes the player animation to start running or stop running 
        if (isRunning()) {
            state = AnimationState.running;
        } else { 
            state = AnimationState.idle;
        }

        if (isJumping()) {
            state = AnimationState.jumping;
        } else if (isFalling()) {
            state = AnimationState.falling;
        }

        animator.SetInteger("state", (int) state);
    }

    // Determines if the player is running and flip the player correspondingly to the direction they are running
    public bool isRunning() {
        float dirX = Input.GetAxisRaw("Horizontal");

        if (dirX > 0f) {
            playerSprite.flipX = false;
            return true;
        } else if (dirX < 0f) {
            playerSprite.flipX = true;
            return true;
        }

        return false;
    }

    // Helper function to determine if player is jumping
    public bool isJumping() {
        if (playerRigidBody.velocity.y > 0.1f) {
            return true;
        }
        return false;
    }

    // Helper function to determine if player is falling
    public bool isFalling() {
        if (playerRigidBody.velocity.y < -0.1f) {
            return true;
        }
        return false;
    }

    // Helper function to determine if player is standing on the ground 
    public bool isGrounded() {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

}
