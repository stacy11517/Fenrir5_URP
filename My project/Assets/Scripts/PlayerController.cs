using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = 20f;
    public float turnSpeed = 10f;  // �s�W��V�t��

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private Animator animator; // �s�W Animator �ܼ�

    public bool canMove = true;  // �Ω󱱨�a�O�_�i�H����

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // ��� Animator ����
    }

    void Update()
    {
        if (canMove)
        {
            if (controller.isGrounded)
            {
                // �ھڤW�U���k��ӨM�w���ʤ�V
                float horizontal = Input.GetAxis("Horizontal");  // ���� X �b (��/�k)
                float vertical = Input.GetAxis("Vertical");      // ���� Z �b (�e/��)

                // �]�w���ʤ�V�A�����ھ���L��J���ʨ���
                moveDirection = new Vector3(horizontal, 0, vertical);
                moveDirection *= moveSpeed;

                // ��s�ʵe���A
                if (horizontal != 0 || vertical != 0)
                {
                    // ���⦳���ʮɡA���� "����" �ʵe
                    animator.SetBool("isWalking", true);

                    // �p�⨤��n���諸�ؼФ�V
                    Vector3 targetDirection = new Vector3(horizontal, 0, vertical);

                    // �ϥ� Quaternion.LookRotation �N��������ؼФ�V
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                    // ���Ʊ��ਤ��A�קK������V
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                }
                else
                {
                    // ����S�����ʮɡA���� "���m" �ʵe
                    animator.SetBool("isWalking", false);
                }
            }

            // ���O����
            moveDirection.y -= gravity * Time.deltaTime;

            // ���ʨ���
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            // �T��ʮɡA����ʵe
            animator.SetBool("isWalking", false);
        }
    }
}
