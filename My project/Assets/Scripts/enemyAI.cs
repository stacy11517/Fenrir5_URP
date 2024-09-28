using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    public Transform player;       // 玩家目標
    public float detectionRadius = 5f;  // 玩家檢測範圍
    public float wanderRadius = 10f;    // 隨機遊走範圍
    public float wanderTime = 5f;   // 遊走時間間隔

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTime;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer > detectionRadius)
        {
            // 玩家在範圍外，隨機遊走
            timer += Time.deltaTime;

            if (timer >= wanderTime)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
        else
        {
            // 玩家在範圍內，停止遊走或執行其他動作
            agent.SetDestination(transform.position);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
