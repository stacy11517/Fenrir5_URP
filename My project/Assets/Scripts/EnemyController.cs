using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;           // 玩家位置
    private NavMeshAgent agent;        // NavMeshAgent 組件
    public float chaseDistance = 10f;  // 追蹤的最大距離
    public float attackDistance = 5f;  // 近戰攻擊距離
    public float attackCooldown = 0.5f; // 攻擊冷卻時間
    private bool canAttack = true;     // 是否可以攻擊

    private Animator animator;         // Animator 組件
    public int maxHealth = 50;         // 最大血量
    public int currentHealth;          // 當前血量
    public PortalManager portalManager; // PortalManager 的引用
    public float deathDelay = 10f;      // 死亡延遲時間
    public bool isDead = false;       // 是否死亡標記

    public PlayerHealth playerHealth;  // 玩家血量系統
    public int attackDamage = 10;      // 攻擊傷害

    public ParticleSystem hurtEffect;  // 受傷特效

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerHealth = playerObj.GetComponent<PlayerHealth>();
            }
        }

        if (player == null || playerHealth == null)
        {
            Debug.LogWarning("未找到玩家或 PlayerHealth 組件，請檢查場景設置！");
        }
    }

    void Update()
    {
        if (isDead)
        {
            agent.enabled = false; // 禁用 NavMeshAgent
            animator.SetBool("isWalking", false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance && canAttack)
        {
            StartCoroutine(PerformAttack());
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            agent.isStopped = false;
            agent.destination = player.position;
            animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            animator.SetBool("isWalking", false);
        }
    }

    IEnumerator PerformAttack()
    {
        canAttack = false;
        agent.isStopped = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        agent.isStopped = false;
    }

    public void ApplyDamage()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackDistance)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("敵人攻擊玩家，造成 " + attackDamage + " 點傷害");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (animator != null) animator.SetTrigger("Hurt");
        if (hurtEffect != null) Instantiate(hurtEffect, transform.position, Quaternion.identity).Play();

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");

        if (portalManager != null) portalManager.AddKill();
        Destroy(gameObject, deathDelay);
    }
}
