using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    public float damage;
   // GameObject weapon;
   /*
    void start()
    {
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        weapon.GetComponent<BoxCollider>().enabled = false;
    }
   */

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enter");
            GameObject enemy = collision.gameObject;
            enemy.GetComponent<PlayerHealthController>().TakeDamage(damage);
        }
    }

    /* Produces null refrence error, code would work better than current solution
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Exit");
            weapon.GetComponent<BoxCollider>().enabled = false;
        }
    }
    */
}
