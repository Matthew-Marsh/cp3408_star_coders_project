using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[System.Serializable]

public class InventorySystem : MonoBehaviour
{
    //[SerializeField]
    public List<GameObject> inventory;

    // Display and navigate inventory items
    private int currentIndex = 0;
    public TMP_Text inventoryText;
    public Image currentItemIcon;
    public TMP_Text currentDamageRangeText;
    public Image currentEquipItemIcon;
    public TMP_Text numberOfKeysText;
    public TMP_Text levelNumberText;

    // Get game manager to update UI values
    private GameManager gameManager;


    private void Awake()
    {
        inventory = new List<GameObject>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        UpdateEquippedWeaponUI();
        levelNumberText.text = gameManager.GetLevelNumber().ToString();
        numberOfKeysText.text = gameManager.GetNumberOfKeys().ToString();
    }

    public void AddToInventory(GameObject inventoryItem)
    {
        Debug.Log(inventoryItem.ToString() + "Just before add.");
        if (inventoryItem != null)
        {
            inventory.Add(inventoryItem);
            Debug.Log(inventoryItem + "Loot item added to inventory.");
            //lootItem.gameObject.SetActive(false); // Disable in scene
            UpdateInventoryText();
            UpdateItemIcon();
            UpdateEquippedWeaponUI();
        }
    }

    // Call on inventory item subclasses to use item selected
    public void UseItem()
    {
        if (currentIndex >= 0 && currentIndex < inventory.Count)
        {
            GameObject itemToUse = inventory[currentIndex];
            InventoryItem itemComponent = itemToUse.GetComponent<InventoryItem>();
            if (itemComponent != null)
            {
                itemComponent.Use();
                Debug.Log("Loot item being used.");
                UpdateInventoryText();
                UpdateItemIcon();
                UpdateEquippedWeaponUI();
            }
        }
    }

    // Change inventory item
    public void ChangeItem(bool next)
    {
        Debug.Log("Current Index Starts at: " + currentIndex);
        if (inventory.Count > 0)  // Inventory is empty skip change
        {
            if (next)
            {
                currentIndex++;
                if (currentIndex >= inventory.Count)
                    currentIndex = 0;
            }
            else
            {
                currentIndex--;
                if (currentIndex < 0)
                    currentIndex = inventory.Count - 1;
            }
            Debug.Log("Current Index Changed to: " + currentIndex);

        }
        UpdateInventoryText();
        UpdateItemIcon();
        UpdateEquippedWeaponUI();
    }


    // Update display of inventory item in UI
    private void UpdateInventoryText()
    {
        if (inventoryText != null)
        {
            if (inventory.Count > 0 && currentIndex < inventory.Count)
            {
                string itemName = inventory[currentIndex].GetComponent<InventoryItem>().displayName;
                inventoryText.text = itemName;
            }
            else
            {
                inventoryText.text = "Empty";
            }
        }
    }

    private void UpdateItemIcon()
    {
        if (currentItemIcon != null)
        {
            if (inventory.Count > 0 && currentIndex < inventory.Count)
            {
                Sprite itemSprite = inventory[currentIndex].GetComponent<InventoryItem>().icon;
                currentItemIcon.sprite = itemSprite;
            }
            else
            {
                currentItemIcon.sprite = null;
            }
        }
    }

    public void RemoveItemFromInventory(InventoryItem itemToRemove)
    {
        int indexOfItemToRemove = inventory.IndexOf(itemToRemove.gameObject);
        if (indexOfItemToRemove >= 0)
        {
            inventory.RemoveAt(indexOfItemToRemove);
            if (currentIndex >= inventory.Count)
            {
                currentIndex = Mathf.Max(0, inventory.Count - 1);
            }
            Destroy(itemToRemove.gameObject);
            UpdateInventoryText();
            UpdateItemIcon();
        }
    }

    public void EquipWeapon(WeaponItem weapon)
    {
        UnequipWeapon();
        inventory.Remove(weapon.gameObject);
        GameObject equipHand = GameObject.FindGameObjectWithTag("EquipHand");
        if (equipHand != null)
        {
            weapon.transform.SetParent(equipHand.transform, false);  // Set weapon as child of equipHand
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.gameObject.SetActive(true);
        }
        UpdateInventoryText();
        UpdateItemIcon();
        UpdateEquippedWeaponUI();
    }

    public void UnequipWeapon()
    {
        GameObject equipHand = GameObject.FindGameObjectWithTag("EquipHand");
        WeaponItem equippedWeapon = equipHand.GetComponentInChildren<WeaponItem>();
        if (equippedWeapon != null)
        {
            equippedWeapon.transform.SetParent(null, false);
            inventory.Add(equippedWeapon.gameObject);
            equippedWeapon.gameObject.SetActive(false);
            Debug.Log(equippedWeapon.ToString() + " unequipped.");
            UpdateInventoryText();
            UpdateItemIcon();
            UpdateEquippedWeaponUI();
        }
    }

    private void UpdateEquippedWeaponUI()
    {
        WeaponItem equippedWeapon = GetEquippedWeapon();
        if (equippedWeapon != null)
        {
            string damageRange = equippedWeapon.minDamage.ToString() + " - " + equippedWeapon.maxDamage.ToString();
            currentDamageRangeText.text = damageRange;
            currentEquipItemIcon.sprite = equippedWeapon.icon;
        }
        else
        {
            currentDamageRangeText.text = "";
            currentEquipItemIcon.sprite = null;
        }
    }

    public InventoryItem GetCurrentItem()
    {
        if (currentIndex >= 0 && currentIndex < inventory.Count)
        {
            return inventory[currentIndex].GetComponent<InventoryItem>();
        }
        return null;
    }

    public WeaponItem GetEquippedWeapon()
    {
        GameObject equipHand = GameObject.FindGameObjectWithTag("EquipHand");

        if (equipHand != null)
        {
            WeaponItem equippedWeapon = equipHand.GetComponentInChildren<WeaponItem>();
            if (equippedWeapon != null)
            {
                Debug.Log("Equipped Weapon is: " + equippedWeapon.ToString());
                return equippedWeapon;
            }

        }
        return null;
    }
}
