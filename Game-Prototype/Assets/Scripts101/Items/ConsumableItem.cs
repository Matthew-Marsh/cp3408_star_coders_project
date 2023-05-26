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
        

        Debug.Log("Using Consumable Item.");
        // Perform health restore

        RemoveFromInventory();
    }
}
