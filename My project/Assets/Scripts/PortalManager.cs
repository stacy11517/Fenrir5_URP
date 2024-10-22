using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public GameObject portal;  // 傳送門對象
    public int requiredKills = 15;  // 開啟傳送門所需的擊殺數
    private int killCount = 0;  // 當前擊殺數

    void Start()
    {
        portal.SetActive(false);  // 起始時隱藏傳送門
    }

    // 增加擊殺數
    public void AddKill()
    {
        killCount++;
        Debug.Log("擊殺數: " + killCount);

        if (killCount >= requiredKills)
        {
            OpenPortal();
        }
    }

    // 開啟傳送門
    void OpenPortal()
    {
        Debug.Log("傳送門已開啟！");
        portal.SetActive(true);  // 顯示傳送門
    }

    // 玩家進入傳送門後觸發下一關
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && portal.activeSelf)
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
