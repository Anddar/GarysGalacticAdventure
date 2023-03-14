using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSortingLayer : MonoBehaviour
{
    [SerializeField] private string layerToPushTo;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().sortingLayerName = layerToPushTo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
