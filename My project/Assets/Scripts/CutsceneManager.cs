using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public GameObject mainMenuPanel;   // 主選單的 Panel
    public GameObject cutscenePanel;  // 用於顯示圖片的 Panel
    public Image displayImage;        // 顯示圖片的 UI Image
    public Sprite[] cutsceneSprites;  // 切換的圖片陣列
    public float imageDuration = 3f;  // 每張圖片顯示時間

    private int currentImageIndex = 0; // 當前播放的圖片索引
    private bool isSkipping = false;   // 是否正在跳過
    private float holdTime = 0f;       // 按鍵按住時間
    private bool isHoldingKey = false; // 是否正在按住鍵

    void Start()
    {
        // 初始化：確保主選單顯示，切換面板隱藏
        mainMenuPanel.SetActive(true);
        cutscenePanel.SetActive(false);
    }

    void Update()
    {
        // 檢測是否長按任意鍵
        if (Input.anyKey)
        {
            if (!isHoldingKey)
            {
                isHoldingKey = true;
                holdTime = 0f; // 初始化按住時間
            }

            holdTime += Time.deltaTime;
            if (holdTime >= 3f) // 如果按住時間達到 3 秒
            {
                SkipCutscene();
            }
        }
        else
        {
            isHoldingKey = false;
        }
    }

    public void StartGame()
    {
        // 關閉主選單面板，打開圖片播放面板
        mainMenuPanel.SetActive(false);
        cutscenePanel.SetActive(true);

        // 開始播放切換圖片的協程
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        while (currentImageIndex < cutsceneSprites.Length)
        {
            // 顯示當前圖片
            displayImage.sprite = cutsceneSprites[currentImageIndex];

            // 等待指定時間或直到跳過
            float elapsedTime = 0f;
            while (elapsedTime < imageDuration)
            {
                if (isSkipping)
                    yield break; // 如果跳過，結束播放
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 切換到下一張圖片
            currentImageIndex++;
        }

        // 播放完畢後進入第一關
        LoadFirstLevel();
    }

    void SkipCutscene()
    {
        isSkipping = true; // 設置跳過標記
        LoadFirstLevel();
    }

    void LoadFirstLevel()
    {
        // 載入第一關，替換 "Level1" 為你的第一關場景名稱
        SceneManager.LoadScene(1);
    }
}
