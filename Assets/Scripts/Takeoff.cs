using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Takeoff : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxCameraZoomOut = 13;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private float rocketSpeed = 0.2f;
    [SerializeField] private TMP_Text finalScoreTMP;
    
    private Transform rocketTransform;

    private static Animator transition;
    private static bool levelComplete;
    private bool continueRocketLaunch;

    private PlayerUILogicScript gameLogic;

    // Start is called before the first frame update
    void Start()
    {
        rocketTransform = gameObject.transform;

        transition = GameObject.FindGameObjectWithTag("LevelFade").GetComponent<Animator>();
        levelComplete = false;
        continueRocketLaunch = false;

        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<PlayerUILogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (continueRocketLaunch) {
            if (mainCamera.orthographicSize < maxCameraZoomOut) {
                mainCamera.orthographicSize += 0.001f;
            }

            rocketTransform.position = new Vector3(rocketTransform.position.x, rocketTransform.position.y + rocketSpeed, rocketTransform.position.z);
            if (rocketTransform.position.y > 175.0f) {
                StartCoroutine(fadeOut());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            levelComplete = true;

            smoke.Play();
            StartCoroutine(launchRocket());
        }
    }

    private IEnumerator launchRocket() {
        // Building up Smoke
        yield return new WaitForSeconds(4.0f);
        smoke.Stop();

        continueRocketLaunch = true;
        // Starting Main Engine for takeoff
        fire.Play();
    }

    private IEnumerator fadeOut() {
        finalScoreTMP.SetText($"Final Score: {gameLogic.getPlayerScore()}");

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(7.0f);

        SceneManager.LoadScene(0); // Bringing player back to Main Menu
    }

    // Returns if the level is complete or not
    public static bool isLevelComplete() {
        return levelComplete;
    }
}
