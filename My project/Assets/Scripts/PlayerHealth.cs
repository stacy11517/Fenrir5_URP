using UnityEngine;
using UnityEngine.UI;          // 用於 UI 操作
using TMPro;                 // 用於 TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;          // 玩家最大生命值
    public int currentHealth;             // 玩家當前生命值
    public int healthPackCount = 0;       // 補血包的數量
    public Image healthBar;                // 血條 UI
    public TMP_Text healthPackText;        // 血量包數量的 TextMeshPro 元件

    void Start()
    {
        // 初始化玩家生命值
        currentHealth = maxHealth;
        UpdateHealthBar();              // 初始化血條
        UpdateHealthPackText();         // 初始化補血包數量文本
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

    // 使用補血包
    void Update()
    {
        // 按下 RB 鍵來使用補血包
        if ((Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.F)) && healthPackCount > 0)
        {
            UseHealthPack();
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
        healthPackText.text = healthPackCount > 0 ? healthPackCount.ToString() : "";  // 當數量大於 0 時顯示數字，否則不顯示
    }
    public void TakeDamage(int damage)
    {
        // 扣除生命值
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player took damage! Current health: " + currentHealth);

        // 检查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 玩家死亡处理逻辑
        Debug.Log("Player died!");
        // 可以在这里添加死亡动画、重生逻辑等
    }
}
