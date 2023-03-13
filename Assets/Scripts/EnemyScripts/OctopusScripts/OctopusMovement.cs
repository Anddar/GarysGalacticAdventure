using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusMovement : MonoBehaviour
{
    // Octopus Game Components
    private static Rigidbody2D octoRigidBody;
    private SpriteRenderer octoSprite;
    private Animator animator;
    private OctopusHealthLogic octoHealthLogic;

    // Enemy Octopus Attributes
    [SerializeField] private float octopusMoveSpeed;
    
    // Octopus Pathfinding
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
        octoRigidBody = GetComponent<Rigidbody2D>();
        octoSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        octoHealthLogic = GetComponent<OctopusHealthLogic>();
        previousPosition = transform.position;

        // Waypoints (positions in space) for the slime to follow to walk left and right
        waypoints[0] = new Vector2(transform.position.x, transform.position.y);
        waypoints[1] = new Vector2(waypoints[0].x + walkingPathDistance, waypoints[0].y);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (octoHealthLogic.isOctopusAlive()) {
            if (isChasing) {
                if (transform.position.x > playerTransform.position.x) {
                    transform.position += Vector3.left * octopusMoveSpeed * Time.deltaTime;
                }
                if (transform.position.x < playerTransform.position.x) {
                    transform.position += Vector3.right * octopusMoveSpeed * Time.deltaTime;
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

            UpdateAnimation(); // Update the slimes animation as it moves to the waypoint
        }
    }

    private void UpdateAnimation() {
        isMoving();
    }

    // This function makes the Octopus follow its waypoint
    private void FollowWaypoint() {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypoint_index], octopusMoveSpeed * Time.deltaTime);

        if (transform.position == waypoints[waypoint_index]) {
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
            transform.localScale = new Vector3(-1f, 1f, 1f);
            return true;
        } else if (previousPosition.x > transform.position.x) {
            previousPosition = transform.position;
            transform.localScale = new Vector3(1f, 1f, 1f);
            return true;
        }
        return false;
    }

    // Helper function to determine if player is falling
    public static bool isFalling() {
        if (octoRigidBody.velocity.y < -0.1f) {
            return true;
        }
        return false;
    }

}
