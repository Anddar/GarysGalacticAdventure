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

    // Enemy Slime Attributes
    [SerializeField] private float slimeMoveSpeed;
    [SerializeField] private float randomAttackDelay;
    private float randomAttackTimer = 0f;
    
    // Slime Pathfinding
    private Vector3[] waypoints = new Vector3[2];
    private int waypoint_index = 1;
    [SerializeField] private float walkingPathDistance;

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

            randomAttackTimer += Time.deltaTime;

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
        if (transform.position.x == waypoints[waypoint_index].x) {
            if (waypoint_index == 1) {
                waypoint_index = 0;
            } else { waypoint_index = 1; }
        }
    }

    // This collision function will turn the enemy around if colliding with another enemy or wall
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.layer == 8 || collidedObject.CompareTag("Enemy")) {
            if (waypoint_index == 1) {
                waypoint_index = 0;
            } else { waypoint_index = 1; }
        }
    }

    // Enemy Helper Functions
    private bool isMoving() {
        if (previousPosition.x < transform.position.x) {
            previousPosition = transform.position;
            slimeSprite.flipX = false;
            return true;
        } else if (previousPosition.x > transform.position.x) {
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
}
