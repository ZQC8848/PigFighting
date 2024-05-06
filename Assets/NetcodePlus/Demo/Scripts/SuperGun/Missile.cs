using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform target; // 目标
    public float speed = 5f; // 导弹速度
    public float lerpSpeed = 5f; // 插值速度

    private Vector2 controlPoint; // 控制点

    void Start()
    {
        // 设置初始控制点为导弹和目标之间的中点
        controlPoint = (transform.position + target.position) / 2f;
    }

    void Update()
    {
        // 更新控制点为导弹和目标之间的中点
        controlPoint = (transform.position + target.position) / 2f;

        // 计算新的位置，使导弹朝向目标
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        Vector2 newPos = Vector2.Lerp(transform.position, target.position, Time.deltaTime * lerpSpeed);
        newPos += direction * Time.deltaTime * speed;

        // 设置导弹的位置
        transform.position = newPos;
    }

    // 在Scene视图中绘制贝塞尔曲线
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, controlPoint);
        Gizmos.DrawLine(controlPoint, target.position);
    }
}
