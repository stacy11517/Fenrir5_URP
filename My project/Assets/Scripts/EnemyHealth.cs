using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public PortalManager portalManager;  // PortalManager 的引用
    public Animator animator;  // Animator 用於控制動畫
    public bool isDead = false;  // 用於判斷敵人是否已死亡
    public string deathAnimationName = "Die";  // 死亡動畫名稱
    public float deathDelay = 0.5f;  // 在動畫結束後等待的時間 (防止動畫播放不完整)

    void Start()
    {
        currentHealth = maxHealth;
        if (animator == null)
        {
            animator = GetComponent<Animator>();  // 獲取 Animator 元件
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;  // 如果已經死亡，不再扣血

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 顯示當前血量
        Debug.Log("敵人當前血量: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 處理敵人死亡
    void Die()
    {
        if (isDead) return;  // 防止重複執行死亡邏輯

        isDead = true;
        Debug.Log("敵人死亡！");

        // 觸發死亡動畫
        if (animator != null)
        {
            animator.SetTrigger(deathAnimationName);
        }

        // 告訴 PortalManager 擊殺數增加
        if (portalManager != null)
        {
            portalManager.AddKill();
        }

        // 開始等待動畫播放完畢後銷毀物件
        StartCoroutine(DestroyAfterDeathAnimation());
    }

    // 等待死亡動畫播放完畢後銷毀物件
    IEnumerator DestroyAfterDeathAnimation()
    {
        // 等待動畫時間 + 一點延遲
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + deathDelay);

        // 銷毀物件
        Destroy(gameObject);
    }
}
