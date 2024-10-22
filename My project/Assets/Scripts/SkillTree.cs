using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Animator animator;         // Animator 元件引用
    public CameraShake cameraShake;   // CameraShake 引用

    // 攻击参数
    public float attackRange = 2f;          // 攻击范围
    public int normalAttackDamage = 10;     // 普通攻击伤害
    public int dashDamage = 20;             // 冲刺攻击伤害
    public int skill01Damage = 15;          // 技能1伤害
    public int skill02Damage = 25;          // 技能2伤害

    void Start()
    {
        // 初始化 Animator 和 CameraShake
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        // 调用攻击和技能逻辑
        ComboAttack();
        Dash();
        Skill01();
        Skill02();
    }

    // 普通攻击处理
    void ComboAttack()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.E))
        {
            // 触发攻击动画 (可以设置为连击动画)
            animator.SetTrigger("Attack01");

            // 调用 PerformAttack 来对敌人造成伤害
            PerformAttack(normalAttackDamage);
        }
    }

    // 冲刺技能
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            animator.SetTrigger("Dash");
            PerformAttack(dashDamage);  // 对敌人造成冲刺伤害
        }
    }

    // 技能1
    void Skill01()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            animator.SetTrigger("Skill01");
            PerformAttack(skill01Damage);  // 对敌人造成技能1伤害
        }
    }

    // 技能2
    void Skill02()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            animator.SetTrigger("Skill02");
            PerformAttack(skill02Damage);  // 对敌人造成技能2伤害
        }
    }

    // 具体的攻击逻辑，传入伤害值
    public void PerformAttack(int damage)
    {
        // 使用 OverlapSphere 检测攻击范围内的所有敌人
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider enemy in hitEnemies)
        {
            // 检查是否是敌人并对敌人造成伤害
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Enemy hit: " + enemy.name + " Damage dealt: " + damage);
            }
        }
    }

    // 可视化攻击范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
