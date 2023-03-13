using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    // Slime Game Components
    private SlimeHealthLogic slimeHealthLogic;
    private PlayerUILogicScript gameLogic;
    private Animator animator;

    // Slime Damage Values
    [SerializeField] private int damageFromSpikesAbove;
    [SerializeField] private int damageFromWalkingInto;

    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        slimeHealthLogic = GetComponent<SlimeHealthLogic>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
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
            animator.SetBool("Run", false);
            animator.SetTrigger("Ability");
            gameLogic.decreaseShield(damageFromSpikesAbove);
        }
    }

    // This collision function is for all other purposes of colliding with the enemy including (getting hit, player walking into slime and getting hit, etc...)
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Bullet")) {
            animator.SetBool("Run", false);
            animator.SetTrigger("Hit");
            slimeHealthLogic.enemyTakeHit();
        } else if (collidedObject.CompareTag("Player")) {
            animator.SetBool("Run", false);
            animator.SetTrigger("Attack");
            
            //player knockback function
            playerMovement.KBCounter = playerMovement.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x){
                playerMovement.KnockFromRight = true;
            }if (collision.transform.position.x > transform.position.x){
                playerMovement.KnockFromRight = false;
            }

            gameLogic.decreaseShield(damageFromWalkingInto);
        }
    }

    // This collision function will help us keep applying damage if the player stays inside the enemy collision box
    private void OnCollisionStay2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            animator.SetBool("Run", false);
            animator.SetTrigger("Attack");
            gameLogic.decreaseShield(damageFromWalkingInto);
        }
    }
}
