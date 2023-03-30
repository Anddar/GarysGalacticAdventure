using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUILogicScript : MonoBehaviour
{
    private PlayerDeath playerDeath;
    private int playerHealth;
    private int playerShield;
    private int playerScore;

    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text playerShieldText;
    [SerializeField] private Text playerScoreText;

    // On-Screen Bullet Cycler
    [SerializeField] private GameObject bulletCycler;
    private static Image currentBulletInCycler;

    // Players Status
    private bool playerLivingState;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerBulletCycler.bulletCyclerActive && bulletCycler != null) {
            bulletCycler.SetActive(true);
            currentBulletInCycler = GameObject.FindGameObjectWithTag("CurrentCyclerBullet").GetComponent<Image>();
        }
        
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            playerDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>();
        }
        playerHealth = 100;
        playerShield = 100;
        playerScore = 0;
        playerLivingState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerBulletCycler.bulletCyclerActive && bulletCycler != null) {
            bulletCycler.SetActive(true);
            if (currentBulletInCycler == null) {
                currentBulletInCycler = GameObject.FindGameObjectWithTag("CurrentCyclerBullet").GetComponent<Image>();
            }
        }
    }

    // This function increases the players health by a certain amount
    public void increaseHealth(int amount) {
        if (playerHealth + amount > 100) {
            playerHealth = 100;
        } else {
            playerHealth += amount;
        }
        playerHealthText.text = playerHealth.ToString();
    }

    // This function decreases the players health by a certain amount
    public void decreaseHealth(int amount) {
        if (!playerDeath.isPlayerInvincible()) {
            playerDeath.tookDamage();
            if (playerHealth - amount < 0) {
                playerHealth = 0;
            } else {
                playerHealth -= amount;
            }
            if (playerHealth == 0) {
                playerLivingState = false; // Here the players state is of living is false or "dead"
            }
            playerHealthText.text = playerHealth.ToString();
        }
    }

    // This function increases the players shield by a certain amount
    public void increaseShield(int amount) {
        if (playerShield + amount > 100) {
            playerShield = 100;
        } else {
            playerShield += amount;
        }
        playerShieldText.text = playerShield.ToString();
    }

    // This function decreases the players shield by a certain amount
    public void decreaseShield(int amount) {
        if (!playerDeath.isPlayerInvincible()) {
            if (playerShield - amount < 0) {
                decreaseHealth(amount - playerShield);
                playerShield = 0;
            } else {
                playerDeath.tookDamage();
                playerShield -= amount;
            }
            playerShieldText.text = playerShield.ToString();
        }
    }

    // This function increases the players score by a certain amount
    public void increaseScore(int amount) {
        playerScore += amount;
        playerScoreText.text = playerScore.ToString();
    }

    // This function decreases the players score by a certain amount
    public void decreaseScore(int amount) {
        if (playerScore - amount < 0) {
            playerScore = 0;
        } else {
            playerScore -= amount;
        }
        playerScoreText.text = playerScore.ToString();
    }

    public static void setUIBullet(Sprite bulletSprite) {
        if (currentBulletInCycler != null) {
            currentBulletInCycler.sprite = bulletSprite;
        }
    }

    // This function returns players living state
    public bool isPlayerAlive() {
        return playerLivingState;
    }

    // Returns whether the player has shielding still
    public bool hasShield() {
        return playerShield > 0;
    }

    // Returns the current player score for the level
    public int getPlayerScore() {
        return playerScore;
    }
}
