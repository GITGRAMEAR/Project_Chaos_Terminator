using UnityEngine;

/// <summary>
/// 敌人基础 AI 控制器
/// 功能： idle → 追击 → 攻击 循环
/// 包含状态机、追击移动、攻击距离判断
/// </summary>
public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// 敌人 AI 状态
    /// Idle    待机
    /// Chase   追击玩家
    /// Attack  攻击玩家
    /// </summary>
    public enum State
    {
        Idle,
        Chase,
        Attack
    }

    [Header("AI 状态")]
    [SerializeField] private State state = State.Idle;

    [Header("追击参数")]
    [Tooltip("追击移动速度")]
    [SerializeField] private float chaseSpeed = 3.5f;

    [Header("攻击参数")]
    [Tooltip("进入攻击的距离")]
    [SerializeField] private float attackRange = 0.9f;

    [Header("目标")]
    [Tooltip("追击目标（通常是玩家）")]
    [SerializeField] private Transform target;

    private void Update()
    {
        // 没有目标直接不运行
        if (target == null) return;

        // 状态机驱动：每一帧执行当前状态逻辑
        switch (state)
        {
            case State.Idle:
                state = State.Chase; // 初始直接进入追击
                break;

            case State.Chase:
                TickChase();
                break;

            case State.Attack:
                TickAttack();
                break;
        }
    }

    /// <summary>
    /// 追击逻辑：向目标移动，进入攻击范围则切换攻击状态
    /// </summary>
    private void TickChase()
    {
        // 计算目标方向与距离
        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position);
        float dist = dir.magnitude;

        // 足够近 → 切换攻击
        if (dist <= attackRange)
        {
            state = State.Attack;
            return;
        }

        // 向目标移动
        dir = dir.normalized;
        transform.position += (Vector3)(dir * chaseSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 攻击逻辑：在攻击范围内执行攻击行为
    /// </summary>
    private void TickAttack()
    {
        // TODO: 对玩家造成伤害/效果
        // TODO: 受混沌负面影响时：攻击变慢、攻击间隔变长、命中降低
        // TODO: 对接秩序/混沌系统的负面效果（行动限制/减速/压制）
    }
}