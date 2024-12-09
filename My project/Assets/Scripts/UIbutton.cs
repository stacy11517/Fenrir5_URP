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

    // 操作圖片和玩法圖片
    public GameObject OperationImage;
    public GameObject GameplayImage;

    // 按鈕
    public Button pauseFirstButton;
    public Button mainMenuFirstButton;
    public Button settingsFirstButton;


    private bool isPause = false;
    private EventSystem eventSystem;

    private void Start()
    {
        // 初始化面板狀態
        if (MainPanel != null) MainPanel.SetActive(true);
        if (PausePanel != null) PausePanel.SetActive(false);
        if (SettingPanel != null) SettingPanel.SetActive(false);

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
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)))
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

    // 恢复游戏
    public void ContinueGame()
    {
        Time.timeScale = 1f; // 恢复游戏时间
        PausePanel.SetActive(false); // 隐藏暂停菜单
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
    // 返回主菜单
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // 恢复时间（防止从暂停状态返回）
        SceneManager.LoadScene(0); // 加载场景索引为 0 的主菜单
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
    // 返回暫停畫面
    public void ReturnToPPausePanel()
    {
        if (PausePanel == null) return;

        if (SettingPanel != null) SettingPanel.SetActive(false);
        PausePanel.SetActive(true);

        if (pauseFirstButton != null) SetFirstSelectedButton(pauseFirstButton);
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
    public void RestartScene()
    {
        // 获取当前活动场景的名称并重新加载
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
