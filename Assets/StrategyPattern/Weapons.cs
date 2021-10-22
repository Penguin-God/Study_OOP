using UnityEngine;

abstract public class Weapons : MonoBehaviour
{
    [SerializeField] Shooter shooter = null;

    public string weaponName;
    public float attackDelayTime ;
    public bool attackAble = true;
    public int damage;

    public abstract void Attack();

    private void OnEnable()
    {
        shooter.SetWeapon(this);
    }
}
