using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Plant_jumphigh : MonoBehaviour
{
    public float hightExtent;
    public Transform parent;
    public float boundExtent;
    public float boundTime;
    private void Start()
    {
        parent = transform.parent;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x,hightExtent);
            AudioSystem.Get().PlaySound("Ò»¶ÎÌø");
            AudioSystem.Get().PlaySound("Ð¡»ÆÑ¼");
            parent.gameObject.GetComponent<Animator>().SetTrigger("duck");
            //BoundAnim();
        }
    }
    void BoundAnim()
    {
        float prescaleY = parent.transform.localScale.y;
        parent.DOScaleY(boundExtent, boundTime / 2);
        StartCoroutine(BackDuckState(prescaleY));
    }
    IEnumerator BackDuckState(float prescaleY)
    {
        yield return new WaitForSeconds(boundTime / 2);
        parent.DOScaleY(prescaleY, boundTime / 2);
    }
}
