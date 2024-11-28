using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIbutton : MonoBehaviour
{
    // 主面板和其他 UI 元素
    public GameObject MainPanel;        // 主畫面 UI（可能不存在）
    public GameObject PausePanel;       // 暫停 UI（可能不存在）
    public GameObject SettingPanel;     // 設置 UI（可能不存在）
    public GameObject DeathScreen;      // 死亡畫面 UI（可能不存在）

    // 操作圖片和玩法圖片
    public GameObject OperationImage;  // 操作圖片
    public GameObject GameplayImage;   // 玩法圖片

    // 按鈕
    public Button pauseFirstButton;    // 暫停面板的第一個按鈕（可能為空）
    public Button mainMenuFirstButton; // 主畫面的第一個按鈕（可能為空）
    public Button settingsFirstButton; // 設置面板的第一個按鈕（可能為空）

    private bool isPause = false;       // 是否處於暫停狀態
    private bool isDeathScreenActive = false; // 死亡畫面是否顯示
    private EventSystem eventSystem;    // 控制按鈕選擇的 EventSystem

    private PlayerInput playerInput;    // Input System 的 PlayerInput

    private void Awake()
    {
        // 初始化 PlayerInput 與 EventSystem
        playerInput = GetComponent<PlayerInput>();
        eventSystem = EventSystem.current;

        // 訂閱 Input Actions 的事件
        playerInput.actions["Pause"].performed += OnPausePerformed;
    }

    private void Start()
    {
        // 初始化面板狀態，檢查面板是否存在再處理
        if (MainPanel != null) MainPanel.SetActive(true);
        if (PausePanel != null) PausePanel.SetActive(false);
        if (SettingPanel != null) SettingPanel.SetActive(false);
        if (DeathScreen != null) DeathScreen.SetActive(false);

        // 初始化圖片顯示狀態
        if (OperationImage != null) OperationImage.SetActive(false);
        if (GameplayImage != null) GameplayImage.SetActive(false);

        // 設置主畫面的第一個按鈕（如果存在）
        if (mainMenuFirstButton != null) SetFirstSelectedButton(mainMenuFirstButton);
    }

    private void OnDestroy()
    {
        // 確保取消事件訂閱
        playerInput.actions["Pause"].performed -= OnPausePerformed;
    }

    // 暫停鍵觸發時的回調
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        // 如果死亡畫面激活，不執行暫停邏輯
        if (isDeathScreenActive || PausePanel == null) return;
        TogglePause();
    }

    // 切換遊戲暫停狀態
    public void TogglePause()
    {
        if (PausePanel == null) return; // 如果暫停面板不存在，直接返回

        isPause = !isPause;

        if (isPause)
        {
            PausePanel.SetActive(true);
            if (MainPanel != null) MainPanel.SetActive(false); // 關閉主畫面
            Time.timeScale = 0f;

            // 設置暫停面板的第一個按鈕
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

    // 打開設置面板
    public void EnterSettings()
    {
        if (SettingPanel == null) return; // 如果設置面板不存在，直接返回

        if (MainPanel != null) MainPanel.SetActive(false);   // 關閉主畫面
        if (PausePanel != null) PausePanel.SetActive(false); // 關閉暫停面板
        SettingPanel.SetActive(true); // 打開設置面板

        // 設置設置面板的第一個按鈕
        if (settingsFirstButton != null) SetFirstSelectedButton(settingsFirstButton);
    }

    // 返回主畫面
    public void ReturnToMainPanel()
    {
        if (MainPanel == null) return; // 如果主畫面不存在，直接返回

        if (SettingPanel != null) SettingPanel.SetActive(false);
        if (PausePanel != null) PausePanel.SetActive(false);
        MainPanel.SetActive(true);

        // 設置主畫面的第一個按鈕
        if (mainMenuFirstButton != null) SetFirstSelectedButton(mainMenuFirstButton);
    }

    // 顯示操作圖片，關閉玩法圖片
    public void ShowOperationImage()
    {
        if (OperationImage != null) OperationImage.SetActive(true);
        if (GameplayImage != null) GameplayImage.SetActive(false);
    }

    // 顯示玩法圖片，關閉操作圖片
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
            eventSystem.SetSelectedGameObject(null); // 清除當前選擇
            eventSystem.SetSelectedGameObject(button.gameObject); // 設置新的選中按鈕
        }
    }

    // 重新開始遊戲
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 回到主畫面
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
        Debug.Log("Game is quitting...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
