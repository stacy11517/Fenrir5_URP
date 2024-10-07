using UnityEngine;
using System.Collections;

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

    public float comboResetTime = 1f; // 连击重置时间
    private int comboStep = 0;        // 追踪连击步骤
    private float lastComboTime = 0;  // 记录上一次攻击的时间

    // 攻击参数
    public float attackRange = 2f;          // 攻击范围
    public int normalAttackDamage = 10;     // 普通攻击伤害
    public int roarDamage = 15;             // 吼叫攻击伤害
    public int dashDamage = 20;             // 冲刺攻击伤害
    public int breathDamage = 25;           // 吐息攻击伤害

    void Start()
    {
        // 獲取 Animator 和 CameraShake 元件
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>(); // 確保 Camera 上有 CameraShake 腳本

        if (cameraShake == null)
        {
            Debug.LogError("CameraShake component not found on the main camera.");
        }
    }

    void Update()
    {
        ComboAttack();  // 连击攻击处理
        Roar();
        Dash();
        Breath();
    }

    // 普通攻击处理，带有连击机制
    void ComboAttack()
    {
        // 检查是否按下攻击键 (Xbox 控制器 X 键对应 joystick button 2 或键盘 E)
        if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.E))
        {
            // 如果超过了连击窗口，重置连击计数器
            if (Time.time - lastComboTime > comboResetTime)
            {
                comboStep = 0;
            }

            // 更新连击步骤和时间
            comboStep++;
            lastComboTime = Time.time;

            // 根据 comboStep 触发不同的动画
            if (comboStep == 1)
            {
                animator.SetTrigger("BasicAttack1");  // 第一击动画
            }
            else if (comboStep == 2)
            {
                animator.SetTrigger("BasicAttack2");  // 第二击动画
            }
            else if (comboStep == 3)
            {
                animator.SetTrigger("BasicAttack3");  // 第三击动画
                comboStep = 0;  // 在第三击后重置连击
            }

            // 这里不再直接调用 PerformAttack，而是在动画事件中调用
        }

        // 如果超过了连击窗口时间，自动重置连击计数器
        if (Time.time - lastComboTime > comboResetTime)
        {
            comboStep = 0;
        }
    }

    // 吼叫
    void Roar()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && canRoar)
        {
            animator.SetTrigger("Roar");  // 触发震吼动画
            StartCoroutine(RoarCooldown());
            // 让动画事件来处理 PerformAttack 和 TriggerShake
        }
    }

    // 冲刺
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && canDash)
        {
            animator.SetTrigger("Dash");  // 触发冲刺动画
            StartCoroutine(DashCooldown());
            // 让动画事件来处理 PerformAttack 和 TriggerShake
        }
    }

    // 吐息攻击
    void Breath()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && canUseBreath)
        {
            animator.SetTrigger("Breath");  // 触发冰吐息动画
            StartCoroutine(BreathCooldown());
            // 让动画事件来处理 PerformAttack 和 TriggerShake
        }
    }

    // 具体的攻击逻辑，传入伤害值
    public void PerformAttack(int damage)
    {
        // 使用 OverlapSphere 检测攻击范围内的所有碰撞体
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders)
        {
            // 检查是否是敌人
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log("Enemy hit: " + hitCollider.name + " Damage dealt: " + damage);
                }
            }
            // 检查是否是可互动物件
            else if (hitCollider.CompareTag("Interactable"))
            {
                InteractableObject interactable = hitCollider.GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    // 触发画面震动
    public void TriggerShake()
    {
        if (cameraShake != null)
        {
            cameraShake.TriggerShake();  // 触发画面震动
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

    // 可视化攻击范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // 处理与可互动物件的碰撞
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            InteractableObject interactable = other.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                // 執行可互動物件的互動行為
                interactable.Interact();
            }
        }
    }
}
