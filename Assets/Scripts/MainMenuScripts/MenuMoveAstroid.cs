using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoveAstroid : MenuObjectMovement
{
    // Start is called before the first frame update
    void Start()
    {
        screen = new Rect(0, 0, Screen.width, Screen.height);
        
        objectRigidBody = GetComponent<Rigidbody2D>();
        objectTransform = GetComponent<RectTransform>();

        objectGotIntoView = false;
        startingPosition = objectTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        objectRigidBody.velocity = new Vector2(moveSpeedX, moveSpeedY);
        objectTransform.Rotate(new Vector3(0f, 0f, objectRotation) * Time.deltaTime);
        if (!isObjectOnScreen()) {
            objectGotIntoView = false;
            objectTransform.position = startingPosition;
        }
    }

}
