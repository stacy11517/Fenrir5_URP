using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;  // 震动持续时间
    public float shakeMagnitude = 0.1f;  // 震动幅度
    private Vector3 originalPosition;     // 原始位置
    private followCamera followCameraScript;  // 引用 followCamera 脚本
    private Transform target;              // 角色的位置

    void Start()
    {
        originalPosition = transform.localPosition;  // 记录相机的初始位置
        followCameraScript = GetComponent<followCamera>();  // 获取 followCamera 脚本
        target = followCameraScript.target;  // 获取角色的 Transform
    }

    public void TriggerShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        followCameraScript.isShaking = true;  // 设置震动标记

        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // 随机偏移量
            float x = Random.Range(-1f, 1f) * shakeMagnitude; // X轴震动
            float y = Random.Range(-1f, 1f) * shakeMagnitude; // Y轴震动

            // 计算震动位置
            Vector3 shakePosition = new Vector3(x, y, 0); // X和Y都施加震动
            transform.position = target.position + followCameraScript.offset + shakePosition; // 根据角色位置、offset和震动偏移量设置相机位置

            elapsed += Time.deltaTime;

            yield return null;
        }

        // 恢复到原始位置
        transform.position = target.position + followCameraScript.offset;

        followCameraScript.isShaking = false;  // 恢复相机跟随
    }
}
