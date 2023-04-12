using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerUILogicScript : MonoBehaviour, IPlayerDataPersistence
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
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletBlue;
    [SerializeField] private GameObject bulletGreen;
    [SerializeField] private GameObject bulletPurple;
    private static string current_color;
    
    // Players Status
    private bool playerLivingState;


    // Start is called before the first frame update
    void Start()
    {
        current_color = "Orange";

        if (PlayerBulletCycler.bulletCyclerActive && bulletCycler != null) {
            bulletCycler.SetActive(true);
        }
        
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            playerDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>();
        }
        playerHealth = 100;
        playerShield = 100;
        playerLivingState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerBulletCycler.bulletCyclerActive && bulletCycler != null) {
            bulletCycler.SetActive(true);
        }

        updateBulletUI();


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

    // This function sets the score when the player data is loaded in
    private void setScore(int amount) {
        playerScore = amount;
        if (playerScoreText != null) {
            playerScoreText.text = playerScore.ToString();
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


    
    // UI FUNCTIONS
    
    // Changing the bullet on the players UI
    private void updateBulletUI() {
        if (bullet && bulletBlue && bulletGreen && bulletPurple) {
            switch (current_color) {
                case "Orange":
                    setBulletActiveState(false);
                    bullet.SetActive(true);
                    break;
                case "Blue":
                    setBulletActiveState(false);
                    bulletBlue.SetActive(true);
                    break;
                case "Green":
                    setBulletActiveState(false);
                    bulletGreen.SetActive(true);
                    break;
                case "Purple":
                    setBulletActiveState(false);
                    bulletPurple.SetActive(true);
                    break;
            }
        }
    }

    public static void setUIBullet(string bulletColor) {
        current_color = bulletColor;
    }

    private void setBulletActiveState(bool state) {
        bullet.SetActive(state);
        bulletBlue.SetActive(state);
        bulletGreen.SetActive(state);
        bulletPurple.SetActive(state);
    }

    // Player Data Persistence 
    public void LoadData(PlayerData playerData) {
        Debug.Log("Loading player data: Score is " + playerData.totalScore);
        setScore(playerData.totalScore);
    }

    public void SaveData(ref PlayerData playerData) {
        if (LevelCompletionStates.isLevelComplete() && getPlayerScore() != 0) {
            playerData.totalScore += getPlayerScore();
        }
    }
}
