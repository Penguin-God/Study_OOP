using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float speed;
    public Camera followCamera;

    public int currentAmmo;
    public int maxAmmo;
    public int currentCoin;
    public int maxCoin;
    public int currentPlayerHp;
    public int maxPlayerHp;
    public int currentGrenade;
    public int maxGrenade;
    public int score;

    public int EquipObjcetIndex = -1; 

    
    public bool isJump; 
    bool isDodje; 
    bool isReload; 
    public bool isMelee; 

    
    private float hAxis;
    private float xAxis;
    bool JumpKey;
    bool GetItemKey;
    bool SwapWeapon1;
    bool SwapWeapon3;
    bool SwapWeapon2;
    bool AttackDown;
    bool ReloadDown;

    Vector3 MoveVec;
    Vector3 DodgeVector;

    Animator animator;
    Dictionary<string, GameObject> weaponDictionary = new Dictionary<string, GameObject>();
    [SerializeField] GameObject Hammer;
    [SerializeField] GameObject Gun1;
    [SerializeField] GameObject Gun2;

    private void Awake()
    {
        weaponDictionary.Add("Hammer", Hammer);
        weaponDictionary.Add("HandGun", Gun1);
        weaponDictionary.Add("SubMachineGun", Gun2);

        animator = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        GetInput();
        Dodge();
        PlayerMove();
        PlayerTurn();
        Jump();
        ActiveWeapon();
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

        //AttackDown = Input.GetButton("Fire1"); 
        //GrenadeDown = Input.GetButtonDown("Fire2");
        //ReloadDown = Input.GetButtonDown("Reload"); 
    }

    public Weapons currentWeapon = null;

    void ActiveWeapon()
    {
        if (SwapWeapon1) this.Hammer.SetActive(true);
        else if (SwapWeapon2) this.Gun1.SetActive(true);
        else if (SwapWeapon3) this.Gun2.SetActive(true);
    }

    public void SetWeapon(Weapons weapon)
    {
        currentWeapon = weapon;
    }

    void Attack()
    {
        if(Input.GetMouseButton(0) && currentWeapon != null && currentWeapon.attackAble)
        {
            currentWeapon.Attack();
        }
    }

    void PlayerMove() 
    {
        if (isDodje) 
            MoveVec = DodgeVector;
        else
            MoveVec = new Vector3(xAxis, 0, hAxis).normalized; 

        if (AttackDown && !isJump && !isDodje) 
            MoveVec = Vector3.zero;

        transform.position += MoveVec * speed * Time.deltaTime;

        //animator.SetBool("IsRun", MoveVec != Vector3.zero); 
    }

    void PlayerTurn() 
    {
        transform.LookAt(transform.position + MoveVec); 

        if (AttackDown && !isDodje && !isJump && !isMelee )
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
        if (JumpKey && !isJump && !isDodje && MoveVec != Vector3.zero && !isReload ) 
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

    void Jump()
    {
        if (JumpKey && !isJump && MoveVec == Vector3.zero) 
        {
            
            GetComponent<Rigidbody>().AddForce(Vector3.up * 13, ForceMode.Impulse); 
            isJump = true;
            animator.SetBool("IsJump", isJump);
            animator.SetTrigger("DoJump"); 
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("�ٴ�"))
        {
            isJump = false;
            animator.SetBool("IsJump", isJump);
        }
    }
}
