using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class PlayerBulletCycler : MonoBehaviour
{

    private PlayerInputActions playerInputActions;

    // Bullet Cycler 
    private static List<GameObject> bullet_cycler = new List<GameObject>();

    // Bullet Types
    [SerializeField] private GameObject regularBullet;
    [SerializeField] private GameObject blueBullet;
    [SerializeField] private GameObject greenBullet;
    [SerializeField] private GameObject purpleBullet;

    // Bullet Cycler Attributes
    public static bool bulletCyclerActive;
    public static int bulletIndex;

    // Start is called before the first frame update
    void Start()
    {
        bulletIndex = 0;

        // If the player is past level 1.2 then 
        if (SceneManager.GetActiveScene().buildIndex > 2) {
            bulletCyclerActive = true;
        }

        // If all of the bullet prefabs exist then we will add them all to the cycler
        if (regularBullet && blueBullet && greenBullet && purpleBullet) {
            bullet_cycler.Add(regularBullet);
            bullet_cycler.Add(blueBullet);
            bullet_cycler.Add(greenBullet);
            bullet_cycler.Add(purpleBullet);
        } else {
            bullet_cycler.Add(regularBullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.BulletCyclerLeft.Enable();
        playerInputActions.Player.BulletCyclerRight.Enable();
        playerInputActions.Player.BulletCyclerLeft.performed += CycleBulletLeft;
        playerInputActions.Player.BulletCyclerRight.performed += CycleBulletRight;
    }

    // Returns the bullet type to the Player Shoot
    public static GameObject getBulletType() {
        // If the bullet cycler is not active then we just return the regular physical bullet to be fired
        if (!bulletCyclerActive) {
            return bullet_cycler[0];
        }

        return bullet_cycler[bulletIndex];
    }

    // This function turns on the bullet cycler, this is used in Level 1.2 to turn on the cycler for the first
    public static void turnOnBulletCycler() {
        bulletCyclerActive = true;
    }

    // Player Input Action to cycler through the bullet cycler
    private void CycleBulletLeft(InputAction.CallbackContext context) {
        if (bulletIndex - 1 < 0) {
            bulletIndex = bullet_cycler.Count - 1;
        } else { --bulletIndex; }
    }

    private void CycleBulletRight(InputAction.CallbackContext context) {
        if (bulletIndex + 1 == bullet_cycler.Count) {
            bulletIndex = 0;
        } else { ++bulletIndex; }
    }

}
