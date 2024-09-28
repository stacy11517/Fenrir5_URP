using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    public Transform player;       // ���a�ؼ�
    public float detectionRadius = 5f;  // ���a�˴��d��
    public float wanderRadius = 10f;    // �H���C���d��
    public float wanderTime = 5f;   // �C���ɶ����j

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
            // ���a�b�d��~�A�H���C��
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
            // ���a�b�d�򤺡A����C���ΰ����L�ʧ@
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
