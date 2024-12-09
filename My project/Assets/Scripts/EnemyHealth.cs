using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;                   // 敵人的最大生命值
    public int currentHealth;                    // 敵人當前生命值
    public ParticleSystem hurtEffect;            // 受傷特效
    public float hurtEffectCooldown = 1f;        // 受傷特效的冷卻時間

    private Animator animator;                   // Animator 用於控制動畫
    private bool isDead = false;                 // 是否已經死亡的標記
    private bool canPlayHurtEffect = true;       // 控制是否可以播放受傷特效

    public PortalManager portalManager; // 引用 PortalManager
    public event System.Action OnEnemyDeath;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        if (hurtEffect != null && canPlayHurtEffect)
        {
            Instantiate(hurtEffect, transform.position, Quaternion.identity).Play();
            StartCoroutine(HurtEffectCooldownRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator HurtEffectCooldownRoutine()
    {
        canPlayHurtEffect = false;
        yield return new WaitForSeconds(hurtEffectCooldown);
        canPlayHurtEffect = true;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 通知 PortalManager 增加擊殺數
        if (portalManager != null)
        {
            portalManager.AddKill();
        }

        Destroy(gameObject, 2f); // 延遲銷毀

        // 通知生成器敵人死亡
        OnEnemyDeath?.Invoke();

        Destroy(gameObject, 2f); // 延遲銷毀
    }

    public bool IsDead()
    {
        return isDead;
    }
}
