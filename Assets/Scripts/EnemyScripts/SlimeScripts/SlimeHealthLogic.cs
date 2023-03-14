using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHealthLogic : MonoBehaviour
{
    // Slime Game Components
    private Rigidbody2D slimeRigidBody;
    private Collider2D[] colliders;
    private Animator animator;
    private PlayerUILogicScript gameLogic;
    
    // Slime Health Stats
    [SerializeField] private int numberHitsTillDead;
    private bool slimeLivingState;

    // Slime Death Fading Variables
    private FadeGameObject fader;
    [SerializeField] private float delayBeforeFadedDeath;

    // Droppables
    [SerializeField] private int scoreGivenOnDeath;
    private PlaceDroppables drop;

    // Start is called before the first frame update
    void Start()
    {
        slimeRigidBody = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
        fader = GetComponent<FadeGameObject>();
        slimeLivingState = true;

        drop = GetComponent<PlaceDroppables>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die() {
        drop.dropDroppable(transform.position);
        gameLogic.increaseScore(scoreGivenOnDeath);
        animator.SetTrigger("Death");
        fader.startFadingObjectOut(delayBeforeFadedDeath, true);
        // Make slime rigidbody static disable the enemies colliders after they die.
        slimeRigidBody.bodyType = RigidbodyType2D.Static;
        for (int i=0; i < colliders.Length; ++i) {
            colliders[i].enabled = false;
        }
    }

    // Calling this function makes the enemy health take a hit
    public void enemyTakeHit() {
        numberHitsTillDead -= 1;
        if (numberHitsTillDead == 0) {
            slimeLivingState = false;
            Die();
        }
        fader.startBlinking();
    }

    // Returns whether or not the slime is living
    public bool isSlimeAlive() {
        return slimeLivingState;
    }    
    
}
