using System;
using System.Collections.Generic;

/// <summary>
/// 单局存档数据（Run 存档）
/// 功能：存储当前一局游戏的所有关键数据，用于存档、读档、继续游戏
/// 可序列化 → 支持保存到 JSON / PlayerPrefs
/// </summary>
[Serializable]
public class RunSaveData
{
    /// <summary>
    /// 单局唯一ID（用于区分不同存档）
    /// </summary>
    public int runId;

    /// <summary>
    /// 当前秩序值
    /// </summary>
    public float orderValue;

    /// <summary>
    /// 当前混沌值
    /// </summary>
    public float chaosValue;

    /// <summary>
    /// 已拥有的技能ID列表
    /// </summary>
    public List<string> ownedSkillIds = new List<string>();

    /// <summary>
    /// 已拥有的遗物ID列表
    /// </summary>
    public List<string> ownedRelicIds = new List<string>();

    /// <summary>
    /// 当前附着的混沌负面效果列表
    /// </summary>
    public List<TChaosNegativeEffect> attachedNegatives = new List<TChaosNegativeEffect>();

    /// <summary>
    /// 当前已通关房间数（进度）
    /// </summary>
    public int roomCount;
}