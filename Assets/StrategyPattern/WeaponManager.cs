using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 모든 무기의 정보를 가지고 있는거보다 따로 무기의 정보를 가지고 있는 매니저를 만들면 좋을 것 같다고 생각해서 만든 스크립트
// 라고 포폴에 쓰기

[System.Serializable]
struct WeaponStruct
{ 
    public string weaponName;
    public Weapons weapons;
}

[System.Serializable]
struct FiledWeaponStruct
{
    public Weapons weaponScript;
    public GameObject FiledWeapon;
}

public class WeaponManager : MonoBehaviour
{
    public static Dictionary<string, Weapons> WeaponDictionary = new Dictionary<string, Weapons>();
    public static Dictionary<Weapons, GameObject> FiledWeaponDictionary = new Dictionary<Weapons, GameObject>();

    [SerializeField] WeaponStruct[] arr_WeaponStruct;
    [SerializeField] FiledWeaponStruct[] arr_FiledWeaponStruct;
    void Start()
    {
        foreach (WeaponStruct weaponStruct in arr_WeaponStruct)
            WeaponDictionary.Add(weaponStruct.weaponName, weaponStruct.weapons);
        foreach (FiledWeaponStruct weaponStruct in arr_FiledWeaponStruct)
            FiledWeaponDictionary.Add(weaponStruct.weaponScript, weaponStruct.FiledWeapon);
    }
}
