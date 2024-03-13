using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    public float damageValue;

    private void Start()
    {
        Invoke("SelfDestory", 3);
    }
    private void OnTriggerEnter(Collider other)
    {
        SelfDestory();
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
