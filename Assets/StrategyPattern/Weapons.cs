using UnityEngine;
using System.Collections;

abstract public class Weapons : MonoBehaviour
{
    public float attackDelayTime;
    public bool attackAble = true;
    public int damage;

    public abstract void Attack();

    public IEnumerator Co_WeaponCollDown()
    {
        attackAble = false;
        yield return new WaitForSeconds(attackDelayTime);
        attackAble = true;
    }

    private void OnDisable()
    {
        attackAble = true;
    }
}
