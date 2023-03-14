using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoHealthLogic : MonoBehaviour
{
    // Rino Game Components
    private Rigidbody2D rinoRigidBody;
    private Collider2D[] colliders;
    private Animator animator;
    private PlayerUILogicScript gameLogic;

    
    // Rino Health Stats
    [SerializeField] private int numberHitsTillDead;
    private bool rinoLivingState;

    // Rino Death Fading Variables
    private FadeGameObject fader;
    [SerializeField] private float delayBeforeFadedDeath;

    // Droppables
    [SerializeField] private int scoreGivenOnDeath;
    private PlaceDroppables drop;

    // Start is called before the first frame update
    void Start()
    {
        rinoRigidBody = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
        fader = GetComponent<FadeGameObject>();
        
        rinoLivingState = true;

        drop = GetComponent<PlaceDroppables>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die() {
        drop.dropDroppable(transform.position);
        gameLogic.increaseScore(scoreGivenOnDeath);
        fader.startFadingObjectOut(delayBeforeFadedDeath, true);
        animator.SetBool("Rolling", false);
        animator.SetBool("Jump", false);
        // Make rino rigidbody static disable the enemies colliders after they die.
        rinoRigidBody.bodyType = RigidbodyType2D.Static;
        for (int i=0; i < colliders.Length; ++i) {
            colliders[i].enabled = false;
        }
    }

    // Calling this function makes the enemy health take a hit
    public void enemyTakeHit() {
        numberHitsTillDead -= 1;
        if (numberHitsTillDead == 0) {
            rinoLivingState = false;
            Die();
        }
        fader.startBlinking();
    }

    // Returns whether or not the Rino is living
    public bool isRinoAlive() {
        return rinoLivingState;
    }
}
