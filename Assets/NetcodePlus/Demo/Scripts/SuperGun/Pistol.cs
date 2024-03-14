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

    public SuperGunPlayer gunPlayer;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        bulletPool = GameObject.Find("BulletsPool").transform;
        gunPlayer = transform.parent.parent.GetComponent<SuperGunPlayer>();
    }
    public override void FisrtFunc()
    {
        ShootRedBullet();
    }

    public override void SecondFunc()
    {
        ShootGreenBullet();
    }

    void ShootRedBullet()
    {
        GameObject bullet = Instantiate(redBullet, firePoint.position ,firePoint.rotation, bulletPool);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = GetBulletDir() * redBulletSpeed;

        GunBullet _bullet = bullet.GetComponent<GunBullet>();
        _bullet.SetDamage(redBulletDamage);
        _bullet.SetImpactForce(redBulletImpactForce*GetBulletDir());

        AddRecoil();
    }

    void ShootGreenBullet()
    {
        GameObject bullet = Instantiate(greenBullet, firePoint.position, firePoint.rotation, bulletPool);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = GetBulletDir() * greenBulletSpeed;

        GunBullet _bullet = bullet.GetComponent<GunBullet>();
        _bullet.SetDamage(greenBulletDamage);
        _bullet.SetImpactForce(greenBulletImpactForce*GetBulletDir());

        AddRecoil();
    }

    public override void SetGunFacing(int i)
    {
        gunFacing = i;
    }

    private void AddRecoil()
    {
        Vector3 forceDir = -GetBulletDir();

        gunPlayer.SetForce(forceDir * recoil);
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
