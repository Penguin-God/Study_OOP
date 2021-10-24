using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapons
{
    public override void Attack()
    {
        StartCoroutine(Co_Shot());
        StartCoroutine(Co_WeaponCollDown());
    }

    IEnumerator Co_Shot()
    {
        ShotBullet();
        yield return null;
        ShotBulletCase();
    }

    [SerializeField] GameObject bullet = null;
    [SerializeField] Transform tf_BulletSpawn = null;
    void ShotBullet()
    {
        // 1. 총알발사, Instantiate(생성할 오브젝트, 생성위치, 오브젝트각도) : 게임오브젝트생성
        GameObject shotBullet = Instantiate(bullet, tf_BulletSpawn.position, Quaternion.identity);
        Rigidbody BulletRigid = shotBullet.GetComponent<Rigidbody>();
        BulletRigid.velocity = tf_BulletSpawn.forward * 50; // forward : Z축  shotBullet_transform부터 z축으로 50의 속도로 총알이 날라가게 함
    }


    [SerializeField] GameObject bulletCase = null;
    [SerializeField] Transform tf_BulletCaseSpawn = null;
    void ShotBulletCase()
    {
        // 2. 탄피배출
        GameObject shot_BulletCase = Instantiate(bulletCase, tf_BulletCaseSpawn.position, Quaternion.identity);
        Rigidbody CaseRigid = shot_BulletCase.GetComponent<Rigidbody>();
        Vector3 caseVec = tf_BulletCaseSpawn.forward * Random.Range(-3f, -1.5f) + Vector3.up * Random.Range(3f, 1.5f); // 탄피가 생성위치에서 얼마나 튈지 설정
        // Z축의 반대쪽에 힘을주기위해 forward에 -값을 곱하고 탄피가 튀는것을 좀 더 느낌있게 보여주기 위해서 Vector.up의 양수값을 더함
        CaseRigid.AddForce(caseVec, ForceMode.Impulse); // 위에서 설정한 백터값만큼 탄피에 힘을 줌
    }
}