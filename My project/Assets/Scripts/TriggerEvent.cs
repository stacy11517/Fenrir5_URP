using System.Collections;
using UnityEngine;
using TMPro;

public class TriggerEvent : MonoBehaviour
{
    public CameraController cameraController; // 攝影機控制器
    public CameraController.CameraMode sideViewMode = CameraController.CameraMode.SideView; // 側拍模式
    public CameraController.CameraMode followMode = CameraController.CameraMode.FollowPlayer; // 跟隨模式

    public Transform platform; // 特定區域的地面物件
    public Transform nextPlatform; // 下一塊地面
    public float[] skillOffsets = { -3f, -6f, -9f }; // 每次技能使用後平台向下移動的距離
    public float moveDuration = 0.5f; // 平台移動所需時間

    private int skillUseCount = 0; // 技能使用次數
    private bool playerInZone = false; // 玩家是否在區域內
    private bool platformLocked = true; // 平台是否鎖定，初始時為鎖定

    public GameObject IceHint;

    private void Start()
    {
        IceHint.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IceHint.SetActive(true);

            if (cameraController == null)
            {
                Debug.LogError("CameraController 未設置！");
                return;
            }

            playerInZone = true;
            platformLocked = true; // 玩家進入時鎖定平台
            cameraController.SetCameraMode(sideViewMode); // 切換到側拍模式
            Debug.Log("Player entered zone. Camera switched to SideView.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IceHint.SetActive(false);

            if (cameraController == null)
            {
                Debug.LogError("CameraController 未設置！");
                return;
            }

            playerInZone = false;
            cameraController.SetCameraMode(followMode); // 切換回跟隨模式
            Debug.Log("Player exited zone. Camera switched to FollowPlayer.");
        }
    }

    public void RegisterSkillUse()
    {
        // 確保玩家在區域內且技能使用次數不超過設定的次數
        if (!playerInZone || skillUseCount >= skillOffsets.Length) return;

        // 解鎖平台，使其可以開始移動
        platformLocked = false;

        // 增加技能使用次數
        float offset = skillOffsets[skillUseCount];
        skillUseCount++;

        // 平滑移動平台向下
        if (platform != null && !platformLocked)
        {
            Vector3 targetPosition = platform.position + new Vector3(0, offset, 0);
            StartCoroutine(SmoothMovePlatform(targetPosition, moveDuration));
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
        if (platform == null) yield break;

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
