using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    // Start is called before the first frame update
    private float remainTime = 10f;
    public float damageValue;
    public int ownerPlayerId;
    void Start()
    {
        Invoke("SelfDestory", remainTime);
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
            if (collision.gameObject.GetComponent<AxeSkill_1>().player.PlayerID == ownerPlayerId)
            {
                return;
            }
            SelfDestory();
        }
    }
    public void SetOwnerPlayerId(int id)
    {
        ownerPlayerId = id;
    }
    public void SetDamage(float damage)
    {
        damageValue = damage;
    }
    void SelfDestory()
    {
        Destroy(gameObject);
    }
}
