using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for both weapons and consumable items

public class InventoryItem : MonoBehaviour
{
    // Common properities to all items
    public string displayName;
    public Sprite icon;

   public virtual void Use()
    {
        // This will be implemented in the sub-classes Consumable Item and Weapon Item
    }

    public virtual void RemoveFromInventory()
    {
        InventorySystem inventorySystem = FindObjectOfType<InventorySystem>();
        inventorySystem.RemoveItemFromInventory(this);
    }
}
