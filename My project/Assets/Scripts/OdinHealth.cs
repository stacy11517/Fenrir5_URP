using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OdinHealth : MonoBehaviour
{
    public int maxHealth = 500;          // 奧丁的最大血量
    public int currentHealth;           // 奧丁的當前血量
    public Image healthBarFill;         // 血量條 UI 使用 Image 的 fillAmount
    public GameObject deathEffect;      // 死亡特效
    public Animator animator;           // 動畫控制器
    public float deathDelay = 5f;       // 死亡後延遲刪除物件的時間

    private bool isDead = false;        // 是否已經死亡

    void Start()
    {
        currentHealth = maxHealth;      // 初始化血量
        UpdateHealthUI();               // 更新血量 UI

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // 扣血方法
    public void TakeDamage(int damage)
    {
        if (isDead) return;             // 如果已死亡，跳過邏輯

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 限制血量範圍
        UpdateHealthUI();               // 更新血量 UI

        Debug.Log("Odin took damage: " + damage + ". Current health: " + currentHealth);

        // 播放受傷動畫
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        // 如果血量歸零，執行死亡邏輯
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 更新血量 UI
    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth; // 根據當前血量更新 fillAmount
        }
    }

    // 死亡邏輯
    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Odin has died!");

        // 播放死亡動畫
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 生成死亡特效
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // 延遲刪除物件
        StartCoroutine(DestroyAfterDelay());
    }

    // 延遲刪除物件的協程
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }

    // 測試用的鍵盤輸入方法
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // 測試時，按 H 鍵減少 50 血量
        {
            TakeDamage(50);
        }
    }
}
