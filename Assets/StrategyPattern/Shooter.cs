using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float speed;
    public Camera followCamera;

    public int currentGrenade;
    public int maxGrenade;

    bool isDodje;

    private float hAxis;
    private float xAxis;
    bool JumpKey;
    bool SwapWeapon1;
    bool SwapWeapon2;
    bool AttackDown;
    bool GetWeaponKey;

    Vector3 MoveVec;
    Vector3 DodgeVector;

    Animator animator;

    public void DoAttackAnimation(string animationName)
    {
        animator.SetTrigger(animationName);
    }

    private void Awake()
    {
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

        GetWeaponKey = Input.GetButtonDown("GetWeapon");

        AttackDown = Input.GetMouseButton(0);
    }

    [SerializeField] Weapons weapon_Gun = null; // 총 인벤토리
    [SerializeField] Weapons weapon_Melee = null; // 근접무기 인벤토리

    // 키입력에 따라 무기를 바꿀 인벤토리의 정보를 반환하는 함수
    Weapons GetSwapInventory()
    {
        if (SwapWeapon1) return weapon_Gun;
        else if (SwapWeapon2) return weapon_Melee;
        else return null;
    }

    // 현재 착용 무기
    [SerializeField] Weapons currentWeapon = null;
    // 현재 착용 무기 변경 및 착용 
    void SwapWeapon()
    {
        // 키입력을 받았으며 인벤토리에 무기가 있다면
        if( (SwapWeapon1 || SwapWeapon2) && GetSwapInventory() != currentWeapon )
        { 
            PutWeaponOn();
        }
    }
    // 무기 착용
    void PutWeaponOn()
    {
        // 애니메이션 실행
        animator.SetTrigger("DoSwap");

        // 현재 착용중인 무기가 있으면 무기 숨기기
        if (currentWeapon != null) currentWeapon.gameObject.SetActive(false);
        // 현재 착용 중인 무기 변경
        currentWeapon = GetSwapInventory();
        // 착용 무기 보여주기
        currentWeapon.gameObject.SetActive(true);
    }

    // 공격
    void Attack()
    {
        // 공격 키입력을 받았으며 무기를 착용중이면
        if(AttackDown && currentWeapon != null && currentWeapon.attackAble)
        {
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
        if (JumpKey && !isDodje && MoveVec != Vector3.zero) 
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

    private void OnTriggerStay(Collider other)
    {
        Field_Weapon contactWeapon = other.GetComponent<Field_Weapon>();
        if (contactWeapon != null && GetWeaponKey) GetWeapon_ToInventory(contactWeapon);
    }

    void GetWeapon_ToInventory(Field_Weapon field_Weapon)
    {
        switch (field_Weapon.weaponType)
        {
            case WeaponType.Gun: field_Weapon.Get_Weapon(ref weapon_Gun); break;
            case WeaponType.Melee: field_Weapon.Get_Weapon(ref weapon_Melee); break;
        }
    }
}
