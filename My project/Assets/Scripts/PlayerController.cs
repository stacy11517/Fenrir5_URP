using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;
    public float turnSpeed = 10f;  // 新增轉向速度

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
            // 移動方向設定
            float moveDirectionY = moveDirection.y;

            // 根據上下左右鍵來決定移動方向
            float horizontal = Input.GetAxis("Horizontal");  // 對應 X 軸 (左/右)
            float vertical = Input.GetAxis("Vertical");      // 對應 Z 軸 (前/後)

            // 設定移動方向，直接根據鍵盤輸入移動角色
            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection *= moveSpeed;

            // 如果有移動輸入，讓角色朝移動方向旋轉
            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                // 計算角色要面對的目標方向
                Vector3 targetDirection = new Vector3(horizontal, 0, vertical);

                // 使用 Quaternion.LookRotation 將角色旋轉到目標方向
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // 平滑旋轉角色，避免瞬間轉向
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            // 跳躍
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = moveDirectionY;
            }
        }

        // 重力應用
        moveDirection.y -= gravity * Time.deltaTime;

        // 移動角色
        controller.Move(moveDirection * Time.deltaTime);
    }
}
