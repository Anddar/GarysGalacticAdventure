using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHealthLogic : MonoBehaviour
{
    // Slime Game Components
    private Rigidbody2D slimeRigidBody;
    private BoxCollider2D[] colliders;
    private Animator animator;
    
    // Slime Health Stats
    [SerializeField] private int numberHitsTillDead;
    private bool slimeLivingState;

    // Slime Death Fading Variables
    private FadeGameObject fader;
    [SerializeField] private float delayBeforeFadedDeath;


    // Start is called before the first frame update
    void Start()
    {
        slimeRigidBody = GetComponent<Rigidbody2D>();
        colliders = GetComponents<BoxCollider2D>();
        animator = GetComponent<Animator>();
        fader = GetComponent<FadeGameObject>();
        slimeLivingState = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die() {
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
