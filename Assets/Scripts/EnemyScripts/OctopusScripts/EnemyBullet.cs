using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb; // Bullet rigid body
    [SerializeField] float force;
    [SerializeField] int bulletDamage;
    private PlayerUILogicScript gameLogic;
    private PlayerMovement playerMovement;

    private bool bulletInView;

    
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
        if (!bulletInView) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            // Player Knockback Function
            playerMovement.KBCounter = playerMovement.KBTotalTime;
            if (other.transform.position.x <= transform.position.x) {
                playerMovement.KnockFromRight = true;
            } 
            if (other.transform.position.x > transform.position.x) {
                playerMovement.KnockFromRight = false;
            }

            gameLogic.decreaseShield(bulletDamage);
            Destroy(gameObject);
        } else if (other.gameObject.layer != 3 && (other.gameObject.layer == 8 || other.gameObject.layer == 6)) {
            force = 0;
            Destroy(gameObject);
        }
    }

    // These two functions will be automatically called from the camera when the bullet is in view and is not in view.
    void OnBecameVisible() {
        bulletInView = true;
    }

    void OnBecameInvisible()
    {
        bulletInView = false;
    }
}
