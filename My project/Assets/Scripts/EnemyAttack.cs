using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10;  // 攻击力

    void OnTriggerEnter(Collider other)
    {
        // 检查碰撞对象是否是玩家
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // 玩家扣血
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
