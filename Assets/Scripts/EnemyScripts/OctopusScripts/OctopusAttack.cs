using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusAttack : MonoBehaviour
{
    // Octopus Game Components
    private OctopusHealthLogic octoHealthLogic;
    private PlayerUILogicScript gameLogic;

    // Octopus Damage Values
    [SerializeField] private int damageFromLaserShot;
    [SerializeField] private int damageFromWalkingInto;

    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        octoHealthLogic = GetComponent<OctopusHealthLogic>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This collision function is for all other purposes of colliding with the enemy including (getting hit, player walking into slime and getting hit, etc...)
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (octoHealthLogic.isOctopusAlive()) {
            if (collidedObject.CompareTag("Bullet")) {
                octoHealthLogic.enemyTakeHit();   
            } else if (collidedObject.CompareTag("Player")) {
                // Player knockback function
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

    // This collision function will help us keep applying damage if the player stays inside the enemy collision box
    private void OnCollisionStay2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (octoHealthLogic.isOctopusAlive()) {
            if (collidedObject.CompareTag("Player")) {
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
}
