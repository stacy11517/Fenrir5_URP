using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Animator animator;         // Animator 元件引用
    public Transform attackPoint;     // 攻擊範圍起點
    public Transform spinAttackPoint; // 起跳旋轉攻擊的起點

    // 技能冷卻時間
    public float dashCooldown = 1f;             // 衝刺冷卻時間
    public float dualAttackCooldown = 2f;       // 來回攻擊冷卻時間
    public float spinAttackCooldown = 3f;       // 起跳旋轉攻擊冷卻時間

    private bool canDash = true;
    private bool canDualAttack = true;
    private bool canSpinAttack = true;

    public float attackRange = 2f;          // 來回攻擊範圍
    public int attackDamage = 10;           // 來回攻擊傷害值
    public int spinAttackDamage = 15;       // 起跳旋轉攻擊傷害值

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleDash();
        HandleDualAttack();
        HandleSpinAttack();
    }

    // 衝刺技能
    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && canDash)
        {
            animator.SetTrigger("Dash");
            StartCoroutine(PerformDash());
            StartCoroutine(DashCooldown());
        }
    }

    IEnumerator PerformDash()
    {
        float dashTime = 0.5f;
        float dashSpeed = 20f;
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // 設定衝刺冷卻時間
    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // 來回攻擊技能（Animation Event 觸發）
    void HandleDualAttack()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && canDualAttack)
        {
            animator.SetTrigger("DualAttack");
            StartCoroutine(DualAttackCooldown());
        }
    }

    public void PerformDualAttack()  // 在 Animation Event 中呼叫此方法
    {
        // 檢查攻擊範圍內的敵人
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);  // 使敵人扣血
                Debug.Log("Hit " + enemy.name);
            }
        }
    }

    // 設定來回攻擊冷卻時間
    IEnumerator DualAttackCooldown()
    {
        canDualAttack = false;
        yield return new WaitForSeconds(dualAttackCooldown);
        canDualAttack = true;
    }

    // 起跳旋轉攻擊技能
    void HandleSpinAttack()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton2) && canSpinAttack)
        {
            animator.SetTrigger("SpinAttack");
            StartCoroutine(SpinAttackCooldown());
        }
    }

    public void PerformSpinAttack()  // 在 Animation Event 中呼叫
    {
        // 檢查攻擊範圍內的敵人
        Collider[] hitEnemies = Physics.OverlapSphere(spinAttackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(spinAttackDamage);  // 使敵人扣血
                Debug.Log("Hit " + enemy.name);
            }
        }
    }

    // 設定起跳旋轉攻擊冷卻時間
    IEnumerator SpinAttackCooldown()
    {
        canSpinAttack = false;
        yield return new WaitForSeconds(spinAttackCooldown);
        canSpinAttack = true;
    }

    // 視覺化攻擊範圍
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        if (spinAttackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(spinAttackPoint.position, attackRange);
        }
    }
}
