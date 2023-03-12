using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteVertical;
    [SerializeField] private bool infiniteHorizontal;

    private Transform cameraTransform;
    private Vector3 lastCameraPos; 
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    void Start()
    {
        cameraTransform = Camera.main.transform; //main cam pos
        lastCameraPos = cameraTransform.position; //cameras current (or last) pos when finding transform location
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit; //size of background horizontally
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit; //size of background veritcally
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPos = cameraTransform.position;

        //allows for check box for infinite horizontal texture replacment
        if(infiniteHorizontal){
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX){
                float offsetPositionX = (cameraTransform.position.x -transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX , transform.position.y, transform.position.z);
            }
        }

        //allows checkbox for infinite verical texture replacment, not currently used in level 1
        if(infiniteVertical){
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY){
                float offsetPositionY = (cameraTransform.position.y -transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY, transform.position.z);
            }
        }
    }
}
