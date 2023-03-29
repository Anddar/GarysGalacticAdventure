using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBulletCycler : MonoBehaviour
{
    // Bullet Cycler Sprite Render Object
    private SpriteRenderer bulletCyclerRenderer;

    // All Bullet Type Sprites to Rotate Through
    private List<Sprite> bulletSprites = new List<Sprite>();
    [SerializeField] private Sprite regularBullet;
    [SerializeField] private Sprite blueBullet;
    [SerializeField] private Sprite greenBullet;
    [SerializeField] private Sprite purpleBullet;

    // Attributes to Rotate through bullet types
    private int bulletSpriteIndex;
    [SerializeField] private float delayTime;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        bulletCyclerRenderer = GetComponent<SpriteRenderer>();

        bulletSprites.Add(regularBullet);
        bulletSprites.Add(blueBullet);
        bulletSprites.Add(greenBullet);
        bulletSprites.Add(purpleBullet);

        bulletSpriteIndex = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > delayTime) {
            ++bulletSpriteIndex;
            if (bulletSpriteIndex == bulletSprites.Count) {
                bulletSpriteIndex = 0;
            }

            // Changing bullet sprite
            bulletCyclerRenderer.sprite = bulletSprites[bulletSpriteIndex];
            
            timer = 0;
        } else { timer += Time.deltaTime; }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            PlayerBulletCycler.turnOnBulletCycler();
            Destroy(gameObject);
        }
    }
}
