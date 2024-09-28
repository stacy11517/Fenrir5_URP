using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    public Transform target;    // ����]�D���^��Transform
    public Vector3 offset;      // �۾��P���⤧�����Z��

    void Start()
    {
        // �p�G�S����ʳ]�m offset�A�h�۰ʳ]�m�@�ӦX�z���Z��
        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        // �۾����H����A�O�� offset �Z��
        transform.position = target.position + offset;

        // �۾��ݦV����
        transform.LookAt(target);
    }
}
