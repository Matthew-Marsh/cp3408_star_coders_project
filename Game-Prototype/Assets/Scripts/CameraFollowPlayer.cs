using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform followPlayer;
    public Vector3 playerOffset;
    public float MoveSpeed = 400f;
    private Transform cameraTransform; // How much the camera gets offset from the player 
    GameObject playerObject;
    public float distance = 5f;
    public float height = 3f;
    public float snapThreshold = 0.9f;

    private void Awake()
    {
        cameraTransform = transform;
        // Needs to be done in awake and on start due to character spawn
        if (followPlayer == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                followPlayer = playerObject.transform;
            }
            // moves camera based on offset
            else
            {
                cameraTransform = transform;
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Needs to be done in awake and on start due to character spawn
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            followPlayer = playerObject.transform;
        }
        // moves camera based on offset
        else
        {
            cameraTransform = transform;
        }

    }

    public void SetTarget(Transform newTransformTarget)
    {
        // Sets followplayer to the referenced GameObject in unity editor
        followPlayer = newTransformTarget;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // Makes camera follow the player 
        if (followPlayer != null)
        {
            //cameraTransform.position = Vector3.Lerp(cameraTransform.position, followPlayer.position + playerOffset,
            //                             MoveSpeed * Time.deltaTime);

            Vector3 targetPosition = followPlayer.position - followPlayer.forward * distance + Vector3.up * height;
            Vector3 cameraDirection = cameraTransform.position - followPlayer.position;
            float dotProduct = Vector3.Dot(followPlayer.forward, cameraDirection.normalized);

            if (dotProduct >= snapThreshold)
            {
                cameraTransform.position = targetPosition;
                Quaternion desiredRotation = Quaternion.LookRotation(-followPlayer.forward, Vector3.up);
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, desiredRotation, 10f * Time.deltaTime);
            }
        }
    }
}
