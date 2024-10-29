using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera introCamera;  // 初始的攝影機，用於拍攝關卡主題
    public Camera mainCamera;   // 遊戲的主攝影機
    public float introDuration = 5f;  // 拍攝主題的持續時間（秒）

    void Start()
    {
        // 確保開始時 introCamera 啟動，mainCamera 關閉
        mainCamera.gameObject.SetActive(false);
        introCamera.gameObject.SetActive(true);

        // 在指定時間後切換到主攝影機
        StartCoroutine(SwitchToMainCamera());
    }

    IEnumerator SwitchToMainCamera()
    {
        // 等待指定的時間
        yield return new WaitForSeconds(introDuration);

        // 切換攝影機
        introCamera.gameObject.SetActive(false);  // 關閉主題攝影機
        mainCamera.gameObject.SetActive(true);    // 啟動主攝影機
    }
}
