using DG.Tweening;
using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStick : Weapons
{
    private int gunFacing;

    public GameObject magicBullet;

    [Header("Bullet Count")]
    public int weaponCount;

    [Header("Inite Bullet Parameter")]
    public Transform firePoint;
    public Transform bulletPool;
    public float minX_distance;
    public float maxX_distance;
    public float Y_range;
    public float moveTime;
    public SuperGunPlayer gunPlayer;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        bulletPool = GameObject.Find("BulletsPool").transform;
        gunPlayer = transform.parent.parent.parent.GetComponent<SuperGunPlayer>();
    }
    public override void FisrtFunc()
    {
        Debug.Log("use Magic 1!!");
        GameObject bullet =  Instantiate(magicBullet);
        bullet.transform.DOLocalMove(firePoint.position + new Vector3(gunFacing * Random.Range(minX_distance, maxX_distance), Random.Range(-Y_range, Y_range), 0f),moveTime);
    }
    public override void SecondFunc()
    {
        Debug.Log("use magic 2!!");
    }
    public override void SetGunFacing(int i)
    {
        gunFacing = i;
    }
}
