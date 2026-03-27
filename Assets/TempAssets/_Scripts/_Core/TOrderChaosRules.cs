using UnityEngine;

/// <summary>
/// 秩序 - 混沌 核心规则公式类
/// 功能：所有与秩序值、混沌值、负面强度、衰减相关的纯计算公式
/// 特点：纯函数、无副作用、可单元测试
/// </summary>
public static class TOrderChaosRules
{
    /// <summary>
    /// 胜利时计算获得的秩序值
    /// 联动房间胜利会获得额外秩序奖励
    /// </summary>
    /// <param name="isLinkageRoom">是否为联动房间</param>
    /// <param name="orderGainBase">基础秩序奖励</param>
    /// <returns>最终获得的秩序值</returns>
    public static float CalcOrderGainOnWin(bool isLinkageRoom, float orderGainBase)
    {
        // 联动房胜利 → 秩序值 ×1.5
        // 普通房胜利 → 正常获取
        return orderGainBase * (isLinkageRoom ? 1.5f : 1.0f);
    }

    /// <summary>
    /// 失败时计算获得的混沌值
    /// 支持外部传入倍率（如难度、事件、遗物效果等）
    /// </summary>
    /// <param name="chaosGainBase">基础混沌值</param>
    /// <param name="chaosMultiplier">混沌倍率（默认1.0）</param>
    /// <returns>最终获得的混沌值</returns>
    public static float CalcChaosOnFail(float chaosGainBase, float chaosMultiplier = 1.0f)
    {
        return chaosGainBase * chaosMultiplier;
    }

    /// <summary>
    /// 计算当前混沌带来的负面强度倍率
    /// 混沌等级越高，负面效果越强
    /// </summary>
    /// <param name="baseChaosValue">基础混沌值（暂未使用）</param>
    /// <param name="chaosValueTier">混沌等级/层数</param>
    /// <returns>负面强度倍率（默认1.0起步）</returns>
    public static float CalcNegativeStrength(float baseChaosValue, float chaosValueTier)
    {
        // TODO: 未来可改成配置曲线/表驱动，方便策划平衡数值
        return 1.0f + 0.1f * chaosValueTier;
    }

    /// <summary>
    /// 混沌衰减计算
    /// 混沌值越高，后续新增混沌的效果会被衰减（防止无限滚雪球）
    /// </summary>
    /// <param name="baseChaosValue">当前基础混沌值</param>
    /// <returns>衰减系数（0~1之间）</returns>
    public static float CalcChaosDecay(float baseChaosValue)
    {
        // 基础混沌越大，衰减越明显
        // Mathf.Clamp01 保证结果不会小于0
        return Mathf.Clamp01(1.0f - baseChaosValue * 0.05f);
    }
}