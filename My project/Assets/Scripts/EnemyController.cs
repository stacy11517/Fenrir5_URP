using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // 追蹤玩家部分
    public Transform player;           // 玩家位置
    private NavMeshAgent agent;        // NavMeshAgent 組件
    public float chaseDistance = 10f;  // 追蹤的最大距離
    public float attackDistance = 2f;  // 近戰攻擊距離
    public float attackCooldown = 1.5f; // 攻擊冷卻時間
    private bool canAttack = true;     // 是否可以攻擊

    // 血量和死亡部分
    public int maxHealth = 50;         // 敵人的最大血量
    public int currentHealth;          // 敵人當前血量
    public PortalManager portalManager; // PortalManager 的引用
    public float deathDelay = 2f;      // 死亡後延遲多少時間銷毀物件
    private bool isDead = false;       // 是否已經死亡的標記

    // 玩家引用（需要實現玩家受傷邏輯）
    public PlayerHealth playerHealth;  // 玩家血量系統引用
    public int attackDamage = 10;      // 攻擊傷害

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 初始化生命值
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isDead)  // 只有在敵人還活著的時候才進行追蹤和攻擊
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // 如果在攻擊範圍內，進行攻擊
            if (distanceToPlayer <= attackDistance && canAttack)
            {
                StartCoroutine(PerformAttack());
            }
            // 如果在追蹤範圍內但不在攻擊範圍，繼續追蹤玩家
            else if (distanceToPlayer <= chaseDistance)
            {
                agent.destination = player.position;
            }
            else
            {
                agent.ResetPath();
            }
        }
    }

    // 進行近戰攻擊
    IEnumerator PerformAttack()
    {
        canAttack = false;
        agent.isStopped = true; // 停止追蹤玩家以便攻擊

        // 模擬攻擊延遲 (等待攻擊動畫前置部分，例如手臂抬起的時間)
        yield return new WaitForSeconds(0.5f);

        // 檢查玩家是否在攻擊範圍內，並扣血
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackDistance)
        {
            playerHealth.TakeDamage(attackDamage);  // 將傷害作用到玩家
            Debug.Log("敵人攻擊玩家，造成 " + attackDamage + " 點傷害");
        }

        // 等待攻擊冷卻時間
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        agent.isStopped = false; // 繼續追蹤玩家
    }

    // 處理敵人受到傷害
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

        // 告訴 PortalManager 擊殺數增加
        if (portalManager != null)
        {
            portalManager.AddKill();
        }

        //死亡動畫交由EnemyHealth處裡
    }
}
