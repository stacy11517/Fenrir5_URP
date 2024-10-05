using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;  // 攻击力
    public Collider attackCollider; // 攻擊部位的 Collider (例如爪子或嘴巴)

    void Start()
    {
        // 初始時關閉攻擊的 Collider
        attackCollider.enabled = false;
    }
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
