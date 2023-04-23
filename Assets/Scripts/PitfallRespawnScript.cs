using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitfallRespawnScript : MonoBehaviour
{
    [SerializeField] private Transform respawnLocation;
    [SerializeField] private int damageToPlayer; // Damage to the player for falling in the pit

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

    // This trigger function will determine when the player has fallen into the pitfall 
    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            collidedObject.transform.position = respawnLocation.position;
            gameLogic.decreaseShield(damageToPlayer);
        } else if (collidedObject.CompareTag("Enemy")) {
            Destroy(collidedObject, 2.0f);
        }
    }

}
