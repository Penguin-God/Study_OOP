using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct WeaponStruct
{ 
    public string weaponName;
    public Weapons weapons;
}
 
public class WeaponManager : MonoBehaviour
{
    public static Dictionary<string, Weapons> WeaponDictionary = new Dictionary<string, Weapons>();

    [SerializeField] WeaponStruct[] arr_WeaponStruct;
    void Start()
    {
        foreach (WeaponStruct weaponStruct in arr_WeaponStruct)
            WeaponDictionary.Add(weaponStruct.weaponName, weaponStruct.weapons);
    }
}
