using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapons
{
    public override void Attack()
    {
        Debug.Log("�𷡹��� ���߻��� : " + transform.name);
    }
}
