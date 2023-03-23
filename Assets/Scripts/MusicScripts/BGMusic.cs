using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [SerializeField] private AudioSource introSoundEffect;
    [SerializeField] private AudioSource loopMidSoundEffect;

    private bool loopMidStarted;

    // Start is called before the first frame update
    void Start() {
        startWaitingForSong(introSoundEffect.clip.length);
        loopMidStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!introSoundEffect.isPlaying && !loopMidStarted) {
            loopMidSoundEffect.Play();
            loopMidStarted = true;
        }

        // Updating Music Volumes from AudioManager
        introSoundEffect.volume = AudioManager.getMusicVolume();
        loopMidSoundEffect.volume = AudioManager.getMusicVolume();
    }

    private IEnumerator waitForSong(float songLength) {
        yield return new WaitForSeconds(songLength);
    } 

    private void startWaitingForSong(float songLength) {
        IEnumerator waitForSongCoroutine = waitForSong(songLength);
        StartCoroutine(waitForSongCoroutine);
    }
}
