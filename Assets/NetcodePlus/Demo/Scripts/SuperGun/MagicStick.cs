using DG.Tweening;
using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicStick : Weapons
{
    private int gunFacing;

    public GameObject magicBullet;
    public GameObject magicSkill;

    [Header("Bullet Count")]
    public int maxWeaponCount;
    private int weaponCount;

    [Header("Inite Bullet Parameter")]
    public float bulletDamage;
    public Transform firePoint;
    public Transform bulletPool;
    public float minX_distance;
    public float maxX_distance;
    public float Y_range;
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
        Debug.Log("use Magic 1!!");
        weaponCount--;
        Random.InitState(weaponCount);
        AudioSystem.Get().PlaySound("·¨ÕÈ");

        for(int i = 0;i<7;i++)
        {
            GameObject bullet = Instantiate(magicBullet, firePoint.position, firePoint.rotation, bulletPool);
            float x = gunFacing * (maxX_distance - (maxX_distance - minX_distance) / 7 * Mathf.Abs(7/2 - i))* Random.Range(1,1.5f);
            float y = (7f / 2 - i) * Y_range* Random.Range(1, 1.5f);
            bullet.transform.DOLocalMove(firePoint.position + new Vector3(x,y,0),moveTime);

            MagicBullet _bullet = bullet.GetComponent<MagicBullet>();
            _bullet.SetDamage(bulletDamage);
            _bullet.SetOwnerPlayerId(gunPlayer.PlayerID);
        }
        if (weaponCount <= 0)
        {
            gunPlayer.EquipWeapon("pistol");
        }
    }
    public override void SecondFunc()
    {
        weaponCount-=maxWeaponCount;
       // Random.InitState(weaponCount);
        Debug.Log("use magic 2!!");
        List<Transform> playerTransforms = FindPlayersAround(transform.position, searchRadius, "Player");
        foreach (Transform playerTransform in playerTransforms)
        {
            GameObject skill = Instantiate(magicSkill, playerTransform.position, playerTransform.rotation, bulletPool);
            MagicSkill _magicSkill = skill.GetComponent<MagicSkill>();
            _magicSkill.targetTransform = playerTransform;
            _magicSkill.damageValue = skillDamage;
            _magicSkill.ownerPlayerId = gunPlayer.PlayerID;
            AudioSystem.Get().PlaySound("À×»÷");
        }

        if (weaponCount <= 0)
        {
            gunPlayer.EquipWeapon("pistol");
        }
    }

    List<Transform> FindPlayersAround(Vector2 center, float radius, string tag)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        List<Transform> foundTransforms = new List<Transform>();

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(tag))
            {
                if(hitCollider.GetComponent<SuperGunPlayer>().PlayerID == gunPlayer.PlayerID)
                {
                    continue;
                }
                foundTransforms.Add(hitCollider.transform);
            }
        }

        return foundTransforms;
    }

    public override void SetGunFacing(int i)
    {
        gunFacing = i;
    }
}
