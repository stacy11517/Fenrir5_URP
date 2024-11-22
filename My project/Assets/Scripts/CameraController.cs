using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode { FollowPlayer, SideView, FixedView }
    public CameraMode currentMode;

    public Transform target;         // 玩家角色的 Transform
    public Vector3 offset;           // 相機與玩家的偏移量
    public Transform fixedPosition;  // 固定攝影機位置（第三關）

    public float shakeDuration = 0.5f;   // 震動持續時間
    public float shakeMagnitude = 0.1f; // 震動幅度

    private bool isShaking = false;  // 是否正在震動

    void Start()
    {
        // 如果 offset 未設置，計算初始偏移量
        if (offset == Vector3.zero && target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (isShaking) return; // 如果正在震動，跳過普通更新

        UpdateCameraPosition(); // 更新攝影機位置
    }

    /// <summary>
    /// 更新攝影機位置和行為
    /// </summary>
    void UpdateCameraPosition()
    {
        switch (currentMode)
        {
            case CameraMode.FollowPlayer:
                FollowPlayer();
                break;

            case CameraMode.SideView:
                SideView();
                break;

            case CameraMode.FixedView:
                FixedView();
                break;
        }
    }

    /// <summary>
    /// 攝影機跟隨玩家
    /// </summary>
    void FollowPlayer()
    {
        if (target == null) return;
        transform.position = target.position + offset;
        transform.LookAt(target);
    }

    /// <summary>
    /// 攝影機側拍玩家
    /// </summary>
    void SideView()
    {
        if (target == null) return;

        Vector3 sideOffset = offset;
        sideOffset.z = 0; // 鎖定 Z 軸，實現側視效果
        transform.position = target.position + sideOffset;
        transform.LookAt(target);
    }

    /// <summary>
    /// 攝影機固定位置
    /// </summary>
    void FixedView()
    {
        if (fixedPosition == null) return;
        transform.position = fixedPosition.position;
        transform.rotation = fixedPosition.rotation;
    }

    /// <summary>
    /// 設定攝影機模式
    /// </summary>
    /// <param name="mode">新的攝影機模式</param>
    public void SetCameraMode(CameraMode mode)
    {
        currentMode = mode;
    }

    /// <summary>
    /// 觸發攝影機震動
    /// </summary>
    /// <param name="duration">震動持續時間</param>
    /// <param name="magnitude">震動幅度</param>
    public void TriggerShake(float duration, float magnitude)
    {
        if (!isShaking)
        {
            shakeDuration = duration;
            shakeMagnitude = magnitude;
            StartCoroutine(Shake());
        }
    }

    /// <summary>
    /// 攝影機震動協程
    /// </summary>
    IEnumerator Shake()
    {
        isShaking = true;

        Vector3 originalPosition = transform.position; // 記錄震動前的初始位置
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            Vector3 shakePosition = new Vector3(x, y, 0); // 震動位置偏移
            transform.position = target.position + offset + shakePosition; // 更新攝影機位置

            elapsed += Time.deltaTime;

            yield return null;
        }

        // 恢復到正常位置
        transform.position = target.position + offset;
        isShaking = false;
    }
}
