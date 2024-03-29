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

    private PlayerUILogicScript gameLogic;
    private SpriteRenderer playerSprite;
    private Animator animator;

    private PlayerInputActions playerInputActions;

    [SerializeField] private float shootingDelay;
    private float shootingDelayTimer;
    public static bool isPlayerShooting = false;

    //Gun Sound Player Listener Slot
    [SerializeField] private AudioSource garygunSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
        animator  = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        shootingDelayTimer += Time.deltaTime; // Timer  to track when the player can shoot again
    }

    // Defines player input variables before game starts
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Movement.Enable();
        playerInputActions.Player.Shoot.Enable();
        playerInputActions.Player.Shoot.performed += playerShootProjectile;
    }

    private void onDestroy() {
        playerInputActions.Player.Shoot.performed -= playerShootProjectile;
        playerInputActions.Dispose();
    }

    // Determines if the player is running and flip the player correspondingly to the direction they are running
    public bool isRunning() {
        Vector2 directionVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        float dirX = directionVector.x;
        if (dirX > 0f) {
            return true;
        } else if (dirX < 0f) {
            return true;
        }
        return false;
    }

    // --------------------- Upgrade Increases ---------------------
    public void updateShootingDelay() {
        shootingDelay -= (shootingDelay * gameLogic.getShootSpeedUpgradeIncrease());
    }

    // --------------------- Player Actions After Input ---------------------

    // This function will allow the bullet to spawn and be fired from barrel of gun
    public void playerShootProjectile(InputAction.CallbackContext context) {
        if (gameLogic.isPlayerAlive() && shootingDelayTimer > shootingDelay && playerSprite != null && !PauseMenuController.getPauseStatus() && !LevelCompletionStates.isLevelComplete()) {
            shootingDelayTimer = 0;
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
            GameObject bullet = Instantiate(PlayerBulletCycler.getBulletType(), bulletPosVector, Quaternion.identity);
            garygunSoundEffect.volume = AudioManager.getSoundFXVolume();
            garygunSoundEffect.Play();
            if (flipBullet) { bullet.GetComponent<SpriteRenderer>().flipX = true; }
        }
    }



}
