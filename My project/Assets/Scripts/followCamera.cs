using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    public Transform target;    // 角色（主角）的Transform
    public Vector3 offset;      // 相機與角色之間的距離

    void Start()
    {
        // 如果沒有手動設置 offset，則自動設置一個合理的距離
        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        // 相機跟隨角色，保持 offset 距離
        transform.position = target.position + offset;

        // 相機看向角色
        transform.LookAt(target);
    }
}
