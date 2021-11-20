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

    public void Get_Weapon(ref Weapons weapons, ref Weapons playerCurrentWeapon)
    {
        // 현재 같은 부위의 무기를 들고 있으면 현재 무기는 주운 무기의 위치에 생성됨
        if (weapons != null)
        {
            weapons.gameObject.SetActive(false);
            GameObject currentWeapon = WeaponManager.FiledWeaponDictionary[weapons];
            Instantiate(currentWeapon, transform.position, currentWeapon.transform.rotation);
            WeaponManager.WeaponDictionary[weaponName].gameObject.SetActive(true);

            playerCurrentWeapon = WeaponManager.WeaponDictionary[weaponName];
        }

        // 무기 교체
        weapons = WeaponManager.WeaponDictionary[weaponName];
        gameObject.SetActive(false);
    }
}
