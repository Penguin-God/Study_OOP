using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field_Weapon : MonoBehaviour
{
    [SerializeField] string weaponName;
    public void Get_Weapon(ref Weapons[] weapons)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                weapons[i] = WeaponManager.WeaponDictionary[weaponName];
                break;
            }
        }
        gameObject.SetActive(false);
    }
}
