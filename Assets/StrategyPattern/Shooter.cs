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
    bool SwapWeapon3;
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
        SwapWeapon3 = Input.GetButtonDown("ActiveGun2");
        GetWeaponKey = Input.GetButtonDown("GetWeapon");

        AttackDown = Input.GetMouseButton(0);
    }

    // 주워서 칸으로 사용하기
    [SerializeField] Weapons[] arr_Item_Inventory = new Weapons[3];
    //[SerializeField] Weapons Item_1;
    //[SerializeField] Weapons Item_2;
    //[SerializeField] Weapons Item_3;

    [SerializeField] Weapons currentWeapon = null;
    Weapons GetSwapWeapons()
    {
        if (SwapWeapon1) return arr_Item_Inventory[0];
        else if (SwapWeapon2) return arr_Item_Inventory[1];
        else if (SwapWeapon3) return arr_Item_Inventory[2];
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
        animator.SetTrigger("DoSwap");
        currentWeapon = GetSwapWeapons();
        currentWeapon.gameObject.SetActive(true);
    }

    void Attack()
    {
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
        if (contactWeapon != null && GetWeaponKey) contactWeapon.Get_Weapon(ref arr_Item_Inventory);
    }
}
