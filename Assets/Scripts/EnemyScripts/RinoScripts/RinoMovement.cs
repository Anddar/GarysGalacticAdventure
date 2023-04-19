using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoMovement : MonoBehaviour
{
    // Rino Game Components
    private static Rigidbody2D rinoRigidBody;
    private SpriteRenderer rinoSprite;
    private Animator animator;
    private RinoHealthLogic rinoHealthLogic;

    // Enemy Rino Attributes
    [SerializeField] private float rinoMoveSpeed;
    [SerializeField] private float switchAnimDelay;
    private float switchAnimTimer = 0f;
    private bool isRolling = true;

    // Rino Pathfinding
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
        rinoRigidBody = GetComponent<Rigidbody2D>();
        rinoSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rinoHealthLogic = GetComponent<RinoHealthLogic>();
        previousPosition = transform.position;

        // Waypoints (positions in space) for the slime to follow to walk left and right
        waypoints[0] = new Vector2(transform.position.x, transform.position.y);
        waypoints[1] = new Vector2(waypoints[0].x + walkingPathDistance, waypoints[0].y);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (rinoHealthLogic.isRinoAlive()) {
            if (isChasing) {
                if (transform.position.x > playerTransform.position.x){
                    transform.position += Vector3.left * rinoMoveSpeed * Time.deltaTime;
                }
                if (transform.position.x < playerTransform.position.x){
                    transform.position += Vector3.right * rinoMoveSpeed * Time.deltaTime;
                }

                // Stop chasing after player leaves chase distance
                if (Vector2.Distance(transform.position, playerTransform.position) > chaseDistance){
                    isChasing = false;

                }
            } else { 
                // Start chasing after player is within chase distance
                if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance){
                    isChasing = true;
                }
                FollowWaypoint(); // Start slime traversing to waypoint
            }

            switchAnimTimer += Time.deltaTime;

            UpdateAnimation(); // Update the slimes animation as it moves to the waypoint
        }
    }

    private void UpdateAnimation() {
        if (isMoving()) {
            if (isRolling) {
                animator.SetBool("Jump", false);
                animator.SetBool("Rolling", true);
            } else {
                animator.SetBool("Rolling", false);
                animator.SetBool("Jump", true);
            }
        }

        if (switchAnimTimer > switchAnimDelay) {
            if (isRolling) {
                isRolling = false;
            } else { isRolling = true; }
            switchAnimTimer = 0f;
        }
    }

    // This function makes the Rino follow its waypoint
    private void FollowWaypoint() {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypoint_index], rinoMoveSpeed * Time.deltaTime);

        if (transform.position.x == waypoints[waypoint_index].x) {
            if (waypoint_index == 1) {
                waypoint_index = 0;
            } else { waypoint_index = 1; }
        }
    }

    // Enemy Helper Functions
    private bool isMoving() {
        if (previousPosition.x < transform.position.x && Mathf.Abs(previousPosition.x - transform.position.x) > 0.2) {
            previousPosition = transform.position;
            transform.localScale = new Vector3(1f, 1f, 1f);
            return true;
        } else if (previousPosition.x > transform.position.x && Mathf.Abs(previousPosition.x - transform.position.x) > 0.2) {
            previousPosition = transform.position;
            transform.localScale = new Vector3(-1f, 1f, 1f);
            return true;
        }
        return false;
    }

    public static bool isJumping() {
        if (rinoRigidBody.velocity.y > 0.1f) {
            return true;
        }
        return false;
    }

    // Helper function to determine if player is falling
    public static bool isFalling() {
        if (rinoRigidBody.velocity.y < -0.1f) {
            return true;
        }
        return false;
    }

    public bool isRinoRolling() {
        return isRolling;
    }
}
