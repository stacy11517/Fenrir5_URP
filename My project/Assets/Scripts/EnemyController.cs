using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;               // 玩家位置
    private NavMeshAgent agent;            // NavMeshAgent 组件
    public float chaseDistance = 15f;      // 追踪的最大距离
    public float attackDistance = 2f;      // 近战攻击距离
    public float attackCooldown = 1f;      // 攻击冷却时间
    private bool canAttack = true;         // 是否可以攻击

    private Animator animator;             // Animator 组件
    public int attackDamage = 10;          // 攻击伤害
    public float attackRadius = 1.5f;      // 攻击范围半径
    public float attackAngle = 60f;        // 攻击有效角度
    public LayerMask playerLayer;          // 玩家层，用于检测范围内是否有玩家

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
            else
            {
                Debug.LogError("未找到玩家，请确保场景中存在带有 'Player' 标签的对象！");
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

        // 冷却计时
        Invoke(nameof(ResetAttackCooldown), attackCooldown);
    }

    void ResetAttackCooldown()
    {
        canAttack = true;
        agent.isStopped = false;
    }

    /// <summary>
    /// 动画事件触发的伤害逻辑
    /// </summary>
    public void DealDamage()
    {
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);
        foreach (Collider target in hitTargets)
        {
            Transform targetTransform = target.transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= attackAngle / 2)
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
