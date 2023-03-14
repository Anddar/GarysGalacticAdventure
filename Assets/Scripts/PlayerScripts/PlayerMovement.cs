using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private static Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSprite;
    private BoxCollider2D collider;
    private PlayerUILogicScript gameLogic;
    private Animator animator;

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    
    private PlayerInputActions playerInputActions;
    private enum AnimationState { idle, running, jumping, falling, crouching, crouchShoot, standShoot, playerDeath }
    private AnimationState state;
    private static bool isCrouched = false;

    // Player knockback vars
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;

    // Sound Listeners
    [SerializeField] private AudioSource footstep1;
    [SerializeField] private AudioSource footstep2;
    [SerializeField] private AudioSource footstep3;
    [SerializeField] private AudioSource footstep4;
    private AudioSource[] footsteps;
    private int footstepIndex = 0;

    [SerializeField] private AudioSource jumpUp;
    [SerializeField] private AudioSource jumpLand;
    private static bool playerInAir;

    // Start is called before the first frame update
    private void Start() {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
        animator = GetComponent<Animator>();

        // Initialize Footstep Audio
        footsteps = new AudioSource[4];
        footsteps[0] = footstep1;
        footsteps[1] = footstep2;
        footsteps[2] = footstep3;
        footsteps[3] = footstep4;

        playerInAir = false;
    
        animator.SetBool("hold_crouch", true); // This is permanent so that the player will stay crouched after shooting in crouched position
    }

    // Update is called once per frame
    private void Update() {
        // Continuously adding the force to keep the player moving in the correct direction we are reading in.
        if (gameLogic.isPlayerAlive()) {
            Vector2 inputDirectionVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
            if (KBCounter <= 0) {
                playerRigidBody.velocity = new Vector2(inputDirectionVector.x * movementSpeed, playerRigidBody.velocity.y);
            } else {
                if (KnockFromRight == true){
                    playerRigidBody.velocity = new Vector2(-KBForce, KBForce);
                }
                if (KnockFromRight == false) {
                    playerRigidBody.velocity = new Vector2(KBForce, KBForce);
                }
                KBCounter -= Time.deltaTime;
            }  

            // Updating Animation States
            updateAnimations();
        }
    }

    // Defines player input variables before game starts
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += player_jump;
        playerInputActions.Player.Crouch.performed += player_crouch;
        playerInputActions.Player.Movement.performed += player_movement;
    }
    
    // Update/Refresh animation states for our player
    private void updateAnimations() {
        // Changes the player animation to start running or stop running 
        if (isRunning()) {
            isCrouched = false;
            flipPlayerColliderSize();
            state = AnimationState.running;
        } else {
            footstepIndex = 0;
            state = AnimationState.idle;
        }

        // Rotating footstep sounds for player
        if (isRunning() && !isJumping() && !isFalling() && !footsteps[footstepIndex].isPlaying) {
            ++footstepIndex;
            if (footstepIndex == footsteps.Length) {
                footstepIndex = 0;
            }
            footsteps[footstepIndex].Play();
        }

        // When player lands we play landing sound
        if (playerInAir && !isFalling() && !isJumping()) {
            playerInAir = false;
            jumpLand.Play();
        }
        
        // Changes the player animation to start crouching
        if (isCrouched && isGrounded()) {
            state = AnimationState.crouching;
        }

        // Changes player animation for when the player is shooting
        if (isCrouched && PlayerShoot.isPlayerShooting) {
            PlayerShoot.isPlayerShooting = false;
            state = AnimationState.crouchShoot;
        } else if (!isJumping() && !isFalling() && PlayerShoot.isPlayerShooting) {
            PlayerShoot.isPlayerShooting = false;
            state = AnimationState.standShoot;
        }

        // Changes the player animation between jumping and falling
        if (isJumping()) {
            isCrouched = false;
            flipPlayerColliderSize();          
            state = AnimationState.jumping;
        } else if (isFalling()) {
            isCrouched = false;
            flipPlayerColliderSize();
            state = AnimationState.falling;
        }

        animator.SetInteger("state", (int) state);
    }

    // Determines if the player is running and flip the player correspondingly to the direction they are running
    public bool isRunning() {
        Vector2 directionVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        float dirX = directionVector.x;
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
    public static bool isJumping() {
        if (playerRigidBody.velocity.y > 0.1f) {
            playerInAir = true;
            return true;
        }
        return false;
    }

    // Helper function to determine if player is falling
    public static bool isFalling() {
        if (playerRigidBody.velocity.y < -0.1f) {
            playerInAir = true;
            return true;
        }
        return false;
    }

    // Helper function to determine if player is standing on the ground 
    public bool isGrounded() {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    // Helper function for other files to determine if the player is crouched
    public static bool getCrouched() {
        return isCrouched;
    }

    // Helper function to change size of player collider based on if he is crouching or not
    private void flipPlayerColliderSize() {
        if (isCrouched) {
            collider.size = new Vector2(collider.size.x, 1.5f);
            collider.offset = new Vector2(collider.offset.x, -0.65f);
        } else {
            collider.size = new Vector2(collider.size.x, 1.97f);
            collider.offset = new Vector2(collider.offset.x, -0.41f);
        }
    }

    // --------------------- Player Actions After Input ---------------------

    // Makes the player jump when pressing jump control, "space" by default
    public void player_jump(InputAction.CallbackContext context) {
        if (gameLogic.isPlayerAlive()) {
            if (isGrounded()) {
                jumpUp.Play();  
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
            }
        }
    }

    // Makes the player crouch when pressing crouch control, "left-control" by default
    public void player_crouch(InputAction.CallbackContext context) {
        if (gameLogic.isPlayerAlive()) {
            if (isGrounded()) {
                if (isCrouched) {
                    isCrouched = false;
                    flipPlayerColliderSize();
                } else {
                    isCrouched = true;
                    flipPlayerColliderSize();
                }
            }
        }
    }

    // Makes the player move (left or right) when pressing movement controls, "a" and "d" by default
    public void player_movement(InputAction.CallbackContext context) {
        if (gameLogic.isPlayerAlive()) {
            Vector2 inputDirectionVector = context.ReadValue<Vector2>();
            playerRigidBody.velocity = new Vector2(inputDirectionVector.x * movementSpeed, playerRigidBody.velocity.y);
        }
    }

}