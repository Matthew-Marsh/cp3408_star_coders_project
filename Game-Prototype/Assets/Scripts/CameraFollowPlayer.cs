using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform followPlayer;
    public Vector3 playerOffset;
    public float MoveSpeed = 400f;
    GameObject playerObject;
    private Transform cameraTransform; // How much the camera gets offset from the player 


    // Start is called before the first frame update
    private void Start()
    {
        // moves camera based on offset
        playerObject = GameObject.FindGameObjectWithTag("Player");
        followPlayer = playerObject.transform;
        cameraTransform = transform;
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
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, followPlayer.position + playerOffset,
                                         MoveSpeed * Time.deltaTime);
    }
}