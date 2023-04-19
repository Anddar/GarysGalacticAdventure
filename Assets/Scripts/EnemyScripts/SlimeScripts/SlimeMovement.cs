using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    // Slime Game Components
    private static Rigidbody2D slimeRigidBody;
    private SpriteRenderer slimeSprite;
    private Animator animator;
    private SlimeHealthLogic slimeHealthLogic;
    private Collider2D[] colliders;

    // Enemy Slime Attributes
    [SerializeField] private float slimeMoveSpeed;
    [SerializeField] private float randomAttackDelay;
    private float randomAttackTimer = 0f;
    
    // Slime Pathfinding
    private Vector3[] waypoints = new Vector3[2];
    private int waypoint_index = 1;
    private int numberOfJumpsTried = 0;
    [SerializeField] private float walkingPathDistance;
    [SerializeField] private bool jumpOnCollision;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask jumpableGround;
    

    // Previous Position
    private Vector3 previousPosition;

    // Chase Vars
    private Transform playerTransform;
    [SerializeField] private bool isChasing;
    [SerializeField] private float chaseDistance;

    // Start is called before the first frame update
    void Start()
    {
        slimeRigidBody = GetComponent<Rigidbody2D>();
        slimeSprite = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();
        slimeHealthLogic = GetComponent<SlimeHealthLogic>();
        previousPosition = transform.position;

        // Waypoints (positions in space) for the slime to follow to walk left and right
        waypoints[0] = new Vector2(transform.position.x, transform.position.y);
        waypoints[1] = new Vector2(waypoints[0].x + walkingPathDistance, waypoints[0].y);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (slimeHealthLogic.isSlimeAlive()) {
            if (isChasing) {
                if (transform.position.x > playerTransform.position.x) {
                    transform.position += Vector3.left * slimeMoveSpeed * Time.deltaTime;
                }
                if (transform.position.x < playerTransform.position.x) {
                    transform.position += Vector3.right * slimeMoveSpeed * Time.deltaTime;
                }

                // Stop chasing after player leaves chase distance
                if (Vector2.Distance(transform.position, playerTransform.position) > chaseDistance){
                    isChasing = false;
                    waypoints[0] = new Vector2(transform.position.x, transform.position.y);
                    waypoints[1] = new Vector2(waypoints[0].x + walkingPathDistance, waypoints[0].y);
                }
            } else { 
                // Start chasing after player is within chase distance
                if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance){
                    isChasing = true;
                }
                FollowWaypoint(); // Start slime traversing to waypoint
            }

            if (!jumpOnCollision) { randomAttackTimer += Time.deltaTime; }

            UpdateAnimation(); // Update the slimes animation as it moves to the waypoint
        }
    }

    private void UpdateAnimation() {
        if (isMoving()) {
            animator.SetBool("Run", true);
        }

        if (randomAttackTimer > randomAttackDelay) {
            animator.SetBool("Run", false); // Stop enemy from running
            animator.SetTrigger("Attack"); // Do random attack
            randomAttackTimer = 0f;
        }
    }

    // This function makes the Slime follow its waypoint
    private void FollowWaypoint() {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypoint_index], slimeMoveSpeed * Time.deltaTime);
        if (Mathf.Abs(transform.position.x - waypoints[waypoint_index].x) < 0.2) {
            if (waypoint_index == 1) {
                waypoint_index = 0;
            } else { waypoint_index = 1; }
        }
    }

    // This collision function will turn the enemy around if colliding with another enemy or wall
    private void OnCollisionEnter2D(Collision2D collision) {
        numberOfJumpsTried = 0;

        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Enemy")) {
            if (waypoint_index == 1) {
                waypoint_index = 0;
            } else { waypoint_index = 1; }
            return;
        } 

        if (collidedObject.layer == 8 && !jumpOnCollision) {
            if (waypoint_index == 1) {
                waypoint_index = 0;
            } else { waypoint_index = 1; }
        } else if (collidedObject.layer == 8 && jumpOnCollision && isGrounded() && slimeCollisionContact(collision)) {
            slimeRigidBody.velocity = new Vector2(slimeRigidBody.velocity.x, jumpForce);
            animator.SetBool("Run", false); // Stop enemy from running
            animator.SetTrigger("Attack"); // Do random attack
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.layer == 8 && !jumpOnCollision) {
            if (waypoint_index == 1) {
                waypoint_index = 0;
            } else { waypoint_index = 1; }
        } else if (collidedObject.layer == 8 && jumpOnCollision && isGrounded() && slimeCollisionContact(collision)) {
            // If the slime has tried to jump 4 times and still not gotten over it then we will have the slime walk in the other direction
            if (numberOfJumpsTried == 4) {
                if (waypoint_index == 1) {
                    waypoint_index = 0;
                } else { waypoint_index = 1; }
                return;
            }
            slimeRigidBody.velocity = new Vector2(slimeRigidBody.velocity.x, jumpForce);
            animator.SetBool("Run", false); // Stop enemy from running
            animator.SetTrigger("Attack"); // Do random attack
            numberOfJumpsTried += 1;
        }
    }

    // Enemy Helper Functions
    private bool isMoving() {
        if (previousPosition.x < transform.position.x && Mathf.Abs(previousPosition.x - transform.position.x) > 0.2) {
            previousPosition = transform.position;
            slimeSprite.flipX = false;
            return true;
        } else if (previousPosition.x > transform.position.x && Mathf.Abs(previousPosition.x - transform.position.x) > 0.2) {
            previousPosition = transform.position;
            slimeSprite.flipX = true;
            return true;
        }
        return false;
    }

    private int enemyDirection() {
        if (slimeSprite.flipX) {
            return -1;
        }
        return 1;
    }

    public static bool isJumping() {
        if (slimeRigidBody.velocity.y > 0.1f) {
            return true;
        }
        return false;
    }

    // Helper function to determine if player is falling
    public static bool isFalling() {
        if (slimeRigidBody.velocity.y < -0.1f) {
            return true;
        }
        return false;
    }

    // Helper function to determine if slime is on the ground 
    public bool isGrounded() {
        foreach (Collider2D collider in colliders) {
            if (!collider.isTrigger) {
                return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
            }
        }
        return false;
    }

    // Helper function to return slimes collision direction, meaning the side the slime collided on, we want to make sure 
    // that it is from the left or right as that is the only time we want a slime to jump is if the collision in on the left or right
    private bool slimeCollisionContact(Collision2D collision) {
        Vector3 collision_contact_point = collision.contacts[0].normal;
        float angle = Vector3.Angle(collision_contact_point, Vector3.up);
        if (Mathf.Approximately(angle, 90)) {
            Vector3 result_vector = Vector3.Cross(Vector3.forward, collision_contact_point);
            if (result_vector.y > 0) {
                return true;
            } else {
                return true;
            }
        }
        return false;
    }
}
