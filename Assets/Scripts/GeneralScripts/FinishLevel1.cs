using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel1 : MonoBehaviour
{
    //Sounds for car end flag in first level
    [SerializeField] private AudioSource doorOpen;
    [SerializeField] private AudioSource doorClose;

    private Rigidbody2D playerRigidBody;

    static private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    private List<AudioSource> sounds = new List<AudioSource>();
    private List<float> delayBetweenSounds = new List<float>();
    
    // Start is called before the first frame update
    private void Start()
    {
        playerRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        sounds.Add(doorOpen);
        delayBetweenSounds.Add(2f); // Delay between doorOpen and doorClose
        sounds.Add(doorClose);
        transition = GameObject.FindGameObjectWithTag("LevelFade").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player")){
            startWaitingSound(sounds, delayBetweenSounds);

            playerRigidBody.bodyType = RigidbodyType2D.Static;
        }
    }

    public void CompleteLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }


    private IEnumerator waitForSound(List<AudioSource> sounds, List<float> delayBetweenSounds) {
        int i = 0;
        while (i < sounds.Count) {
            sounds[i].volume = AudioManager.getSoundFXVolume();
            sounds[i].Play();
            yield return new WaitForSeconds(sounds[i].clip.length);
            if (delayBetweenSounds != null && i < delayBetweenSounds.Count) {
                yield return new WaitForSeconds(delayBetweenSounds[i]);
                ++i;
            } else { ++i; }
        }
        Invoke("CompleteLevel", 2f);
    } 

    private IEnumerator LoadLevel(int levelIndex){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    private void startWaitingSound(List<AudioSource> sounds, List<float> delayBetweenSounds=null) {
        IEnumerator waitForSoundCoroutine = waitForSound(sounds, delayBetweenSounds);
        StartCoroutine(waitForSoundCoroutine);
    }
}
