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
    public void SelfDestory()
    {
        Destroy(this.gameObject);
    }
}
