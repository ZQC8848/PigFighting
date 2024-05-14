using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AxeBullet : MonoBehaviour
{
    public float damageValue;
    public int ownerPlayerId;
    public float rotateduration;
    public float existDuration;
    public Vector3 rotateAxis;
    public int dir;
    public float velocity_horizon;
    public float velocity_verticle;
    public float gravity;
    private void Start()
    {
        Invoke("SelfDestory", existDuration);
        // 循环旋转
        //transform.DORotate(rotateAxis * 360, rotateduration, RotateMode.FastBeyond360)
        //         .SetEase(Ease.Linear) // 设置线性运动，确保平滑旋转
        //         .SetLoops(-1, LoopType.Incremental); // 设置无限循环
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SuperGunPlayer superGunPlayer = collision.gameObject.GetComponent<SuperGunPlayer>();
        if (superGunPlayer != null && superGunPlayer.PlayerID != ownerPlayerId)
        {
            superGunPlayer.SetDamageSource(ownerPlayerId);
            superGunPlayer.Damage(damageValue);
            SelfDestory();
        }
        else if (collision.gameObject.GetComponent<AxeSkill_1>() != null)
        {
            if(collision.gameObject.GetComponent<AxeSkill_1>().player.PlayerID == ownerPlayerId)
            {
                return;
            }
            SelfDestory();
        }
    }
    private void Update()
    {
        velocity_verticle -= gravity*Time.deltaTime;
        Vector3 distance = new Vector3(velocity_horizon*dir,velocity_verticle,0)*Time.deltaTime;
        transform.Translate(distance);
    }
    void SelfDestory()
    {
        Destroy(gameObject);
    }
}
