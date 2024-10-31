using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public PortalManager portalManager;  // PortalManager 的引用
    public Animator animator;  // Animator 用於控制動畫
    public CameraShake cameraShake;  // CameraShake 引用
    public float deathDelay = 2f;  // 死亡後延遲多少時間銷毀物件

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (animator == null)
        {
            animator = GetComponent<Animator>();  // 獲取 Animator 元件
        }

        if (cameraShake == null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();  // 獲取 CameraShake 元件
            if (cameraShake == null)
            {
                Debug.LogError("CameraShake component not found on the main camera.");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;  // 如果已經死亡，不再扣血

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 啟動 CameraShake
        if (cameraShake != null)
        {
            cameraShake.TriggerShake();
        }

        Debug.Log("敵人當前血量: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;  // 防止重複執行死亡邏輯

        isDead = true;
        Debug.Log("敵人死亡！");

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
        Destroy(gameObject, deathDelay);
    }
}
