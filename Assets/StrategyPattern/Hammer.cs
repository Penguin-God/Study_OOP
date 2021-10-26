using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapons
{
    public override void Attack()
    {
        StartCoroutine(Co_WeaponCollDown());
        AttackAnimation();
        Debug.Log("망치 퍽퍽");
    }
}
