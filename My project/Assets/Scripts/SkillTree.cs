using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Animator animator;         // Animator 元件引用
    public CameraShake cameraShake;   // CameraShake 引用

    // 用來控制技能冷卻時間
    public float dashCooldown = 1f;
    public float roarCooldown = 2f;
    public float breathCooldown = 3f;

    private bool canDash = true;
    private bool canRoar = true;
    private bool canUseBreath = true;

    // 攻击参数
    public float attackRange = 2f;    // 攻击范围
    public int attackDamage = 10;     // 攻击伤害

    void Start()
    {
        // 獲取 Animator 和 CameraShake 元件
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>(); // 確保 Camera 上有 CameraShake 腳本
    }

    void Update()
    {
        Attack();
        Roar();
        Dash();
        Breath();
    }

    // X 普攻、长按蓄力重击 (Xbox 控制器 X 鍵對應 joystick button 2)
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            animator.SetTrigger("Attack");  // 触发普攻动画
            cameraShake.TriggerShake();     // 触发画面震动
            PerformAttack();                // 调用 PerformAttack 方法执行攻击逻辑
        }
    }

    // 具体的攻击逻辑
    void PerformAttack()
    {
        // 攻击时，在玩家周围生成一个短暂的碰撞区域
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            // 确认是否检测到敌人
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // 对敌人造成伤害
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy hit: " + hitCollider.name + " Damage dealt: " + attackDamage);
            }
        }
    }

    void Roar()
    {
        // B 震吼 (Xbox 控制器 B 鍵對應 joystick button 1)
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && canRoar)
        {
            animator.SetTrigger("Roar");  // 触发震吼动画
            cameraShake.TriggerShake();   // 触发画面震动
            StartCoroutine(RoarCooldown());  // 开始冷却
        }
    }

    void Dash()
    {
        // A 小冲刺 (Xbox 控制器 A 键对应该 joystick button 0)
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && canDash)
        {
            animator.SetTrigger("Dash");  // 触发冲刺动画
            cameraShake.TriggerShake();   // 触发画面震动
            StartCoroutine(DashCooldown());  // 开始冷却
        }
    }

    void Breath()
    {
        // Y 特殊技能（冰吐息）(Xbox 控制器 Y 键对应该 joystick button 3)
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && canUseBreath)
        {
            animator.SetTrigger("Breath");  // 触发冰吐息动画
            cameraShake.TriggerShake();     // 触发画面震动
            StartCoroutine(BreathCooldown());  // 开始冷却
        }
    }

    // 冷却时间处理
    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator RoarCooldown()
    {
        canRoar = false;
        yield return new WaitForSeconds(roarCooldown);
        canRoar = true;
    }

    IEnumerator BreathCooldown()
    {
        canUseBreath = false;
        yield return new WaitForSeconds(breathCooldown);
        canUseBreath = true;
    }
}
