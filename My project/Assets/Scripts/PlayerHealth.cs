using UnityEngine;
using UnityEngine.UI;          // 用於 UI 操作
using TMPro;                 // 用於 TextMeshPro
using UnityEngine.SceneManagement;  // 用於場景管理

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;          // 玩家最大生命值
    public int currentHealth;            // 玩家當前生命值
    public int healthPackCount = 0;      // 補血包的數量
    public Image healthBar;              // 血條 UI
    public TMP_Text healthPackText;      // 血量包數量的 TextMeshPro 元件
    public GameObject deathScreen;       // 死亡畫面 UI
    private bool isDead = false;         // 用於檢查玩家是否已死亡

    void Start()
    {
        // 初始化玩家生命值
        currentHealth = maxHealth;
        UpdateHealthBar();              // 初始化血條
        UpdateHealthPackText();         // 初始化補血包數量文本
        deathScreen.SetActive(false);   // 隱藏死亡畫面 UI
    }

    void Update()
    {
        if (isDead)
        {
            // 如果玩家死亡，任何按鍵都可以重新開始遊戲
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0); // 回到場景編號為 0 的場景
            }
        }
        else
        {
            // 按下 RB 鍵（手把）或 F 鍵（鍵盤）來使用補血包
            if ((Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.F)) && healthPackCount > 0)
            {
                UseHealthPack();
            }
        }
    }

    // 恢復生命值的方法
    public void Heal(int amount)
    {
        // 恢復生命值，但不超過最大生命值
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player healed! Current health: " + currentHealth);
        UpdateHealthBar();              // 更新血條
    }

    // 當碰到補血包時觸發
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPack"))
        {
            // 撿起補血包，數量增加
            healthPackCount++;
            Debug.Log("Health pack picked up! Total packs: " + healthPackCount);
            UpdateHealthPackText();       // 更新補血包數量文本

            // 銷毀補血包物件
            Destroy(other.gameObject);
        }
    }

    // 使用補血包恢復生命
    void UseHealthPack()
    {
        if (currentHealth < maxHealth)
        {
            Heal(10);                    // 恢復10點生命值
            healthPackCount--;           // 補血包數量減少
            Debug.Log("Health pack used! Remaining packs: " + healthPackCount);
            UpdateHealthPackText();      // 更新補血包數量文本
        }
        else
        {
            Debug.Log("Health is full, cannot use health pack.");
        }
    }

    // 更新血條顯示
    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBar.fillAmount = healthPercent;  // 調整血條的長度
    }

    // 更新補血包數量文本顯示
    void UpdateHealthPackText()
    {
        healthPackText.text = healthPackCount.ToString();  // 顯示補血包的數量，即使是 0
    }

    // 當玩家受到傷害時調用
    public void TakeDamage(int damage)
    {
        if (isDead) return;  // 如果已經死亡，不再扣血

        // 扣除生命值
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player took damage! Current health: " + currentHealth);
        UpdateHealthBar();              // 更新血條

        // 檢查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 玩家死亡邏輯
    void Die()
    {
        isDead = true; // 標記玩家為已死亡
        Debug.Log("Player died!");

        // 禁用玩家控制
        GetComponent<PlayerController>().enabled = false;

        // 顯示死亡畫面
        deathScreen.SetActive(true);
    }
}
