using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public GameObject portal;                // 傳送門對象
    public int requiredKills = 15;          // 開啟傳送門所需的擊殺數
    private int killCount = 0;              // 當前擊殺數
    private bool canEnterPortal = false;    // 是否能夠進入傳送門
    public string nextSceneName;            // 下一關卡的名稱（可選）

    void Start()
    {
        // 初始化傳送門狀態
        if (portal != null)
        {
            portal.SetActive(false);        // 傳送門在一開始是隱藏的
        }
    }

    /// <summary>
    /// 增加擊殺數
    /// </summary>
    public void AddKill()
    {
        killCount++;
        Debug.Log("擊殺數: " + killCount);

        if (killCount >= requiredKills && !canEnterPortal)
        {
            EnablePortalEntry();
        }
    }

    /// <summary>
    /// 允許進入傳送門
    /// </summary>
    void EnablePortalEntry()
    {
        Debug.Log("傳送門現在可以進入！");
        canEnterPortal = true;

        if (portal != null)
        {
            portal.SetActive(true); // 顯示傳送門
        }
    }

    /// <summary>
    /// 當玩家進入傳送門的範圍時觸發
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canEnterPortal)
        {
            Debug.Log("進入傳送門，進入下一關！");
            LoadNextLevel();
        }
    }

    /// <summary>
    /// 加載下一個場景
    /// </summary>
    void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); // 使用場景名稱加載
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // 加載下一個場景
        }
    }
}
