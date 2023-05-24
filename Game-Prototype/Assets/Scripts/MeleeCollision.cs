using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Enter");
    }
}
