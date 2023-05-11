using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoUI : MonoBehaviour
{
    public static WeaponInfoUI Instance { get; private set; }

    public Text WeaponName;

    void OnEnable()
    {
        Instance = this;
    }

    public void UpdateWeaponName(Weapon weapon)
    {
        WeaponName.text = weapon.name;
    }
}
