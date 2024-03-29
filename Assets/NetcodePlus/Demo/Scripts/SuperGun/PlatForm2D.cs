using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatForm2D : MonoBehaviour
{
    Collider2D platformCollider;

    private float passthroughTime=0.25f;
    public List<SuperGunPlayer> onPlatPlayerList;
    SuperGunPlayer player;

    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((player = collision.gameObject.GetComponent<SuperGunPlayer>()) != null)
        {
            onPlatPlayerList.Add(player);
            player.touchedGround = this;
            player = null;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((player = collision.gameObject.GetComponent<SuperGunPlayer>()) != null)
        {
            onPlatPlayerList.Remove(player);
            player.touchedGround = null;
            player = null;
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
