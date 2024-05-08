using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    public float RotateSpeed = 1;
    // Start is called before the first frame update
    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 1) * 360, 0.5f/RotateSpeed , RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear) // 设置线性运动，确保平滑旋转
                 .SetLoops(-1, LoopType.Incremental); // 设置无限循环
    }
}
