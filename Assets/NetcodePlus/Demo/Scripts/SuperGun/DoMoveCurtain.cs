using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DoMoveCurtain : MonoBehaviour
{
    public Transform target;
    public float time;
    public void DoMoveObj()
    {
        this.transform.DOMove(target.position, time);
    }
}
