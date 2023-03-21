using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    // Player Game Components
    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerCollider;
    private SpriteRenderer playerSprite;
    private Animator animator;
    private PlayerUILogicScript gameLogic;
    private FadeGameObject fader;

    // Fading Variables
    private Color playerColor;
    private bool fadedOut;
    [SerializeField] private float delayBeforeFadedDeath;

    // Invincibility Frame Variables
    [SerializeField] private float invincibilityTimeLength;
    private float invincibilityTimer;
    private bool isInvincible;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();

        // Fader Tools
        fader = GetComponent<FadeGameObject>();
        fadedOut = false;

    }

    // Update is called once per frame
    void Update()
    {
        // Sets the player to stop being invincible after the invincibility time frame is up
        if (invincibilityTimer > invincibilityTimeLength) {
            isInvincible = false;
        }
        invincibilityTimer += Time.deltaTime;

        if (!gameLogic.isPlayerAlive() && !fadedOut) {
            Die(); // If the player is not alive then he dies here

            // Fade player out
            fader.startFadingObjectOut(delayBeforeFadedDeath, false);
            flipFadeOutState();

            // Turn off sprite renderer and colliders so enemies wont get stuck on invisible player after he is dead.
            playerCollider.enabled = false;
        }
    }

    // This function is called when the player dies
    private void Die() {
        playerRigidBody.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("death");
    }

    // Restarts level with animation event caller
    private void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // If the player collides with any type of other object (enemy, etc..)
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidedObject = collision.gameObject;
        //if (collidedObject.CompareTag("Enemy")) {}
    }

    // If a player took damage an alternative way that could not be detected in the collision box, we can make the player blink. We also make sure the player is invincible for a
    // period of time.
    public void tookDamage() {
        if (invincibilityTimer > invincibilityTimeLength) {
            isInvincible = true;
            invincibilityTimer = 0f;
            fader.startBlinking();
        }
    }

    // Returns whether the player is invincible or not
    public bool isPlayerInvincible() {
        return isInvincible;
    }

    // A switch to control whether the player is faded out or not
    public void flipFadeOutState() {
        if (fadedOut) {
            fadedOut = false;
        } else {
            fadedOut = true;
        }
    }

    // Returns players faded out status
    public bool isPlayerFadedOut() {
        return fadedOut;
    }


}
