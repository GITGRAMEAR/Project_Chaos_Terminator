using System;
using System.Collections.Generic;

/// <summary>
/// 混沌负面效果实体类
/// 代表一个具体的混沌负面效果（如减速、时长惩罚、属性降低）
/// </summary>
[Serializable]
public class TChaosNegativeEffect
{
    /// <summary>
    /// 效果唯一ID（用于识别不同负面效果）
    /// </summary>
    public string effectId;

    /// <summary>
    /// 效果强度/倍率
    /// 例如：行动变慢倍率、冷却延长倍数、惩罚强度等
    /// </summary>
    public float strength;

    /// <summary>
    /// 混沌等级Key
    /// 用于根据“基础混沌值”做衰减计算，区分不同层级的负面
    /// </summary>
    public string baseChaosTierKey;
}

/// <summary>
/// 秩序 - 混沌 核心状态类
/// 作用：存储玩家当前秩序值、混沌值、附着的所有混沌负面效果
/// 纯数据类，不包含业务逻辑，可序列化
/// </summary>
[Serializable]
public class TOrderChaosState
{
    /// <summary>
    /// 当前秩序值
    /// </summary>
    public float orderValue;

    /// <summary>
    /// 当前混沌值
    /// </summary>
    public float chaosValue;

    /// <summary>
    /// 当前附着在玩家身上的所有混沌负面效果列表
    /// </summary>
    public List<TChaosNegativeEffect> attachedNegatives = new List<TChaosNegativeEffect>();

    /// <summary
    /// 清空所有负面效果
    /// 通常在关卡胜利/净化事件/特殊奖励时调用
    /// </summary>
    public void ClearNegatives()
    {
        attachedNegatives.Clear();
    }
}