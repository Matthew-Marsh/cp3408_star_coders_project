using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to consumable items
public class ConsumableItem : InventoryItem
{
    public int healthRestoreAmount;

    // Consume the item
    public override void Use()
    {
        base.Use();

        Debug.Log("Using Consumable Item: " + healthRestoreAmount);
        //PlayerHealthController playerHealthController = GetComponent<PlayerHealthController>();
        PlayerHealthController playerHealthController = FindObjectOfType<PlayerHealthController>();
        Debug.Log("Player Health Controller: " + playerHealthController.ToString());
        playerHealthController.AddHealth(healthRestoreAmount);

        RemoveFromInventory();  // It will be destroyed by calling this
    }
}
