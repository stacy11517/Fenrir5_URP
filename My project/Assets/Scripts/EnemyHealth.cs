using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;    // 敌人最大生命值
    public int currentHealth;     // 敌人当前生命值

    void Start()
    {
        // 初始化敌人生命值
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // 扣除生命值
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Enemy took damage! Current health: " + currentHealth);

        // 检查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 敌人死亡处理逻辑
        Debug.Log("Enemy died!");
        // 可以在这里添加死亡动画、销毁对象等
        Destroy(gameObject);
    }
}
