using UnityEngine;
using System.Collections;

abstract public class Weapons : MonoBehaviour
{
    private Shooter shooter = null;

    private void Start()
    {
        shooter = GetComponentInParent<Shooter>();
    }

    public float attackDelayTime;
    public bool attackAble = true;
    public int damage;

    public abstract void Attack();

    // 어택 기본
    public void BaseAttack()
    {
        AttackAnimation();
        StartCoroutine(Co_WeaponCollDown());
    }

    [SerializeField] string animationName = "";
    public void AttackAnimation()
    {
        shooter.DoAttackAnimation(animationName);
    }

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