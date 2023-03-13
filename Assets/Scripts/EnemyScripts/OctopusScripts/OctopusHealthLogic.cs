using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusHealthLogic : MonoBehaviour
{
    // Slime Game Components
    private Rigidbody2D octoRigidBody;
    private PolygonCollider2D collider;
    [SerializeField] private LayerMask ground;
    
    // Slime Health Stats
    [SerializeField] private int numberHitsTillDead;
    private bool octoLivingState;

    // Slime Death Fading Variables
    private FadeGameObject fader;
    [SerializeField] private float delayBeforeFadedDeath;
    [SerializeField] private float gravityOnDeath;

    // Start is called before the first frame update
    void Start()
    {
        octoRigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<PolygonCollider2D>();
        fader = GetComponent<FadeGameObject>();
        octoLivingState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!octoLivingState && isGrounded()) {
            octoRigidBody.bodyType = RigidbodyType2D.Static;
            collider.enabled = false;
        }
    }

    private void Die() {
        fader.startFadingObjectOut(delayBeforeFadedDeath, true);
        // Make slime rigidbody static disable the enemies colliders after they die.
        octoRigidBody.gravityScale = gravityOnDeath;
    }

    // Calling this function makes the enemy health take a hit
    public void enemyTakeHit() {
        numberHitsTillDead -= 1;
        if (numberHitsTillDead == 0) {
            octoLivingState = false;
            Die();
        }
        fader.startBlinking();
    }

    // Returns whether or not the octopus is living
    public bool isOctopusAlive() {
        return octoLivingState;
    }

    // Helper function to determine if octupus is on the ground 
    public bool isGrounded() {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, ground);
    }
}
