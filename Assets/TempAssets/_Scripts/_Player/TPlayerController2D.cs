using UnityEngine;

/// <summary>
/// 2D玩家基础移动控制器
/// 依赖Rigidbody2D，物理帧驱动移动，斜向速度归一化防超速
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("移动配置")]
    [Tooltip("玩家基础移动速度")]
    [SerializeField] private float moveSpeed = 6f;

    // 玩家刚体组件
    private Rigidbody2D rb;

    private void Awake()
    {
        // 初始化获取自身Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 获取原生横竖轴输入，并归一化（防止斜着走速度变快）
        Vector2 moveInput = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
           ).normalized;

        // 物理速度赋值，FixedUpdate更新更贴合物理引擎
        rb.velocity = moveInput * moveSpeed;
    }
}