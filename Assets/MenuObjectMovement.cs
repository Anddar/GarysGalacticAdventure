using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectMovement : MonoBehaviour
{
    // Screen Sizings
    protected Rect screen;
    
    // Object's Game Components
    protected Rigidbody2D objectRigidBody;
    protected RectTransform objectTransform;
    protected bool objectGotIntoView;

    // Object's Stats
    [SerializeField] protected float moveSpeedX;
    [SerializeField] protected float moveSpeedY;
    [SerializeField] protected float objectRotation;
    protected Vector3 startingPosition;

    // Determine whether object is on screen or not 
    protected bool isObjectOnScreen() {
        Vector3[] objectWorldCorners = new Vector3[4];
        objectTransform.GetWorldCorners(objectWorldCorners);

        // Counting the amount of corners not on screen
        int cornersNotOnScreen = 0;
        foreach (Vector3 corner in objectWorldCorners) {
            if (!screen.Contains(corner)) {
                ++cornersNotOnScreen;
            }
        }

        if (!objectGotIntoView && cornersNotOnScreen == 4) {
            return true;
        } else {
            objectGotIntoView = true;
        }

        return cornersNotOnScreen != 4;
    }
}
