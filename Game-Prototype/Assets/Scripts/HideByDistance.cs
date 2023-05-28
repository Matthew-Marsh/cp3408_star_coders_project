using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add to walls to hide when camera is too close
public class HideByDistance : MonoBehaviour
{

    public float hideDistance = 10f;
    private Renderer wallRenderer;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

        if (distanceToCamera <= hideDistance)
        {
            wallRenderer.enabled = false;
        }
        else
        {
            wallRenderer.enabled = true; 
        }
    }
}
