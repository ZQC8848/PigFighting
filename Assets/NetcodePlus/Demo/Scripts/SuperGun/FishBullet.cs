using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBullet : MonoBehaviour
{
    public float Speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0),Space.Self);
    }
}
