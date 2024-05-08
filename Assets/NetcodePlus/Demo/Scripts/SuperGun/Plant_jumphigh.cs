using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_jumphigh : MonoBehaviour
{
    public float hightExtent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x,hightExtent);
            AudioSystem.Get().PlaySound("Ò»¶ÎÌø");
        }
    }
}
