using UnityEngine;
using System.Collections;

public class SkillTree : MonoBehaviour
{
    public Animator animator;         // Animator 元件引用
    public CameraShake cameraShake;   // CameraShake 引用

    // 冷却时间参数
    public float attack01Cooldown = 1f;
    public float attack02Cooldown = 2f;
    public float dashCooldown = 3f;
    public float skill01Cooldown = 4f;
    public float skill02Cooldown = 5f;

    // 技能是否可以使用
    private bool canUseAttack01 = true;
    private bool canUseAttack02 = true;
    private bool canDash = true;
    private bool canUseSkill01 = true;
    private bool canUseSkill02 = true;

    void Start()
    {
        // 确保 Animator 和 CameraShake 已经附加到对象
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();

        if (cameraShake == null)
        {
            Debug.LogError("CameraShake component not found on the main camera.");
        }
    }

    void Update()
    {
        HandleSkills();  // 处理技能输入
    }

    // 处理技能输入
    void HandleSkills()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && canUseAttack01)  // Attack01 对应按钮
        {
            TriggerSkill("Attack01", attack01Cooldown, SkillType.Attack01);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton1) && canUseAttack02)  // Attack02 对应按钮
        {
            TriggerSkill("Attack02", attack02Cooldown, SkillType.Attack02);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton2) && canDash)  // Dash 对应按钮
        {
            TriggerSkill("Dash", dashCooldown, SkillType.Dash);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton3) && canUseSkill01)  // Skill01 对应按钮
        {
            TriggerSkill("Skill01", skill01Cooldown, SkillType.Skill01);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton4) && canUseSkill02)  // Skill02 对应按钮
        {
            TriggerSkill("Skill02", skill02Cooldown, SkillType.Skill02);
        }
    }

    // 定义技能类型枚举
    enum SkillType
    {
        Attack01,
        Attack02,
        Dash,
        Skill01,
        Skill02
    }

    // 触发技能的方法
    void TriggerSkill(string skillName, float cooldown, SkillType skillType)
    {
        // 播放对应技能的动画
        animator.SetTrigger(skillName);

        // 触发画面震动
        if (cameraShake != null)
        {
            cameraShake.TriggerShake();
        }

        // 根据技能类型调用冷却协程
        StartCoroutine(SkillCooldown(cooldown, skillType));
    }

    // 冷却处理的协程
    IEnumerator SkillCooldown(float cooldownTime, SkillType skillType)
    {
        // 禁用对应技能
        switch (skillType)
        {
            case SkillType.Attack01:
                canUseAttack01 = false;
                break;
            case SkillType.Attack02:
                canUseAttack02 = false;
                break;
            case SkillType.Dash:
                canDash = false;
                break;
            case SkillType.Skill01:
                canUseSkill01 = false;
                break;
            case SkillType.Skill02:
                canUseSkill02 = false;
                break;
        }

        // 等待冷却时间
        yield return new WaitForSeconds(cooldownTime);

        // 冷却结束后启用技能
        switch (skillType)
        {
            case SkillType.Attack01:
                canUseAttack01 = true;
                break;
            case SkillType.Attack02:
                canUseAttack02 = true;
                break;
            case SkillType.Dash:
                canDash = true;
                break;
            case SkillType.Skill01:
                canUseSkill01 = true;
                break;
            case SkillType.Skill02:
                canUseSkill02 = true;
                break;
        }
    }
}
