using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private float SelfDestoryTime = 10;

    public int eventSerial;
    private void Start()
    {
        Invoke("SelfDestory", SelfDestoryTime);
    }
    void SelfDestory()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
    public void SetEventSerial(int _eventSerial)
    {
        this.eventSerial = _eventSerial;
    }
    private void TrrigerEvent(SuperGunPlayer player)
    {
        RandomEventOnServer.instance.TriggerEvent(eventSerial, player);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TrrigerEvent(collision.gameObject.GetComponent<SuperGunPlayer>());
            SelfDestory();
        }
    }
}
