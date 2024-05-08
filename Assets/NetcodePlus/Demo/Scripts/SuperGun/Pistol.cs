using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapons
{
    private int gunFacing;

    [Header("Bullet Prefab")]
    public GameObject redBullet;
    public GameObject greenBullet;

    [Header("Bullet Count")]
    public int weaponCount;

    [Header("Inite Bullet Parameter")]
    public Transform firePoint;
    public Transform bulletPool;

    public float redBulletSpeed;
    public float greenBulletSpeed;

    public float redBulletDamage;
    public float greenBulletDamage;

    public float redBulletImpactForce;
    public float greenBulletImpactForce;

    [Header("Recoil Force")]
    public float recoil;

    [Header("Player Id")]
    public int playerId;

    [Header("Skill Continue")]
    public float skillContinueTime;
    public bool onSkill;

    public SuperGunPlayer gunPlayer;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        bulletPool = GameObject.Find("BulletsPool").transform;
        gunPlayer = transform.parent.parent.parent.GetComponent<SuperGunPlayer>();
    }
    public override void FisrtFunc()
    {
        if(onSkill)
        {
            ShootGreenBullet();
            return;
        }
        ShootRedBullet();
    }

    public override void SecondFunc()
    {
        onSkill = true;
        Invoke("ShutDownSkillTime", skillContinueTime);
    }
    
    void ShutDownSkillTime()
    {
        onSkill = false;
    }

    void ShootRedBullet()
    {
        GameObject bullet = Instantiate(redBullet, firePoint.position ,firePoint.rotation, bulletPool);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = GetBulletDir() * redBulletSpeed + new Vector3(gunPlayer.rigid.velocity.x,0,0);

        GunBullet _bullet = bullet.GetComponent<GunBullet>();
        _bullet.SetDamage(redBulletDamage);
        _bullet.SetImpactForce(redBulletImpactForce*GetBulletDir());
        _bullet.SetOwnerPlayerId(gunPlayer.PlayerID);
        AddRecoil();
        AudioSystem.Get().PlaySound("ÅÝÅÝÐ¡");
    }

    void ShootGreenBullet()
    {
        GameObject bullet = Instantiate(greenBullet, firePoint.position +  GetBulletDir(), firePoint.rotation, bulletPool);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = GetBulletDir() * greenBulletSpeed + new Vector3(gunPlayer.rigid.velocity.x, 0, 0);

        GunBullet _bullet = bullet.GetComponent<GunBullet>();
        _bullet.gameObject.transform.localScale *= 5f;
        _bullet.SetDamage(greenBulletDamage);
        _bullet.SetImpactForce(greenBulletImpactForce*GetBulletDir());
        _bullet.SetOwnerPlayerId(gunPlayer.PlayerID);
        AddRecoil();
        AudioSystem.Get().PlaySound("ÅÝÅÝ±¬·¢");
    }

    public override void SetGunFacing(int i)
    {
        gunFacing = i;
    }

    private void AddRecoil()//do not add reciol to avoid bug
    {
        //Vector3 forceDir = -GetBulletDir();

        //gunPlayer.SetForce(forceDir * recoil);
    }

    public Vector3 GetBulletDir()
    {
        Vector3 diraction;

        if (gunFacing < 0)
        {
            diraction = -transform.right;
        }
        else
        {
            diraction = transform.right;
        }
        return diraction;
    }
}
