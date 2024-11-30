using System.Collections;
using UnityEngine;

public class OdinBoss : MonoBehaviour
{
    public Transform player; // 玩家位置
    public GameObject lightningStrikePrefab; // 地圖雷擊特效預製件
    public GameObject thunderBallPrefab; // 雷球特效預製件
    public Transform throwPoint; // 雷球丟出的起點
    public float lightningCooldown = 5f; // 地圖雷擊技能的冷卻時間
    public float thunderBallCooldown = 7f; // 雷球技能的冷卻時間
    public float lightningRange = 10f; // 雷擊攻擊的範圍
    public float thunderBallSpeed = 15f; // 雷球的飛行速度
    public int lightningDamage = 20; // 雷擊的傷害
    public int thunderBallDamage = 50; // 雷球的傷害

    private bool canUseLightning = true;
    private bool canThrowThunderBall = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canUseLightning)
        {
            StartCoroutine(UseLightningStrike());
        }

        if (canThrowThunderBall)
        {
            StartCoroutine(ThrowThunderBall());
        }
    }

    // 地圖雷擊技能
    IEnumerator UseLightningStrike()
    {
        canUseLightning = false;
        animator.SetTrigger("UseLightning"); // 播放雷擊動畫

        yield return new WaitForSeconds(1f); // 等待動畫前置部分

        // 確定雷擊攻擊的隨機位置，範圍為奧丁周圍
        Vector3 randomPosition = new Vector3(
            transform.position.x + Random.Range(-lightningRange, lightningRange),
            transform.position.y,
            transform.position.z + Random.Range(-lightningRange, lightningRange)
        );

        // 生成雷擊特效
        Instantiate(lightningStrikePrefab, randomPosition, Quaternion.identity);

        // 檢查玩家是否在雷擊範圍內，並扣血
        float distanceToPlayer = Vector3.Distance(randomPosition, player.position);
        if (distanceToPlayer <= 3f) // 假設雷擊範圍半徑為3單位
        {
            player.GetComponent<PlayerHealth>().TakeDamage(lightningDamage);
            Debug.Log("Odin used Lightning Strike! Player took " + lightningDamage + " damage.");
        }

        yield return new WaitForSeconds(lightningCooldown);
        canUseLightning = true;
    }

    // 雷球攻擊技能
    IEnumerator ThrowThunderBall()
    {
        canThrowThunderBall = false;
        animator.SetTrigger("ThrowThunderBall"); // 播放雷球攻擊動畫

        yield return new WaitForSeconds(0.5f); // 等待動畫前置部分

        // 創建雷球並設置其方向朝向玩家
        GameObject thunderBall = Instantiate(thunderBallPrefab, throwPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - throwPoint.position).normalized;
        Rigidbody rb = thunderBall.GetComponent<Rigidbody>();
        rb.velocity = direction * thunderBallSpeed;

        // 設置雷球飛行時間
        Destroy(thunderBall, 5f);

        yield return new WaitForSeconds(thunderBallCooldown);
        canThrowThunderBall = true;
    }

    // 當雷球碰撞到玩家時觸發傷害
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.name.Contains("ThunderBall"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(thunderBallDamage);
                Debug.Log("Player hit by Thunder Ball! Took " + thunderBallDamage + " damage.");
            }

            // 銷毀雷球
            Destroy(collision.gameObject);
        }
    }
}
