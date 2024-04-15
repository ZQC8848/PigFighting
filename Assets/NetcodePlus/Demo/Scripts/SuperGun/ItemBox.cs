using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private float SelfDestoryTime = 10;
    
    private void Start()
    {
        Invoke("SelfDestory", SelfDestoryTime);
    }
    void SelfDestory()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            Transform SeperateBoxes = this.transform.parent.Find("SeperateBoxes");
            SeperateBoxes.gameObject.SetActive(true);
            foreach (Transform child in SeperateBoxes)
            {
                child.gameObject.GetComponent<Rigidbody2D>().velocity = (child.position - SeperateBoxes.position)*Random.Range(3,8);
            }
        }
    }
}
