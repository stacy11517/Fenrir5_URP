using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;                   // 敵人的最大生命值
    public int currentHealth;                    // 敵人當前生命值
    public PortalManager portalManager;          // PortalManager 的引用，用於增加擊殺數
    public Animator animator;                    // Animator 用於控制動畫
    public ParticleSystem hurtEffect;            // 受傷特效
    public float deathDelay = 10f;               // 死亡後延遲 10 秒銷毀物件
    public float hurtEffectCooldown = 1f;        // 受傷特效的冷卻時間

    public bool isDead = false;                  // 是否已經死亡的標記

    public Transform headTransform;              // 敵人的頭部位置，用於顯示血量條
    public Canvas healthBarCanvas;               // 血量條的 Canvas
    public Image healthFillImage;                // 血量條的填充圖片
    public Vector3 healthBarOffset = new Vector3(0, 2, 0); // 血量條偏移

    private ParticleSystem instantiatedHurtEffect; // 預生成的受傷特效
    private bool canPlayHurtEffect = true;         // 控制是否可以播放受傷特效
    private Camera mainCamera;                     // 主攝影機

    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
        {
            animator = GetComponent<Animator>();  // 獲取 Animator 元件
        }

        // 預生成受傷特效
        if (hurtEffect != null)
        {
            instantiatedHurtEffect = Instantiate(hurtEffect, transform.position, Quaternion.identity);
            instantiatedHurtEffect.Stop(); // 初始時先停止特效
        }

        // 初始化血量條
        if (healthBarCanvas != null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // 更新血量條的位置和朝向
        if (healthBarCanvas != null && headTransform != null)
        {
            healthBarCanvas.transform.position = headTransform.position + healthBarOffset; // 跟隨頭部
            healthBarCanvas.transform.LookAt(mainCamera.transform); // 朝向攝影機
            healthBarCanvas.transform.Rotate(0, 180, 0); // 修正方向
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;  // 如果已經死亡，不再扣血

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 更新血量條
        UpdateHealthBar();

        // 顯示當前血量
        Debug.Log("敵人當前血量: " + currentHealth);

        // 播放受傷動畫
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        // 播放受傷特效（如果冷卻時間已經過去）
        if (instantiatedHurtEffect != null && canPlayHurtEffect)
        {
            instantiatedHurtEffect.transform.position = transform.position; // 更新特效的位置
            instantiatedHurtEffect.Play();
            StartCoroutine(HurtEffectCooldownRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 更新血量條
    void UpdateHealthBar()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = (float)currentHealth / maxHealth; // 根據血量更新填充比例
        }
    }

    // 冷卻協程，用於控制受傷特效的播放頻率
    IEnumerator HurtEffectCooldownRoutine()
    {
        canPlayHurtEffect = false;
        yield return new WaitForSeconds(hurtEffectCooldown);
        canPlayHurtEffect = true;
    }

    void Die()
    {
        if (isDead) return;  // 防止重複執行死亡邏輯

        isDead = true;
        Debug.Log("敵人死亡！");

        // 隱藏血量條
        if (healthBarCanvas != null)
        {
            healthBarCanvas.enabled = false;
        }

        // 觸發死亡動畫
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 告訴 PortalManager 擊殺數增加
        if (portalManager != null)
        {
            portalManager.AddKill();
        }

        // 在死亡動畫播放完畢後銷毀物件
        StartCoroutine(DestroyAfterDeath());
    }

    IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(deathDelay); // 等待死亡延遲時間
        Destroy(gameObject); // 銷毀敵人遊戲物件
    }
}
