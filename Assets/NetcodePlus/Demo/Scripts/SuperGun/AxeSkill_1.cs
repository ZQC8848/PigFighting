using DG.Tweening;
using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSkill_1 : MonoBehaviour
{
    public float damageValue;
    public SuperGunPlayer player;
    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 1) * 360, 0.5f, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear) // 设置线性运动，确保平滑旋转
                 .SetLoops(-1, LoopType.Incremental); // 设置无限循环
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            SuperGunPlayer superGunPlayer = collision.gameObject.GetComponent<SuperGunPlayer>();
            if (superGunPlayer.PlayerID != player.PlayerID)
            {
                superGunPlayer.Damage(damageValue);
                SelfDestory();
            }
        }
    }
    void SelfDestory()
    {
        Destroy(gameObject);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //    else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        SuperGunPlayer superGunPlayer = collision.gameObject.GetComponent<SuperGunPlayer>();
    //        if (superGunPlayer.PlayerID != player.PlayerID)
    //        {
    //            superGunPlayer.Damage(damageValue);
    //        }
    //    }
    //}
}
