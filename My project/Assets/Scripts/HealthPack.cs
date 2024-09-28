using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healAmount = 20;  // 補血包提供的生命值

    void OnTriggerEnter(Collider other)
    {
        // 檢查碰撞對象是否是玩家
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // 如果是玩家，恢復生命值
            playerHealth.Heal(healAmount);

            // 摧毀補血包
            Destroy(gameObject);
        }
    }
}
