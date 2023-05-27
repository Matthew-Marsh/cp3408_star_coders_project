using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float healthRegeneration = 1f;
    private Image healthBar;
    Canvas gamePlayUI;

    void Start()
    {
        maxHealth = health;
        
        // Find health bar in Game Play UI
        gamePlayUI = GameObject.Find("UIGamePlay").GetComponent<Canvas>();
        Transform healthBarTransform = gamePlayUI.transform.Find("HealthBar");
        if (healthBar == null)
        {
            healthBar = healthBarTransform.GetComponentInChildren<Image>(true);
        }
    }

    // Update health bar
    void Update()
    {
        //Debug.Log("Health / MaxHealth: " +  health + " / " + maxHealth);
        
        // Check Health Bar before updating - incase UI GamePlay were deactivated
        if (healthBar == null)
        {
            Debug.Log("Health Bar Image Not Found");
            Transform healthBarTransform = gamePlayUI.transform.Find("HealthBar");
            gamePlayUI = GameObject.Find("UIGamePlay").GetComponent<Canvas>();
            healthBar = healthBarTransform.GetComponentInChildren<Image>(true);
        }
        //Debug.Log(healthBar.ToString());
        
        // Health regeneration over time 
        health += healthRegeneration * Time.deltaTime;

        // Fills the players health bar based on how much health is remaining
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
    }

    // Handle on Death
    void Death()
    {
        if (health <= 0)
        {
            health = 0; // Death UI handled in Player Controller
        }
    }

    public void AddHealth(int healthAmountToAdd)
    {
        if(healthAmountToAdd + health > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += healthAmountToAdd;
        }
    }
}
