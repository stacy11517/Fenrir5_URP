using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;  // 攻击力

    void OnTriggerEnter(Collider other)
    {
        // 检查碰撞对象是否是敌人
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            // 敌人扣血
            enemyHealth.TakeDamage(attackDamage);
        }
    }
}
