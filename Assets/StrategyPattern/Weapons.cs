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

    // 이거 이름 바꾸고 애니메이션 등 공통적인 부분을 다루는 새로운 Attack 함수 만들고 그거 사용하기 확장하기
    public abstract void Attack();

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
