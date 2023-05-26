using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    private int damage;
    bool isDamageAvailable = true;
    public float coolDownDuration = 2.0f;

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isDamageAvailable == false)
        {
            return;
        }

        // If object collision is tagged with Enemy then enemy takes damage and put on cooldown
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enter");
            GameObject enemy = collision.gameObject;
            //enemy.GetComponent<EnemyAI>().TakeDamage(damage);
            StartCoroutine(StartCooldown());
        }
    }

    public IEnumerator StartCooldown()
    {
        isDamageAvailable = false;
        yield return new WaitForSeconds(coolDownDuration); // starts a countdown for cooldown
        isDamageAvailable = true;
    }
}
