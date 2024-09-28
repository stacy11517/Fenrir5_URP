using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;
    public float turnSpeed = 10f;  // �s�W��V�t��

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            // ���ʤ�V�]�w
            float moveDirectionY = moveDirection.y;

            // �ھڤW�U���k��ӨM�w���ʤ�V
            float horizontal = Input.GetAxis("Horizontal");  // ���� X �b (��/�k)
            float vertical = Input.GetAxis("Vertical");      // ���� Z �b (�e/��)

            // �]�w���ʤ�V�A�����ھ���L��J���ʨ���
            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection *= moveSpeed;

            // �p�G�����ʿ�J�A������²��ʤ�V����
            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                // �p�⨤��n���諸�ؼФ�V
                Vector3 targetDirection = new Vector3(horizontal, 0, vertical);

                // �ϥ� Quaternion.LookRotation �N��������ؼФ�V
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // ���Ʊ��ਤ��A�קK������V
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            // ���D
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = moveDirectionY;
            }
        }

        // ���O����
        moveDirection.y -= gravity * Time.deltaTime;

        // ���ʨ���
        controller.Move(moveDirection * Time.deltaTime);
    }
}
