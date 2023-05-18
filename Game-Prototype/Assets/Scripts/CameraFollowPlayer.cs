using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform followPlayer;
    public Vector3 playerOffset;
    public float MoveSpeed = 400f;
    private Transform cameraTransform;


    // Start is called before the first frame update
    private void Start()
    {
        cameraTransform = transform;
    }

    public void SetTarget(Transform newTransformTarget)
    {
        followPlayer = newTransformTarget;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (followPlayer != null)
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, followPlayer.position + playerOffset,
                                         MoveSpeed * Time.deltaTime);
    }
}
