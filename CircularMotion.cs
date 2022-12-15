using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    // ҪΧ���ĸ����������ת
    public Transform target;

    // ��ת����
    public Vector3 axis = Vector3.up;

    // ��ת���ٶ�
    public float speed = 1.0f;

    void Update() {
        // Χ��target������ת
        transform.RotateAround(target.position, axis, speed * Time.deltaTime);
    }
}
