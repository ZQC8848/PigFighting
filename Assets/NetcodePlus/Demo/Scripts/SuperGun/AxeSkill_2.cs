using DG.Tweening;
using NetcodePlus.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSkill_2 : MonoBehaviour
{
    public float amplificateTime;
    public float skillTime;
    public float existDuration;
    public AxeSkill_1 child;
    public Transform playerTransform;

    public float radius; // 圆周运动的半径
    public float speed; // 圆周运动的速度

    public float angle = 0f;
    void Start()
    {
        Invoke("SelfDestory", existDuration);
    }
    void Update()
    {
        // 计算物体在圆周上的位置
        float x = playerTransform.position.x + Mathf.Cos(angle) * radius;
        float y = playerTransform.position.y + Mathf.Sin(angle) * radius;

        // 设置物体的位置
        transform.position = new Vector3(x, y, transform.position.z);

        // 更新角度，实现圆周运动
        angle += speed * Time.deltaTime;
    }
    void SelfDestory()
    {
        AudioSystem.Get().StopSound("斧头旋转");
        Destroy(gameObject);
    }
}
