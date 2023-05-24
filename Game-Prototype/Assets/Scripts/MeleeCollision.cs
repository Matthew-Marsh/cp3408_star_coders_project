using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enter");
            GameObject enemy = collision.gameObject;
            enemy.GetComponent<PlayerHealthController>().TakeDamage(damage);
        }
    }
}
