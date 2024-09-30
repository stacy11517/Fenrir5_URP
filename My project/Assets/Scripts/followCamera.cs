using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    public Transform target;    // 角色（主角）的Transform
    public Vector3 offset;      // 相机与角色之间的距离
    public bool isShaking = false;  // 标记是否正在震动

    void Start()
    {
        // 如果没有手动设置 offset，则自动设置一个合理的距离
        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        // 只有在不震动时，才更新相机的位置
        if (!isShaking)
        {
            // 相机跟随角色，保持 offset 距离
            transform.position = target.position + offset;
            // 相机看向角色
            transform.LookAt(target);
        }
    }
}
