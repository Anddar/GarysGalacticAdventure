using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform endOfGunBarrelStandingRight;
    [SerializeField] private Transform endOfGunBarrelCrouchRight;
    [SerializeField] private Transform endOfGunBarrelRunningRight;
    [SerializeField] private Transform endOfGunBarrelStandingLeft;
    [SerializeField] private Transform endOfGunBarrelCrouchLeft;
    [SerializeField] private Transform endOfGunBarrelRunningLeft;
    
    [SerializeField] private GameObject bulletPF;

    private SpriteRenderer playerSprite;
    private Animator animator;

    private PlayerInputActions playerInputActions;

    public static bool isPlayerShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        animator  = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Defines player input variables before game starts
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += playerShootProjectile;
    }

    // Determines if the player is running and flip the player correspondingly to the direction they are running
    public bool isRunning() {
        Vector2 directionVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        float dirX = directionVector.x;
        if (dirX > 0f) {
            playerSprite.flipX = false;
            return true;
        } else if (dirX < 0f) {
            playerSprite.flipX = true;
            return true;
        }
        return false;
    }

    // --------------------- Player Actions After Input ---------------------

    // This function will allow the bullet to spawn and be fired from barrel of gun
    public void playerShootProjectile(InputAction.CallbackContext context) {
        Vector3 worldPosition;
        Vector3 bulletPosVector;
        bool flipBullet = false;
        // First determine if the player is crouched, or running then based on the playerSprite we can determine which side to create the bullet on.
        if (PlayerMovement.getCrouched()) {
            if (playerSprite.flipX) {
                worldPosition = endOfGunBarrelCrouchLeft.position;
                flipBullet = true;
            } else {
                worldPosition = endOfGunBarrelCrouchRight.position;
            }
            bulletPosVector = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z); 
        } else if (isRunning()) {
            if (playerSprite.flipX) {
                worldPosition = endOfGunBarrelRunningLeft.position;
                flipBullet = true;
            } else {
                worldPosition = endOfGunBarrelRunningRight.position;
            }
            bulletPosVector = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z); 
        } else {
            if (playerSprite.flipX) {
                worldPosition = endOfGunBarrelStandingLeft.position;
                flipBullet = true;
            } else {
                worldPosition = endOfGunBarrelStandingRight.position;
            }
            bulletPosVector = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z); 
        }

        isPlayerShooting = true; // Allow the player movement file to know the player is shooting

        // Create our bullet in 2D space and flip it if neccessary
        GameObject bullet = Instantiate(bulletPF, bulletPosVector, Quaternion.identity);
        if (flipBullet) { bullet.GetComponent<SpriteRenderer>().flipX = true; }
    }

    

}
