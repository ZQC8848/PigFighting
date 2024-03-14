using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatForm2D : MonoBehaviour
{
    Collider2D platformCollider;

    public float passthroughTime=0.25f;

    public SuperGunPlayer player;


    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((player =  collision.gameObject.GetComponent<SuperGunPlayer>())!=null)
        {
            player.touchedGround = this;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((player = collision.gameObject.GetComponent<SuperGunPlayer>()) != null)
        {
            player.touchedGround = null;
        }
    }

    public void PassThroughPlatForm(Collider2D playerCol)
    {
        StartCoroutine(DisablePlatFormCoroutine(playerCol));
        Debug.Log("usePassThrough");
    }
    
    private IEnumerator DisablePlatFormCoroutine(Collider2D playerCol)
    {
        Physics2D.IgnoreCollision(playerCol, platformCollider);

        yield return new WaitForSeconds(passthroughTime);

        Physics2D.IgnoreCollision(playerCol, platformCollider,false);
    }
}
