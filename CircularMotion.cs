using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使物体进行围绕一个目标进行圆周运动
/// </summary>
public class CircularMotion : MonoBehaviour
{
    // 要围绕哪个物体进行旋转
    public Transform target;

    // 旋转的轴
    public Vector3 axis = Vector3.up;

    // 旋转的速度
    public float speed = 1.0f;

    void Update() {
        // 围绕target物体旋转
        transform.RotateAround(target.position, axis, speed * Time.deltaTime);
    }
}
