using DG.Tweening;
using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Fish : Weapons
{
    private int gunFacing;
    public float offset;
    public GameObject fishBullet;
    public GameObject fishSkill;

    [Header("Bullet Count")]
    public int maxWeaponCount;
    private int weaponCount;

    [Header("Inite Bullet Parameter")]
    public float bulletDamage;
    public Transform firePoint;
    public Transform bulletPool;

    public float moveTime;
    public SuperGunPlayer gunPlayer;


    [Header("Inite Skill Parameter")]
    public float skillDamage;
    public float searchRadius = 50f;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        bulletPool = GameObject.Find("BulletsPool").transform;
        gunPlayer = transform.parent.parent.parent.GetComponent<SuperGunPlayer>();
        weaponCount = maxWeaponCount;
    }
    public override void FisrtFunc()
    {
        weaponCount--;
        Random.InitState(weaponCount);

        for(int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(fishBullet, firePoint.position, firePoint.rotation*Quaternion.Euler(0,0, -90*gunFacing + offset * (i-1) ), bulletPool);
            FishBullet _fishBullet = bullet.GetComponent<FishBullet>();
            _fishBullet.Speed = 100;
        }
        
        if (weaponCount <= 0)
        {
            gunPlayer.EquipWeapon("pistol");
        }
    }
    public override void SecondFunc()
    {
        weaponCount -= maxWeaponCount;


        if (weaponCount <= 0)
        {
            gunPlayer.EquipWeapon("pistol");
        }
    }

    List<Transform> FindPlayersAround()
    {
        List<Transform> foundTransforms = new List<Transform>();

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject gameObject in gameObjects)
        {
            foundTransforms.Add(gameObject.transform);
        }

        return foundTransforms;
    }

    public override void SetGunFacing(int i)
    {
        gunFacing = i;
    }
}
