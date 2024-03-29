using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Hurt : MonoBehaviour
{
    public float hurtTimeEclipse;
    public float hurtValue;
    public List<SuperGunPlayer> onPlatPlayerList;
    public float timer;
    void Start()
    {
        timer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<SuperGunPlayer>()!=null)
        {
            onPlatPlayerList.Add(collision.gameObject.GetComponent<SuperGunPlayer>());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SuperGunPlayer>() != null)
        {
            onPlatPlayerList.Remove(collision.gameObject.GetComponent<SuperGunPlayer>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if(onPlatPlayerList.Count>0 && timer >= hurtTimeEclipse)
        {
            foreach(SuperGunPlayer player in onPlatPlayerList)
            {
                player.Damage(hurtValue);
            }
            timer = 0;
        }
    }
}
