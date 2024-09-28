using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public Animator animator;  

   
    public float dashCooldown = 1f;
    public float roarCooldown = 2f;
    public float breathCooldown = 3f;

    private bool canDash = true;
    private bool canRoar = true;
    private bool canUseBreath = true;

    
    void Start()
    {
      
        animator = GetComponent<Animator>();
    }

   
    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            animator.SetTrigger("Attack");  
        }

        
        if (Input.GetKey(KeyCode.JoystickButton2))
        {
            animator.SetBool("ChargeAttack", true); 
        }
        //�n��
        else
        {
            animator.SetBool("ChargeAttack", false); 
        }

        
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && canRoar)
        {
            animator.SetTrigger("Roar");  
            StartCoroutine(RoarCooldown());  
        }

      
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && canDash)
        {
            animator.SetTrigger("Dash"); 
            StartCoroutine(DashCooldown());
        }

      
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && canUseBreath)
        {
            animator.SetTrigger("Breath");  
            StartCoroutine(BreathCooldown());  
        }
    }

  
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

    void 
}


//X ����B�����W�O����
//B �_�q
//�o�X�p���νd��_�i�A�i�H�_�H�B�}�a�i���ʪ��~�άO��

//A �p�Ĩ�
//���e��[�t�Ĩ�@�p�q

//Y �S��ޯ�]�B�R���^
//�R�X�p�d��B���A��ᵲ�ĤH


