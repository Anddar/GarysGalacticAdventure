using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyBullet : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb; //bullets rigid body
    [SerializeField] float force;
    private PlayerUILogicScript gameLogic;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){

            // Player Knockback Function
            playerMovement.KBCounter = playerMovement.KBTotalTime;
            if (other.transform.position.x <= transform.position.x) {
                playerMovement.KnockFromRight = true;
            } 
            if (other.transform.position.x > transform.position.x) {
                playerMovement.KnockFromRight = false;
            }

            gameLogic.decreaseShield(12);
            Destroy(gameObject);
        }
    }
}
