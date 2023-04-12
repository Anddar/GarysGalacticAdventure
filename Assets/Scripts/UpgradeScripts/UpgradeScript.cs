using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour
{
    private PlayerUILogicScript gameLogic;

    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the player goes to pick up an upgrade we increase the amount of total upgrades they collected and 
    // destroy the upgrade object
    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            if (gameObject.name.Contains("ShootSpeed")) {
                Destroy(gameObject);
                gameLogic.increaseTotalShootSpeedUpgrades();
            } else if (gameObject.name.Contains("MovementSpeed")) {
                Destroy(gameObject);
                gameLogic.increaseTotalMovementSpeedUpgrades();
            }
        }
    }
}
