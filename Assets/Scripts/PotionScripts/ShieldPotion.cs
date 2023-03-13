using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPotion : MonoBehaviour
{
    // Potion Game Components
    private SpriteRenderer potionSprite;
    private BoxCollider2D collider;

    private PlayerUILogicScript gameLogic;
    [SerializeField] private AudioSource drinkPotion;

    [SerializeField] private int potHealAmount;

    // Start is called before the first frame update
    void Start()
    {
        potionSprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Adds shield to player and destroy pot
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            drinkPotion.Play();
            gameLogic.increaseHealth(potHealAmount);

            potionSprite.enabled = false;
            collider.enabled = false;

            Destroy(gameObject, drinkPotion.clip.length);
        }
    }
}
