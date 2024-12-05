using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;               // 玩家位置
    private NavMeshAgent agent;            // NavMeshAgent 組件
    public float chaseDistance = 15f;      // 追蹤的最大距離
    public float attackDistance = 2f;      // 近戰攻擊距離
    public float attackCooldown = 1f;      // 攻擊冷卻時間
    private bool canAttack = true;         // 是否可以攻擊

    private Animator animator;             // Animator 組件
    public int attackDamage = 10;          // 攻擊傷害
    public LayerMask playerLayer;          // 玩家層，用於檢測範圍內是否有玩家

    private EnemyHealth enemyHealth;       // 引用 EnemyHealth

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.IsDead())
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance && canAttack)
        {
            PerformAttack();
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.destination = player.position;
        animator.SetBool("isWalking", true);
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
    }

    void PerformAttack()
    {
        canAttack = false;
        agent.isStopped = true;
        animator.SetTrigger("Attack");

        // 開始冷卻計時
        Invoke(nameof(ResetAttackCooldown), attackCooldown);
    }

    void ResetAttackCooldown()
    {
        canAttack = true;
        agent.isStopped = false;
    }

    // 動畫事件觸發的傷害邏輯
    public void DealDamage()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}