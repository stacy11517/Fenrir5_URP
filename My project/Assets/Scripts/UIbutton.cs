using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIbutton : MonoBehaviour
{
    public GameObject MainPanel;        // 主畫面 UI 的物件
    public GameObject PausePanel;       // 暫停 UI 的物件
    public GameObject SettingPanel;     // 設置 UI 的物件
    public GameObject DeathScreen;      // 死亡畫面 UI 的物件

    private int currentButtonIndex = 0; // 追踪當前選中的按鈕索引
    private bool isPause = false;       // 控制遊戲是否暫停
    private bool isDeathScreenActive = false;  // 死亡畫面是否顯示
    private EventSystem eventSystem;    // 控制按鈕選擇的 EventSystem

    private void Start()
    {
        // 初始化面板顯示狀態
        MainPanel.SetActive(true);        // 開始時顯示主畫面
        PausePanel.SetActive(false);      // 開始時隱藏暫停畫面
        SettingPanel.SetActive(false);    // 開始時隱藏設置畫面
        DeathScreen.SetActive(false);     // 開始時隱藏死亡畫面

        eventSystem = EventSystem.current; // 取得 EventSystem

        // 初始時選中主畫面的第一個按鈕
        SelectButton(currentButtonIndex, GetActiveButtons());
    }

    private void Update()
    {
        if (isDeathScreenActive)
        {
            HandleMenuInput(); // 處理死亡畫面按鈕選擇
        }
        else if (isPause)
        {
            HandleMenuInput(); // 處理暫停畫面按鈕選擇
        }
        else
        {
            // 檢查是否按下 joystick button 7（通常是 Start 按鈕）或 ESC 鍵來顯示或隱藏暫停 UI
            if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }

            // 檢查是否按下 `T` 鍵進入當前場景的下一關
            if (Input.GetKeyDown(KeyCode.T))
            {
                EnterNextLevel();
            }
        }
    }

    // 切換遊戲暫停狀態
    public void TogglePause()
    {
        isPause = !isPause;

        if (isPause)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0f;
            currentButtonIndex = 0; // 重置索引以選中第一個按鈕
            SelectButton(currentButtonIndex, GetActiveButtons()); // 顯示暫停 UI 時自動選擇當前顯示的按鈕
        }
        else
        {
            PausePanel.SetActive(false);
            SettingPanel.SetActive(false);
            MainPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // 顯示死亡畫面
    public void ShowDeathScreen()
    {
        isDeathScreenActive = true;
        DeathScreen.SetActive(true);
        Time.timeScale = 0f; // 暫停遊戲時間
        currentButtonIndex = 0; // 重置索引以選中第一個按鈕
        SelectButton(currentButtonIndex, GetActiveButtons()); // 顯示死亡 UI 時自動選擇按鈕
    }

    // 處理當前顯示面板中的按鈕選擇
    void HandleMenuInput()
    {
        // 獲取當前最上層 active = true 的按鈕
        Button[] activeButtons = GetActiveButtons();

        // 使用上下鍵來切換選中的按鈕
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentButtonIndex--;
            if (currentButtonIndex < 0)
            {
                currentButtonIndex = activeButtons.Length - 1;
            }
            SelectButton(currentButtonIndex, activeButtons);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentButtonIndex++;
            if (currentButtonIndex >= activeButtons.Length)
            {
                currentButtonIndex = 0;
            }
            SelectButton(currentButtonIndex, activeButtons);
        }

        // 使用回車鍵或 joystick button 0 來按下選中的按鈕
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            activeButtons[currentButtonIndex].onClick.Invoke();
        }
    }

    // 選擇指定索引的按鈕
    void SelectButton(int index, Button[] activeButtons)
    {
        if (activeButtons.Length == 0) return;

        // 確保 EventSystem 正確選中按鈕
        eventSystem.SetSelectedGameObject(null); // 清除之前的選擇狀態
        eventSystem.SetSelectedGameObject(activeButtons[index].gameObject); // 選擇新的按鈕
    }

    // 取得當前最上層 active = true 的面板中的按鈕
    Button[] GetActiveButtons()
    {
        List<Button> activeButtons = new List<Button>();

        // 遍歷每個面板，只選擇最上層且 active = true 的按鈕
        if (DeathScreen.activeSelf)
        {
            AddActiveButtonsFromPanel(DeathScreen, activeButtons);
        }
        else if (SettingPanel.activeSelf)
        {
            AddActiveButtonsFromPanel(SettingPanel, activeButtons);
        }
        else if (PausePanel.activeSelf)
        {
            AddActiveButtonsFromPanel(PausePanel, activeButtons);
        }
        else if (MainPanel.activeSelf)
        {
            AddActiveButtonsFromPanel(MainPanel, activeButtons);
        }

        return activeButtons.ToArray();
    }

    // 將特定面板中的 active = true 的按鈕添加到 activeButtons 中
    void AddActiveButtonsFromPanel(GameObject panel, List<Button> activeButtons)
    {
        Button[] panelButtons = panel.GetComponentsInChildren<Button>(true);
        foreach (Button button in panelButtons)
        {
            if (button.gameObject.activeSelf)
            {
                activeButtons.Add(button);
            }
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
        PausePanel.SetActive(false);
        Time.timeScale = 1f; // 恢復遊戲時間
    }

    // 進入設置面板
    public void EnterSettings()
    {
        PausePanel.SetActive(false);    // 關閉暫停畫面
        SettingPanel.SetActive(true);   // 開啟設置畫面
        currentButtonIndex = 0;
        SelectButton(currentButtonIndex, GetActiveButtons()); // 設置面板開啟後，立即選中按鈕
    }

    // 回到主面板
    public void ReturnToMainPanel()
    {
        SettingPanel.SetActive(false);   // 關閉設置畫面
        MainPanel.SetActive(true);       // 開啟主畫面
        currentButtonIndex = 0;
        SelectButton(currentButtonIndex, GetActiveButtons()); // 主面板開啟後，立即選中按鈕
    }

    // 返回主畫面 (新增功能)
    public void BackToMainPanelFromPauseOrSetting()
    {
        PausePanel.SetActive(false);
        SettingPanel.SetActive(false);
        MainPanel.SetActive(true);
        currentButtonIndex = 0;
        SelectButton(currentButtonIndex, GetActiveButtons());
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
