using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class PlayerUILogicScript : MonoBehaviour, IPlayerDataPersistence
{
    private PlayerDeath playerDeath;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private int playerHealth;
    private int playerShield;
    private int playerScore;

    // Player Upgrades 
    private int totalShootSpeedUpgrades;
    private int totalMovementSpeedUpgrades;
    private bool upgradeDataAcquired_ShootSpeed;
    private bool upgradeDataAcquired_MoveSpeed;

    private TMP_Text totalShootSpeedUpgradeText;
    private TMP_Text totalMovementSpeedUpgradeText;
    [SerializeField] private float movementSpeedUpgradePercentIncrease;
    [SerializeField] private float shootSpeedUpgradePercentIncrease;
    [SerializeField] private GameObject movementSpeedUI;
    [SerializeField] private GameObject shootSpeedUI;
    

    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text playerShieldText;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private TMP_Text playerInfoUpdateText;
    private Queue<string> notifyPlayerQueue;
    private bool notificationOnScreen;


    // On-Screen Bullet Cycler
    [SerializeField] private GameObject bulletCycler;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletBlue;
    [SerializeField] private GameObject bulletGreen;
    [SerializeField] private GameObject bulletPurple;
    private static string current_color;
    
    // Players Status
    private bool playerLivingState;

    // Fader Variables
    private FadeGameObject fader;


    // Start is called before the first frame update
    void Start()
    {
        current_color = "Orange";

        if (PlayerBulletCycler.bulletCyclerActive && bulletCycler != null) {
            bulletCycler.SetActive(true);
        }
        
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            playerDeath = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>();
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            playerShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
        }
        playerHealth = 100;
        playerShield = 100;
        playerLivingState = true;
        upgradeDataAcquired_ShootSpeed = false;
        upgradeDataAcquired_MoveSpeed = false;

        notifyPlayerQueue = new Queue<string>();
        notificationOnScreen = false;
        fader = playerInfoUpdateText.gameObject.GetComponent<FadeGameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Turning on the upgrades UI if user acquires an upgrade
        if (shootSpeedUI != null && !shootSpeedUI.activeSelf && totalShootSpeedUpgrades > 0) {
            shootSpeedUI.SetActive(true);
            totalShootSpeedUpgradeText = shootSpeedUI.GetComponentInChildren<TMP_Text>();
            totalShootSpeedUpgradeText.text = totalShootSpeedUpgrades.ToString();
            if (upgradeDataAcquired_ShootSpeed) { playerShoot.updateShootingDelay(); upgradeDataAcquired_ShootSpeed = false; }
        }  
        if (movementSpeedUI != null && !movementSpeedUI.activeSelf && totalMovementSpeedUpgrades > 0) {
            movementSpeedUI.SetActive(true);
            totalMovementSpeedUpgradeText = movementSpeedUI.GetComponentInChildren<TMP_Text>();
            totalMovementSpeedUpgradeText.text = totalMovementSpeedUpgrades.ToString();
            if (upgradeDataAcquired_MoveSpeed) { playerMovement.updateMovementSpeed(); upgradeDataAcquired_MoveSpeed = false; }
        }

        // Notify player with notification if there is still a notification in the queue
        if (notifyPlayerQueue.Count > 0 && !notificationOnScreen) {
            notifyPlayer(notifyPlayerQueue.Dequeue());
        }

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

    // Increase amount of acquired upgrade by 1 using ++ incrementation 
    public void increaseTotalShootSpeedUpgrades() {
        ++totalShootSpeedUpgrades;
        playerShoot.updateShootingDelay();
        if (totalShootSpeedUpgradeText == null) { totalShootSpeedUpgradeText = shootSpeedUI.GetComponentInChildren<TMP_Text>(); } 
        totalShootSpeedUpgradeText.text = totalShootSpeedUpgrades.ToString();

        notifyPlayer("Upgrade: Fire Rate Increased by " + shootSpeedUpgradePercentIncrease*100 + "%\nTotal Increase: " + totalShootSpeedUpgrades*(shootSpeedUpgradePercentIncrease*100) + "%");
    }

    public void increaseTotalMovementSpeedUpgrades() {
        ++totalMovementSpeedUpgrades;
        playerMovement.updateMovementSpeed();
        if (totalMovementSpeedUpgradeText == null) { totalMovementSpeedUpgradeText = movementSpeedUI.GetComponentInChildren<TMP_Text>(); } 
        totalMovementSpeedUpgradeText.text = totalMovementSpeedUpgrades.ToString();

        notifyPlayer("Upgrade: Running Speed Increased by " + movementSpeedUpgradePercentIncrease*100 + "%\nTotal Increase: " + totalMovementSpeedUpgrades*(movementSpeedUpgradePercentIncrease*100) + "%");
    }


    // Getter Methods for the number of acquired upgrades
    public float getShootSpeedUpgradeIncrease() {
        return totalShootSpeedUpgrades * shootSpeedUpgradePercentIncrease;
    }

    public float getMovementSpeedUpgradeIncrease() {
        return totalMovementSpeedUpgrades * movementSpeedUpgradePercentIncrease;
    }

    // Helper function to notify players of an upgrade collected or new item collected for examples
    public void notifyPlayer(String notifyText) {
        if (notificationOnScreen) {
            notifyPlayerQueue.Enqueue(notifyText);
            return;
        }

        notificationOnScreen = true;
        fader.startFadingObjectIn(0f, true);
        playerInfoUpdateText.text = notifyText;
        fader.startFadingObjectOut(4.0f, false, true);
        StartCoroutine(waitForNotification(6f));
    }

    // This enumerator waits for notification to end that is currently on screen
    private IEnumerator waitForNotification(float notificationWaitTime) {
        yield return new WaitForSeconds(notificationWaitTime);
        notificationOnScreen = false;
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
        setScore(playerData.totalScore);
        totalShootSpeedUpgrades = playerData.totalShootSpeedUpgrades;
        totalMovementSpeedUpgrades = playerData.totalMovementSpeedUpgrades;

        if (totalShootSpeedUpgrades > 0) {
            upgradeDataAcquired_ShootSpeed = true;
        } 
        if (totalMovementSpeedUpgrades > 0) {
            upgradeDataAcquired_MoveSpeed = true;
        }
    }

    public void SaveData(ref PlayerData playerData) {
        if (LevelCompletionStates.isLevelComplete() && getPlayerScore() != 0) {
            playerData.totalScore += getPlayerScore();
        }
        if (LevelCompletionStates.isLevelComplete()) {
            playerData.totalShootSpeedUpgrades = totalShootSpeedUpgrades;
            playerData.totalMovementSpeedUpgrades = totalMovementSpeedUpgrades;
        }
    }
}
