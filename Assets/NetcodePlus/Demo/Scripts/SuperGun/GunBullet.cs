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
        Invoke("SelfDestory", 3);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SuperGunPlayer superGunPlayer = collision.gameObject.GetComponent<SuperGunPlayer>();
        if (superGunPlayer != null && superGunPlayer.PlayerID != ownerPlayerId)
        {
            superGunPlayer.Damage(damageValue);
            superGunPlayer.SetForce(impactForce);
        }    
        
        SelfDestory();
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
