using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public int requiredKills = 15;          // 開啟傳送門所需的擊殺數
    private int killCount = 0;              // 當前擊殺數
    private bool canEnterPortal = false;    // 是否能夠進入傳送門
    public string nextSceneName;            // 下一關卡的名稱（可選）

    public LevelLoader levelLoader;

    void Start()
    {
        canEnterPortal = false;
        killCount = 0;
    }


    /// 增加擊殺數
    public void AddKill()
    {
        killCount++;
        Debug.Log("擊殺數: " + killCount);

        if (killCount >= requiredKills)
        {
            Debug.Log("傳送門現在可以進入！");
            canEnterPortal = true;
        }
    
    }

    /// <summary>
    /// 當玩家進入傳送門的範圍時觸發
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canEnterPortal==true)
        {
            Debug.Log("開啟傳送門，進入下一關！");
            levelLoader.LoadNextLevel();
        }
    }

}
