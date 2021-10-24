using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float speed;
    public Camera followCamera;

    public int currentAmmo;
    public int maxAmmo;
    public int currentGrenade;
    public int maxGrenade;

    bool isDodje; 
    bool isReload;

    private float hAxis;
    private float xAxis;
    bool JumpKey;
    bool SwapWeapon1;
    bool SwapWeapon3;
    bool SwapWeapon2;
    bool AttackDown;
    bool ReloadDown;

    Vector3 MoveVec;
    Vector3 DodgeVector;

    Animator animator;

    Dictionary<Weapons, string> Dic_WeaponAnitionName = new Dictionary<Weapons, string>();
    private void Awake()
    {
        Dic_WeaponAnitionName.Add(Hammer, "DoSwing");
        Dic_WeaponAnitionName.Add(Gun1, "DoShot");
        Dic_WeaponAnitionName.Add(Gun2, "DoMachineGunShot");
        animator = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        SwapWeapon();
        GetInput();
        Dodge();
        PlayerMove();
        PlayerTurn();
        Attack();
    }

    void GetInput() 
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        hAxis = Input.GetAxisRaw("Vertical");
        JumpKey = Input.GetButtonDown("Jump");

        SwapWeapon1 = Input.GetButtonDown("ActiveHammer");
        SwapWeapon2 = Input.GetButtonDown("ActiveGun1");
        SwapWeapon3 = Input.GetButtonDown("ActiveGun2");

        AttackDown = Input.GetMouseButton(0);
    }

    [SerializeField] Weapons Hammer;
    [SerializeField] Weapons Gun1;
    [SerializeField] Weapons Gun2;
    public Weapons currentWeapon = null;
    Weapons GetSwapWeapons()
    {
        if (SwapWeapon1) return Hammer;
        else if (SwapWeapon2) return Gun1;
        else if (SwapWeapon3) return Gun2;
        else return null;
    }

    void SwapWeapon()
    {
        if( (SwapWeapon1 || SwapWeapon2 || SwapWeapon3) && GetSwapWeapons() != currentWeapon )
        {
            if(currentWeapon != null) currentWeapon.gameObject.SetActive(false);
            PutWeaponOn();
        }
    }

    // 무기 바꾸기
    void PutWeaponOn()
    {
        currentWeapon = GetSwapWeapons();
        currentWeapon.gameObject.SetActive(true);
    }

    void Attack()
    {
        if(AttackDown && currentWeapon != null && currentWeapon.attackAble)
        {
            animator.SetTrigger(Dic_WeaponAnitionName[currentWeapon]);
            currentWeapon.Attack();
        }
    }

    void PlayerMove() 
    {
        if (isDodje) MoveVec = DodgeVector;
        else MoveVec = new Vector3(xAxis, 0, hAxis).normalized; 

        if (AttackDown && !isDodje) MoveVec = Vector3.zero;

        transform.position += MoveVec * speed * Time.deltaTime;

        animator.SetBool("IsRun", MoveVec != Vector3.zero); 
    }

    void PlayerTurn() 
    {
        transform.LookAt(transform.position + MoveVec); 

        if (AttackDown && !isDodje)
        {
            Ray CameraRay = followCamera.ScreenPointToRay(Input.mousePosition); 
            RaycastHit rayHit;
            if (Physics.Raycast(CameraRay, out rayHit, 100)) 
            {
                Vector3 nextVec = rayHit.point; 
                nextVec.y = 0; 
                transform.LookAt(nextVec); 
            }
        }
    }

    void Dodge() 
    {
        if (JumpKey && !isDodje && MoveVec != Vector3.zero && !isReload ) 
        {
            DodgeVector = MoveVec;
            speed *= 2;
            animator.SetTrigger("DoDodge");
            isDodje = true;

            Invoke("DodgeOut", 0.5f); 
        }
    }

    void DodgeOut() 
    {
        isDodje = false;
        speed *= 0.5f;
    }
}
