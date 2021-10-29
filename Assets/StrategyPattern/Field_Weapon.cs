using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee,
    Gun,
}

public class Field_Weapon : MonoBehaviour
{
    [SerializeField] string weaponName;
    public WeaponType weaponType;

    public void Get_Weapon(ref Weapons weapons)
    {
        weapons = WeaponManager.WeaponDictionary[weaponName];
        gameObject.SetActive(false);
    }
}
