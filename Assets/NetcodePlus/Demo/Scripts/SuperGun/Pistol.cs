using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapons
{
    public int gunFacing;

    public GameObject redBullet;
    public GameObject greenBullet;

    public Transform firePoint;
    public Transform bulletPool;
    public Transform handler;

    public int weaponCount;

    public float redBulletSpeed;
    public float greenBulletSpeed;

    public float redBulletDamage;
    public float greenBulletDamage;

    public int playerId;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        bulletPool = GameObject.Find("BulletsPool").transform;
    }
    public override void FisrtFunc()
    {
        ShootRedBullet();
        Debug.Log("use pistol 1st func");
    }

    public override void SecondFunc()
    {
        ShootGreenBullet();
        Debug.Log("use pistol 2nd func");
    }

    void ShootRedBullet()
    {
        GameObject bullet = Instantiate(redBullet, firePoint.position ,firePoint.rotation, bulletPool);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = GetBulletDir() * redBulletSpeed;

        bullet.GetComponent<GunBullet>().SetDamage(redBulletDamage);
    }

    void ShootGreenBullet()
    {
        GameObject bullet = Instantiate(greenBullet, firePoint.position, firePoint.rotation, bulletPool);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = GetBulletDir() * greenBulletSpeed;

        bullet.GetComponent<GunBullet>().SetDamage(greenBulletDamage);
    }

    public override void SetGunFacing(int i)
    {
        gunFacing = i;
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
    public override void SetFirePointOringin(Transform _handleTransform)
    {
        handler = _handleTransform;
    }
}
