using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBullet : MonoBehaviour
{
    public float Speed;
    public int ownerPlayerId;
    public float damageValue;
    private float mentainTime=3f;
    void Start()
    {
        Invoke("SelfDestory", mentainTime);
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
    void SelfDestory()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0),Space.Self);
    }
}
