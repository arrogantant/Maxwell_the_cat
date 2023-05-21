using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAround : MonoBehaviour
{
    public Transform target;  // 대상 오브젝트.
    public float speed;       // 회전 속도.

    void Update()
    {
        transform.RotateAround(target.position, Vector3.forward, speed * Time.deltaTime);
    }
}
