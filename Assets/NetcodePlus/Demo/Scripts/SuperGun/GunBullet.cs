using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    public float damageValue;
    public Vector3 impactForce;
    int ownerPlayerId;
        
    private void Start()
    {
        Invoke("SelfDestory", 2);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SuperGunPlayer superGunPlayer = collision.gameObject.GetComponent<SuperGunPlayer>();
        if (superGunPlayer != null && superGunPlayer.PlayerID != ownerPlayerId)
        {
            superGunPlayer.SetDamageSource(ownerPlayerId);
            superGunPlayer.Damage(damageValue);
            //superGunPlayer.SetForce(impactForce);
            AudioSystem.Get().PlaySound("Уќжа");
            SelfDestory();
        }
        else if(collision.gameObject.GetComponent<AxeSkill_1>()!=null)
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

    public void SetImpactForce(Vector3 _impactForce)
    {
        impactForce = _impactForce;
    }

    void SelfDestory()
    {
        Destroy(gameObject);
    }
}
