using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIbutton : MonoBehaviour
{
    // 主畫面和其他 UI 面板
    public GameObject MainPanel;
    public GameObject PausePanel;
    public GameObject SettingPanel;
    public GameObject DeathScreen;

    // 操作圖片和玩法圖片
    public GameObject OperationImage;
    public GameObject GameplayImage;

    // 按鈕
    public Button pauseFirstButton;
    public Button mainMenuFirstButton;
    public Button settingsFirstButton;
    public Button deathFirstButton;
    public Button volumeSettingsButton; // 音量設定按鈕
    public Button confirmVolumeButton;  // 確認音量按鈕

    // 背景音樂音量調整
    public Slider bgmVolumeSlider;      // 音量滑塊

    private bool isPause = false;
    private bool isDeathScreenActive = false;
    private bool isAdjustingVolume = false; // 是否正在調整音量
    private EventSystem eventSystem;

    private void Start()
    {
        // 初始化面板狀態
        if (MainPanel != null) MainPanel.SetActive(true);
        if (PausePanel != null) PausePanel.SetActive(false);
        if (SettingPanel != null) SettingPanel.SetActive(false);
        if (DeathScreen != null) DeathScreen.SetActive(false);

        // 初始化圖片狀態
        if (OperationImage != null) OperationImage.SetActive(false);
        if (GameplayImage != null) GameplayImage.SetActive(false);

        // 設置主畫面按鈕
        eventSystem = EventSystem.current;
        if (mainMenuFirstButton != null) SetFirstSelectedButton(mainMenuFirstButton);


    }

    private void Update()
    {
        // 檢測按下暫停按鍵（Escape 或手柄 Start 按鈕）
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) && !isDeathScreenActive)
        {
            TogglePause();
        }

        // 如果在調整音量，允許手柄或鍵盤操作滑塊
        if (isAdjustingVolume && bgmVolumeSlider != null)
        {
            float input = Input.GetAxis("Horizontal"); // 手柄左右軸或鍵盤左右鍵
            if (Mathf.Abs(input) > 0.1f)
            {
                bgmVolumeSlider.value += input * Time.deltaTime; // 平滑調整音量
            }

            // 按下確認鍵（Enter 或手柄 A 按鈕）時確認音量
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ConfirmVolumeAdjustment();
            }
        }
    }

    // 切換暫停狀態
    public void TogglePause()
    {
        if (PausePanel == null) return;

        isPause = !isPause;

        if (isPause)
        {
            PausePanel.SetActive(true);
            if (MainPanel != null) MainPanel.SetActive(false);
            Time.timeScale = 0f;

            // 設置暫停面板按鈕
            if (pauseFirstButton != null) SetFirstSelectedButton(pauseFirstButton);
        }
        else
        {
            PausePanel.SetActive(false);
            if (SettingPanel != null) SettingPanel.SetActive(false);
            if (MainPanel != null) MainPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // 進入設置面板
    public void EnterSettings()
    {
        if (SettingPanel == null) return;

        if (MainPanel != null) MainPanel.SetActive(false);
        if (PausePanel != null) PausePanel.SetActive(false);
        SettingPanel.SetActive(true);

        if (settingsFirstButton != null) SetFirstSelectedButton(settingsFirstButton);
    }

    // 進入音量調整功能
    public void EnterVolumeSettings()
    {
        if (bgmVolumeSlider == null || confirmVolumeButton == null) return;

        isAdjustingVolume = true;

        // 激活音量滑塊並設置滑塊為選中目標
        bgmVolumeSlider.gameObject.SetActive(true);
        SetFirstSelectedButton(confirmVolumeButton);
    }

    // 確認音量調整，返回音量設定按鈕
    public void ConfirmVolumeAdjustment()
    {
        isAdjustingVolume = false;

        if (volumeSettingsButton != null)
        {
            SetFirstSelectedButton(volumeSettingsButton);
        }
    }

    // 返回主畫面
    public void ReturnToMainPanel()
    {
        if (MainPanel == null) return;

        if (SettingPanel != null) SettingPanel.SetActive(false);
        if (PausePanel != null) PausePanel.SetActive(false);
        MainPanel.SetActive(true);

        if (mainMenuFirstButton != null) SetFirstSelectedButton(mainMenuFirstButton);
    }

    // 顯示操作圖片
    public void ShowOperationImage()
    {
        if (OperationImage != null) OperationImage.SetActive(true);
        if (GameplayImage != null) GameplayImage.SetActive(false);
    }

    // 顯示玩法圖片
    public void ShowGameplayImage()
    {
        if (OperationImage != null) OperationImage.SetActive(false);
        if (GameplayImage != null) GameplayImage.SetActive(true);
    }

    // 設置第一個選中的按鈕
    private void SetFirstSelectedButton(Button button)
    {
        if (button != null)
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(button.gameObject);
        }
    }

    // 退出遊戲
    public void QuitGame()
    {
        Debug.Log("退出遊戲...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
