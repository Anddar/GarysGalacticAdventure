using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        // This will change the position of camera to follow our player, without the rotation axis following the player
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 2, transform.position.z);
    }
}
