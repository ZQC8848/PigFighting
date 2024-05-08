using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MagicSkill : MonoBehaviour
{
    private float moveTime = 0.2f;
    public float damageValue;
    public int ownerPlayerId;
    public Transform targetTransform;
    void Start()
    {
        Invoke("SelfDestory", moveTime+0.2f);
        this.transform.DOMove(targetTransform.position+ new Vector3(0,30f,0),moveTime).From(true);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SuperGunPlayer superGunPlayer = collision.gameObject.GetComponent<SuperGunPlayer>();
        if (superGunPlayer != null )
        {
            superGunPlayer.Damage(damageValue);
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
