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

    private bool isPause = false;
    private bool isDeathScreenActive = false;
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

    // 顯示死亡畫面
    public void ShowDeathScreen()
    {
        if (DeathScreen == null) return;

        isDeathScreenActive = true;
        DeathScreen.SetActive(true);

        Time.timeScale = 0f;

        if (deathFirstButton != null) SetFirstSelectedButton(deathFirstButton);
    }

    // 重新開始遊戲
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 返回主選單
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // 繼續遊戲
    public void ContinueGame()
    {
        isPause = false;
        if (PausePanel != null) PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // 進入下一關
    public void EnterNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.sceneCountInBuildSettings > currentSceneIndex + 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("已經是最後一關，無法進入下一關。");
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

    // 設置第一個選中的按鈕
    private void SetFirstSelectedButton(Button button)
    {
        if (button != null)
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(button.gameObject);
        }
    }
}
