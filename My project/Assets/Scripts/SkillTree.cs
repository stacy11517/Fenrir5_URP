using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public CameraShake cameraShake;  // CameraShake �ޥ�

    void Start()
    {
        // �T�O Camera �W�� CameraShake �}��
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        // ���յe���_�ʮĪG�A���U���N����]�o�̥H Xbox ����� X �䬰�ҡ^
        if (Input.GetKeyDown(KeyCode.JoystickButton2))  // ���� Xbox ����� X ��
        {
            cameraShake.TriggerShake();  // Ĳ�o�e���_��
        }
    }
}
