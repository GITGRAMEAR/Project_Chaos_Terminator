using UnityEngine;

/// <summary>
/// 2D 技能释放器（施法者）
/// 功能：处理技能长按蓄力、释放、根据蓄力时间计算半径
/// 对接技能配置、房间联动、战斗系统
/// </summary>
public class SkillCaster2D : MonoBehaviour
{
    /// <summary>
    /// 当前使用的技能配置（范围、效果、标签等）
    /// </summary>
    [SerializeField] private SkillDefinition currentSkill;

    [Header("蓄力参数")]
    [Tooltip("最小蓄力时间（低于这个时间不加强）")]
    [SerializeField] private float minHold = 0.2f;

    [Tooltip("最大蓄力时间（达到后不再加强）")]
    [SerializeField] private float maxHold = 1.2f;

    [Tooltip("最小蓄力时的技能半径")]
    [SerializeField] private float radiusAtMin = 1.0f;

    [Tooltip("最大蓄力时的半径倍数（乘以技能基础半径）")]
    [SerializeField] private float radiusAtMaxMultiplier = 1.8f;

    // 是否正在长按蓄力
    private bool holding;

    // 当前已经蓄力的时间
    private float holdTime;

    /// <summary>
    /// 技能按下（开始蓄力）
    /// </summary>
    public void OnSkillPress()
    {
        holding = true;
        holdTime = 0f;
    }

    /// <summary>
    /// 技能松开（结束蓄力，释放技能）
    /// </summary>
    public void OnSkillRelease()
    {
        holding = false;

        // 计算蓄力比例（0~1）
        float t = Mathf.InverseLerp(minHold, maxHold, holdTime);

        // 根据蓄力比例，插值计算最终技能半径
        float radius = Mathf.Lerp(radiusAtMin, currentSkill.baseRadius * radiusAtMaxMultiplier, t);

        // TODO: 根据房间匹配情况，应用“进化/负面视野限制/混沌叠加”等联动效果
        // 执行最终施法
        CastAtRadius(radius);
    }

    private void Update()
    {
        // 仅在蓄力时累加时间
        if (!holding) return;
        holdTime += Time.deltaTime;
    }

    /// <summary>
    /// 根据最终半径释放技能
    /// </summary>
    /// <param name="radius">最终技能范围半径</param>
    private void CastAtRadius(float radius)
    {
        // MVP：生成伤害圈 / 特效 / 投射物
        // TODO: 与战斗系统、命中判定、效果系统对接
    }

    // TODO: 未来扩展
    // - 技能冷却
    // - 多技能管理
    // - 技能连锁
    // - 自动瞄准 / 朝向
}