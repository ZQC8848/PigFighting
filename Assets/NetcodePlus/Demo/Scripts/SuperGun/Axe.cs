using DG.Tweening;
using NetcodePlus.Demo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Axe : Weapons
{
    private int gunFacing;

    [Header("Bullet Prefab")]
    public GameObject axeBullet;
    public GameObject axeSkill;

    [Header("Bullet Count")]
    public int maxWeaponCount;
    private int weaponCount;

    [Header("Inite Bullet Parameter")]
    public Transform firePoint;
    public Transform bulletPool;
    public float damageValue;
    public float skillDamageValue;
    private float height = 5f; // 抛物线的最高点相对起点的高度
    public float heightMultiply;
    private float duration = 3f; // 到达目标所需的时间
    public float durationMultiply;
    public float skillExsitDuration;
    public List<Transform> playerTransforms = new List<Transform>();
    public SuperGunPlayer gunPlayer;
    private void Start()
    {
        firePoint = transform.Find("FirePoint");
        bulletPool = GameObject.Find("BulletsPool").transform;
        gunPlayer = transform.parent.parent.parent.GetComponent<SuperGunPlayer>();
        weaponCount = maxWeaponCount;
        this.transform.localPosition = new Vector3 (0, 0, 0);
        //FindAllPlayers();
    }
    public override void FisrtFunc()
    {
       // FindAllPlayers();
        weaponCount --;
        GameObject bullet = Instantiate(axeBullet, firePoint.position, firePoint.rotation, bulletPool);

        //if (playerTransforms.Count <= 0)
        //{
        //    Fire(firePoint.position + new Vector3(gunFacing * 60, -10, 0), firePoint.position + new Vector3(gunFacing * 30, 5, 0), bullet.transform);
        //    return;
        //}
       // Fire(playerTransforms[UnityEngine.Random.Range(0, playerTransforms.Count - 1)].position,bullet.transform);
        AxeBullet _axeBullet = bullet.GetComponent<AxeBullet>();
        _axeBullet.damageValue = damageValue;
        _axeBullet.existDuration = duration + 0.2f;
        _axeBullet.ownerPlayerId = gunPlayer.PlayerID;
        _axeBullet.dir = gunFacing;
        AudioSystem.Get().PlaySound("斧头丢出");
        if (weaponCount <= 0)
        {
            gunPlayer.EquipWeapon("pistol");
        }
    }
    //void FindAllPlayers()
    //{
    //    // 查找所有带有 "Player" 标签的游戏对象
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    //    // 清空列表以避免重复添加
    //    playerTransforms.Clear();

    //    // 遍历每一个找到的对象，并将其 Transform 添加到列表中
    //    foreach (GameObject player in players)
    //    {
    //        if(player.GetComponent<SuperGunPlayer>().PlayerID == gunPlayer.PlayerID)
    //        {
    //            continue;
    //        }
    //        playerTransforms.Add(player.transform);
    //    }

    //    // 输出找到的玩家数量以便调试
    //    Debug.Log("Found " + playerTransforms.Count + " player(s) in the scene.");
    //    UnityEngine.Random.InitState(weaponCount);
        
    //}
    //void Fire(Vector3 targetPos,Transform bulletTransform)
    //{
    //    // 起始位置
    //    Vector3 startPos = firePoint.position;
    //    // 计算抛物线顶点
    //    height = Mathf.Abs((targetPos.x - startPos.x) * heightMultiply);
    //    duration = Mathf.Abs((targetPos - startPos).magnitude) * durationMultiply +0.2f;
    //    Vector3 peakPos = new Vector3(startPos.x, startPos.y > targetPos.y ? startPos.y : targetPos.y,0)+ new Vector3((targetPos.x - startPos.x) / 5*3,height,0);

    //    // 创建一个包含三个点的路径：起点，顶点，终点
    //    Vector3[] path = new Vector3[] { startPos, peakPos, targetPos };

    //    // 使用DOPath创建抛物线动画
    //    bulletTransform.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.Linear); // 设置缓动函数为平滑的进出
    //}
    //void Fire(Vector3 targetPos,Vector3 peakPos, Transform bulletTransform)
    //{
    //    // 起始位置
    //    Vector3 startPos = firePoint.position;
    //    // 计算抛物线顶点

    //    // 创建一个包含三个点的路径：起点，顶点，终点
    //    Vector3[] path = new Vector3[] { startPos, peakPos, targetPos };

    //    // 使用DOPath创建抛物线动画
    //    bulletTransform.DOPath(path, duration, PathType.CatmullRom); // 设置缓动函数为平滑的进出
    //}
    public override void SecondFunc()
    {
        weaponCount-=maxWeaponCount;
        AudioSystem.Get().PlaySound("斧头旋转");
        for (int i = 0; i < 12; i++)
        {
            GameObject bullet = Instantiate(axeSkill, firePoint.position, firePoint.rotation, bulletPool);
            AxeSkill_2 _axeSkill = bullet.GetComponent<AxeSkill_2>();
            _axeSkill.child.damageValue = skillDamageValue;
            _axeSkill.child.player = gunPlayer;
            _axeSkill.playerTransform = gunPlayer.transform;
            _axeSkill.existDuration = skillExsitDuration;
            _axeSkill.angle += MathF.PI/6*i;
        }
        if( weaponCount <=0 ) {
            gunPlayer.EquipWeapon("pistol");
        }
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
}
