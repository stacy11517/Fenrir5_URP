using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10;  // 攻击力

    // 当敌人与其他对象发生碰撞时调用
    void OnCollisionEnter(Collision collision)
    {
        // 获取碰撞对象上的 PlayerHealth 组件
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // 玩家扣血
            playerHealth.TakeDamage(attackDamage);
        }
    }

}
