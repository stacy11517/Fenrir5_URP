using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public CameraShake cameraShake;  // CameraShake 引用

    void Start()
    {
        // 確保 Camera 上有 CameraShake 腳本
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        // 測試畫面震動效果，按下任意按鍵（這裡以 Xbox 控制器的 X 鍵為例）
        if (Input.GetKeyDown(KeyCode.JoystickButton2))  // 對應 Xbox 控制器的 X 鍵
        {
            cameraShake.TriggerShake();  // 觸發畫面震動
        }
    }
}
