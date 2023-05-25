using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar; 
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        // Fills the players health bar based on how much health is remaining
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
    }

    void Death()
    {
        if (health == 0)
        {
            // end game code
        }
    }
}
