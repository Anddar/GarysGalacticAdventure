using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoveGary : MenuObjectMovement
{
    [SerializeField] private float delayUntilGaryMovesAcrossScreen;

    private bool movementControl;
    private bool leftScreen;

    // Start is called before the first frame update
    void Start()
    {
        screen = new Rect(0, 0, Screen.width, Screen.height);
        
        objectRigidBody = GetComponent<Rigidbody2D>();
        objectTransform = GetComponent<RectTransform>();

        objectGotIntoView = false;
        movementControl = false;
        leftScreen = false;
        startingPosition = objectTransform.position;

        // Wait time before moving gary across screen
        waitForGaryStart(delayUntilGaryMovesAcrossScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if (movementControl) {
            objectRigidBody.velocity = new Vector2((leftScreen ? -1 : 1) * moveSpeedX, moveSpeedY);
            objectTransform.Rotate(new Vector3(0f, 0f, (leftScreen ? -1 : 1) * objectRotation) * Time.deltaTime);
            if (!isObjectOnScreen()) {
                objectGotIntoView = false;
                startingPosition = objectTransform.position;
                waitForGaryStart(delayUntilGaryMovesAcrossScreen, true);
            }
        }
    }

    // Wait seconds before sending gary character across screen
    private IEnumerator waitForGary(float player_delay, bool switch_side) {
        yield return new WaitForSeconds(player_delay);
        if (switch_side) {
            leftScreen = !leftScreen;
            objectTransform.position = startingPosition;
        } else if (!movementControl) {
            movementControl = true;
        }
    }

    // Starts the above waitForGary Coroutine
    private void waitForGaryStart(float player_delay, bool switch_side = false) {
        IEnumerator gary_wait = waitForGary(player_delay, switch_side);
        StartCoroutine(gary_wait);
    }

}
