using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Rigidbody2D bulletRigidBody;
    private SpriteRenderer bulletSprite;
    private Animator animator;
    private bool bulletInView;
    
    [SerializeField] private float bulletSpeed;

    // Start is called before the first frame update
    void Start() {
        bulletRigidBody = GetComponent<Rigidbody2D>();
        bulletSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        bulletInView = true;

        // Starts moving the bullet as soon as it is instantiated at the barrel
        bulletRigidBody.velocity = new Vector2(bulletDirection() * bulletSpeed, bulletRigidBody.velocity.y);
    }

    // Update is called once per frame
    void Update() {
        bulletRigidBody.velocity = new Vector2(bulletDirection() * bulletSpeed, bulletRigidBody.velocity.y);

        // Deletes bullet if it goes off screen
        if (!bulletInView) {
            Destroy(gameObject, 1f);
        }
    }

    // Helper function to determine which direction the bullet is traveling in
    public int bulletDirection() {
        if (bulletSprite.flipX) {
            return -1;
        }
        return 1;
    }

    // These two functions will be automatically called from the camera when the bullet is in view and is not in view.
    void OnBecameVisible() {
        bulletInView = true;
    }

    void OnBecameInvisible()
    {
        bulletInView = false;
    }

    // This trigger function will decide when a bullet hits another object in the game world (terrain, walls, etc..)
    private void OnTriggerEnter2D(Collider2D collision) {
        // If the bullet collides with a wall the animation should trigger, and we should destroy the bullet object since it hits a wall
        if (collision.gameObject.layer != 3 && (collision.gameObject.layer == 6)) {
            animator.SetBool("wall_collision", true);
            bulletSpeed = 0;
            Destroy(gameObject, 0.25f);
        }
    }  
}
