using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    private Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSprite;
    private Animator animator;
    private PlayerUILogicScript gameLogic;

    // Fading Variables
    private Color playerColor;
    private bool fadedOut = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameLogic.isPlayerAlive() && !fadedOut) {
            Die(); // If the player is not alive then he dies here
            startFadingPlayerOut();
        }
    }

    // This function is called when the player dies
    private void Die() {
        playerRigidBody.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("death");
    }

    // Fades player into existence 
    IEnumerator FadePlayerIn() {
        for (float a = 0.05f; a <= 1; a += 0.05f) {
            playerSprite.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Starts coroutine
    private void startFadingPlayerIn() {
        fadedOut = false;
        StartCoroutine("FadePlayerIn");
    }

    // Fades player out of existence 
    IEnumerator FadePlayerOut() {
        for (float a = 1f; a >= -0.05; a -= 0.05f) {
            playerSprite.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.05f);
        }
    } 

    // Starts coroutine
    private void startFadingPlayerOut() {
        fadedOut = true;
        StartCoroutine("FadePlayerOut");
    }

}
