using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeGameObject : MonoBehaviour
{
    private SpriteRenderer objectSprite;

    // Start is called before the first frame update
    void Start()
    {
        objectSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Fades object into existence 
    IEnumerator FadeObjectIn(float delayBeforeFade) {
        for (float a = 0.05f; a <= 1; a += 0.05f) {
            objectSprite.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Starts coroutine
    public void startFadingObjectIn(float delayBeforeFade) {
        IEnumerator fadeInCoroutine = FadeObjectIn(delayBeforeFade);
        StartCoroutine(fadeInCoroutine);
    }

    // Fades object out of existence 
    IEnumerator FadeObjectOut(float delayBeforeFade, bool destroyObjectAfterFade) {
        yield return new WaitForSeconds(delayBeforeFade);
        for (float a = 1f; a >= -0.05; a -= 0.05f) {
            objectSprite.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.05f);
        }
        if (destroyObjectAfterFade) {
            Destroy(gameObject);
        }
    } 

    // Starts coroutine
    public void startFadingObjectOut(float delayBeforeFade, bool destroyObjectAfterFade) {
        IEnumerator fadeOutCoroutine = FadeObjectOut(delayBeforeFade, destroyObjectAfterFade);
        StartCoroutine(fadeOutCoroutine);
    }

    // On object injured or hit we will make the object blink white
    IEnumerator BlinkObject() {
        int blinkAmt = 2;
        int blinkCount = 0;
        while (blinkCount < blinkAmt) {
            objectSprite.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.25f);
            objectSprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.05f);
            ++blinkCount;
        }
        yield return new WaitForSeconds(0.05f);
    } 

    // Starts coroutine
    public void startBlinking() {
        IEnumerator blinkObjectCoroutine = BlinkObject();
        StartCoroutine(blinkObjectCoroutine);
    }

}
