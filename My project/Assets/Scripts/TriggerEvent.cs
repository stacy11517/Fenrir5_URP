using System.Collections;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public CameraController cameraController; // 連接攝影機控制器
    public CameraController.CameraMode sideViewMode = CameraController.CameraMode.SideView; // 側拍模式
    public CameraController.CameraMode followMode = CameraController.CameraMode.FollowPlayer; // 跟隨模式

    public Transform platform; // 特定區域的地面物件
    public Transform nextPlatform; // 下一塊地面
    public float[] skillOffsets = { -3f, -6f, -9f }; // 每次技能使用後平台下移的距離
    public float moveDuration = 0.5f; // 平台移動所需時間
    private int skillUseCount = 0; // 技能使用次數
    private bool playerInZone = false; // 玩家是否在區域內

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true; // 玩家進入區域
            cameraController.SetCameraMode(sideViewMode); // 切換到側拍模式
            Debug.Log("Player entered zone. Camera switched to SideView.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false; // 玩家離開區域
            cameraController.SetCameraMode(followMode); // 切換回跟隨模式
            Debug.Log("Player exited zone. Camera switched to FollowPlayer.");
        }
    }

    public void RegisterSkillUse()
    {
        if (!playerInZone || skillUseCount >= skillOffsets.Length) return;

        // 增加技能使用次數
        float offset = skillOffsets[skillUseCount];
        skillUseCount++;

        // 平滑移動平台
        if (platform != null)
        {
            StartCoroutine(SmoothMovePlatform(platform.position + new Vector3(0, offset, 0), moveDuration));
        }

        // 如果是最後一次技能使用，連接下一塊地面
        if (skillUseCount == skillOffsets.Length && nextPlatform != null)
        {
            Vector3 newPosition = platform.position;
            newPosition.y = nextPlatform.position.y; // 確保新位置接上下一塊地面
            StartCoroutine(SmoothMovePlatform(newPosition, moveDuration));
            Debug.Log("Platform connected to next platform.");
        }
    }

    private IEnumerator SmoothMovePlatform(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = platform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            platform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        platform.position = targetPosition;
        Debug.Log("Platform moved to position: " + targetPosition);
    }
}
