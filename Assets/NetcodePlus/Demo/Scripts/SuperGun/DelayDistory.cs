using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DelayDistory : MonoBehaviour
{
    public float time = 5;
    void Start()
    {
        Invoke("DelayDisory", time);
    }
    void DelayDisory()
    {
        Destroy(gameObject);
    }
    public void StartRotate()
    {
        this.transform.DOLocalRotate(new Vector3(0, 0, 180), 1.5f).SetLoops(10, LoopType.Incremental);
    }
}
