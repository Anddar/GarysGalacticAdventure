using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoAttack : MonoBehaviour
{
    // Slime Game Components
    private RinoMovement rinoMovement;
    private RinoHealthLogic rinoHealthLogic;
    private PlayerUILogicScript gameLogic;
    private EnemyWeakness weaknessObj;
    private Animator animator;

    // Slime Damage Values
    [SerializeField] private int damageFromBiteAttack;
    [SerializeField] private int damageFromWalkingInto;

    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        rinoMovement = GetComponent<RinoMovement>();
        rinoHealthLogic = GetComponent<RinoHealthLogic>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        weaknessObj = gameObject.GetComponentInChildren<EnemyWeakness>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This trigger function will decide when the player is above our enemy telling the enemy to attack
    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            if (rinoMovement.isRinoRolling()) {
                animator.SetBool("Rolling", false);
            } else { animator.SetBool("Jump", false); }
            animator.SetTrigger("Attack");
            gameLogic.decreaseShield(damageFromBiteAttack);
        }
    }

    // This collision function is for all other purposes of colliding with the enemy including (getting hit, player walking into slime and getting hit, etc...)
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.tag.Contains("Bullet")) {
            if (weaknessObj.getWeakness() != "") {
                string weakness = weaknessObj.getWeakness();
                if (collidedObject.tag.Equals("Bullet") && weakness == "Orange") {
                    rinoHealthLogic.enemyTakeHit();
                } else if (collidedObject.tag.Contains("Blue") && weakness == "Blue") {
                    rinoHealthLogic.enemyTakeHit();
                } else if (collidedObject.tag.Contains("Green") && weakness == "Green") {
                    rinoHealthLogic.enemyTakeHit();
                } else if (collidedObject.tag.Contains("Purple") && weakness == "Purple") {
                    rinoHealthLogic.enemyTakeHit();
                }
            } else {
                rinoHealthLogic.enemyTakeHit();
            }
        } else if (collidedObject.CompareTag("Player")) {
            if (rinoMovement.isRinoRolling()) {
                animator.SetBool("Rolling", false);
            } else { animator.SetBool("Jump", false); }
            animator.SetTrigger("Attack");
            
            // Player Knockback Function
            playerMovement.KBCounter = playerMovement.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x) {
                playerMovement.KnockFromRight = true;
            } 
            if (collision.transform.position.x > transform.position.x) {
                playerMovement.KnockFromRight = false;
            }

            gameLogic.decreaseShield(damageFromWalkingInto);
        }
    }

    // This collision function will help us keep applying damage if the player stays inside the enemy collision box
    private void OnCollisionStay2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            if (rinoMovement.isRinoRolling()) {
                animator.SetBool("Rolling", false);
            } else { animator.SetBool("Jump", false); }
            animator.SetTrigger("Attack");

            // Player Knockback Function
            playerMovement.KBCounter = playerMovement.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x) {
                playerMovement.KnockFromRight = true;
            } 
            if (collision.transform.position.x > transform.position.x) {
                playerMovement.KnockFromRight = false;
            }

            gameLogic.decreaseShield(damageFromWalkingInto);
        }
    }
}
