using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIbutton : MonoBehaviour
{
    public GameObject PauseUI;        // 暫停 UI 的物件
    public GameObject SettingUI;      // 設置 UI 的物件
    public GameObject DeathScreen;    // 死亡畫面 UI
    public Button[] buttons;          // 所有按鈕的陣列，包括暫停畫面和死亡畫面的按鈕

    private int currentButtonIndex = 0; // 追踪當前選中的按鈕索引
    private bool isPause = false;       // 控制遊戲是否暫停
    private bool isDeathScreenActive = false;  // 死亡畫面是否顯示
    private EventSystem eventSystem;    // 控制按鈕選擇的 EventSystem

    private void Start()
    {
        PauseUI.SetActive(false);     // 開始時隱藏暫停 UI
        SettingUI.SetActive(false);   // 開始時隱藏設置 UI
        DeathScreen.SetActive(false); // 開始時隱藏死亡 UI
        eventSystem = EventSystem.current; // 取得 EventSystem
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
            // 檢查是否按下 joystick button 7（通常是 Start 按鈕）來顯示或隱藏暫停 UI
            if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }

    // 切換遊戲暫停狀態
    public void TogglePause()
    {
        isPause = !isPause;

        if (isPause)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0f;
            currentButtonIndex = 0; // 重置索引以選中第一個按鈕
            SelectButton(currentButtonIndex, GetActiveButtons()); // 顯示暫停 UI 時自動選擇當前顯示的按鈕
        }
        else
        {
            PauseUI.SetActive(false);
            SettingUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // 處理暫停和死亡畫面的按鈕選擇
    void HandleMenuInput()
    {
        // 獲取當前顯示的按鈕
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
        // 確保 EventSystem 正確選中按鈕
        eventSystem.SetSelectedGameObject(null); // 清除之前的選擇狀態
        eventSystem.SetSelectedGameObject(activeButtons[index].gameObject); // 選擇新的按鈕
    }

    // 取得當前 active = true 的按鈕
    Button[] GetActiveButtons()
    {
        List<Button> activeButtons = new List<Button>();
        foreach (Button button in buttons)
        {
            if (button.gameObject.activeSelf)  // 只選擇 active 為 true 的按鈕
            {
                activeButtons.Add(button);
            }
        }
        return activeButtons.ToArray();  // 返回一個 active 按鈕的陣列
    }

    // 重新開始遊戲
    public void RestartGame()
    {
        Time.timeScale = 1f; // 恢復遊戲時間
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 重新加載當前場景
    }

    // 回到主畫面
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // 恢復遊戲時間
        SceneManager.LoadScene(0); // 加載主畫面 (場景編號 0)
    }

    // 其他按鈕的功能
    public void EnterLobbyList()
    {
        SceneManager.LoadScene(1);  // 進入選擇關卡大廳
    }

    public void EnterLevel1()
    {
        SceneManager.LoadScene(2);  // 進入第一關
    }

    public void EnterLevel2()
    {
        SceneManager.LoadScene(3);  // 進入第二關
    }

    public void EnterLevel3()
    {
        SceneManager.LoadScene(4);  // 進入第三關
    }

    public void Setting()
    {
        SettingUI.SetActive(true);
    }

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
