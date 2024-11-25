using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = 20f;
    public float turnSpeed = 10f;  // 新增轉向速度

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private Animator animator; // 新增 Animator 變數

    public bool canMove = true;  // 用於控制玩家是否可以移動

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // 獲取 Animator 元件
    }

    void Update()
    {
        if (canMove)
        {
            if (controller.isGrounded)
            {
                // 根據上下左右鍵來決定移動方向
                float horizontal = Input.GetAxis("Horizontal");  // 對應 X 軸 (左/右)
                float vertical = Input.GetAxis("Vertical");      // 對應 Z 軸 (前/後)

                // 設定移動方向，直接根據鍵盤輸入移動角色
                moveDirection = new Vector3(horizontal, 0, vertical);
                moveDirection *= moveSpeed;

                // 更新動畫狀態
                if (horizontal != 0 || vertical != 0)
                {
                    // 當角色有移動時，播放 "走路" 動畫
                    animator.SetBool("isWalking", true);

                    // 計算角色要面對的目標方向
                    Vector3 targetDirection = new Vector3(horizontal, 0, vertical);

                    // 使用 Quaternion.LookRotation 將角色旋轉到目標方向
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                    // 平滑旋轉角色，避免瞬間轉向
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                }
                else
                {
                    // 當角色沒有移動時，播放 "閒置" 動畫
                    animator.SetBool("isWalking", false);
                }
            }

            // 重力應用
            moveDirection.y -= gravity * Time.deltaTime;

            // 移動角色
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            // 禁止移動時，停止動畫
            animator.SetBool("isWalking", false);
        }
    }
}
