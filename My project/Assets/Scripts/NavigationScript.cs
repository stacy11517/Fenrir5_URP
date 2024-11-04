using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{
    public Transform player;           // 玩家位置
    private NavMeshAgent agent;        // NavMeshAgent 组件
    public float chaseDistance = 10f;  // 追踪的最大距离
    private Animator animator;         // Animator 组件
    private EnemyHealth enemyHealth;   // 引用 EnemyHealth 脚本來檢查死亡狀態

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // 获取 Animator 组件
        enemyHealth = GetComponent<EnemyHealth>(); // 获取 EnemyHealth 组件
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.isDead)
        {
            // 停止追踪并禁用 NavMeshAgent
            agent.isStopped = true;
            agent.ResetPath(); // 停止所有導航行為
            animator.SetBool("isWalking", false); // 停止走路動畫
            return; // 如果敌人已经死亡，退出 Update 方法
        }

        // 计算敌人和玩家之间的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 如果距离小于设定的追踪距离，敌人开始追踪并播放走路动画
        if (distanceToPlayer <= chaseDistance)
        {
            agent.destination = player.position;  // 设定目标为玩家的位置
            animator.SetBool("isWalking", true);  // 播放走路动画
        }
        else
        {
            agent.ResetPath();                    // 超过距离时停止追踪
            animator.SetBool("isWalking", false); // 停止走路动画
        }
    }
}
