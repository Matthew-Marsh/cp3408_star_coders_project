using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to weapons
public class WeaponItem : InventoryItem
{
    public int minDamage = 10;
    public int maxDamage = 20;

    // Equip the weapon
    public override void Use()
    {
        base.Use();
        Debug.Log("Equipping Weapon.");
        InventorySystem inventorySystem = FindObjectOfType<InventorySystem>();

        // Equip this weapon
        inventorySystem.EquipWeapon(this);
    }

   private int GenereateDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }
}
