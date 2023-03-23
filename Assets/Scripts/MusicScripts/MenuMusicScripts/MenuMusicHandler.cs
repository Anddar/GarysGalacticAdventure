using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicHandler : MonoBehaviour
{
    [SerializeField] private AudioSource main_menu_intro;
    [SerializeField] private AudioSource main_menu_loop;
    [SerializeField] private AudioSource main_menu_overlap_loop;

    private bool overlapStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        startPlayingMenuMusic(main_menu_loop, (float) (main_menu_intro.clip.length - 4.5));
        startPlayingMenuMusic(main_menu_overlap_loop, (float) (main_menu_intro.clip.length + main_menu_loop.clip.length - 8.5));
    }

    // Update is called once per frame
    void Update()
    {
        // Restarts the looping of the loop tracks
        if (main_menu_overlap_loop.isPlaying && !overlapStarted) {
            startPlayingMenuMusic(main_menu_loop, main_menu_overlap_loop.clip.length - 4);
            startPlayingMenuMusic(main_menu_overlap_loop, main_menu_loop.clip.length + main_menu_overlap_loop.clip.length - 8, true);
            overlapStarted = true;
        }

        main_menu_intro.volume = AudioManager.getMusicVolume();
        main_menu_loop.volume = AudioManager.getMusicVolume();
        main_menu_overlap_loop.volume = AudioManager.getMusicVolume();
    }

    // This Coroutine allows us to time up the looping tracks so that they fade in and out the music properly
    private IEnumerator playMenuMusic(AudioSource music, float delayBeforePlaying, bool flipOverlapStarted) {
        yield return new WaitForSeconds(delayBeforePlaying);
        if (flipOverlapStarted) { overlapStarted = !overlapStarted; }
        music.Play();
    }

    private void startPlayingMenuMusic(AudioSource music, float delayBeforePlaying, bool flipOverlapStarted=false) {
        IEnumerator playMenuMusicCoroutine = playMenuMusic(music, delayBeforePlaying, flipOverlapStarted);
        StartCoroutine(playMenuMusicCoroutine);
    }

}
