using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NetcodePlus.Demo;
public class FishSkill : MonoBehaviour
{
    public float skillMentainTime;
    float openAndCloseTime=0.2f;
    public float skillDamage;
    public int ownerPlayerId;
    void Start()
    {
        ShowLaser();
        Invoke("CloseLaser", skillMentainTime);
    }
    void ShowLaser()
    {
        this.transform.DOScaleY(10f, openAndCloseTime);
    }
    void CloseLaser()
    {
        this.transform.DOScaleY(0.01f, openAndCloseTime);
        Invoke("SelfDestory", openAndCloseTime);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        SuperGunPlayer superGunPlayer = collision.gameObject.GetComponent<SuperGunPlayer>();
        if (superGunPlayer != null && superGunPlayer.PlayerID != ownerPlayerId)
        {
            superGunPlayer.SetDamageSource(ownerPlayerId);
            superGunPlayer.Damage(skillDamage);
        }
    }
    void SelfDestory()
    {
        Destroy(this.gameObject);
    }
}
