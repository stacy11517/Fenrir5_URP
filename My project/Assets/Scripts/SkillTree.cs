using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Animator animator;         // Animator 元件引用
    public CameraShake cameraShake;   // CameraShake 引用
    public Transform attackPoint;     // 攻擊範圍起點
    public Transform spinAttackPoint; // 起跳旋轉攻擊的起點

    // 技能冷卻時間
    public float dashCooldown = 1f;
    public float dualAttackCooldown = 2f;
    public float spinAttackCooldown = 3f;

    private bool canDash = true;
    private bool canDualAttack = true;
    private bool canSpinAttack = true;

    // 攻擊範圍及傷害
    public float attackRange = 2f;
    public int attackDamage = 10;
    public int spinAttackDamage = 15;

    void Start()
    {
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();

        if (cameraShake == null)
        {
            Debug.LogError("CameraShake component not found on the main camera.");
        }
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

        // 可選擇在衝刺結束後觸發相機震動
        cameraShake.TriggerShake();
    }

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
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Hit " + enemy.name);
            }
        }

        // 攻擊時觸發相機震動
        cameraShake.TriggerShake();
    }

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

    public void PerformSpinAttack()  // 在 Animation Event 中或手動呼叫
    {
        Collider[] hitEnemies = Physics.OverlapSphere(spinAttackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(spinAttackDamage);
                Debug.Log("Hit " + enemy.name);
            }
        }

        // 攻擊時觸發相機震動
        cameraShake.TriggerShake();
    }

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
