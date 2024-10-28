using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public GameObject portal;  // 傳送門對象
    public int requiredKills = 15;  // 開啟傳送門所需的擊殺數
    private int killCount = 0;  // 當前擊殺數
    private bool canEnterPortal = false;  // 是否能夠進入傳送門

    void Start()
    {
        // 傳送門一開始可以顯示，但不允許進入
        portal.SetActive(true);  // 確保傳送門從一開始就顯示
    }

    // 增加擊殺數
    public void AddKill()
    {
        killCount++;
        Debug.Log("擊殺數: " + killCount);

        if (killCount >= requiredKills)
        {
            EnablePortalEntry();
        }
    }

    // 允許進入傳送門
    void EnablePortalEntry()
    {
        Debug.Log("傳送門現在可以進入！");
        canEnterPortal = true;  // 當擊殺數達到要求，允許進入傳送門
    }

    // 玩家進入傳送門後觸發下一關
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canEnterPortal)
        {
            Debug.Log("進入傳送門，進入下一關！");
            LoadNextLevel();
        }
    }

    // 加載下一個場景
    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
