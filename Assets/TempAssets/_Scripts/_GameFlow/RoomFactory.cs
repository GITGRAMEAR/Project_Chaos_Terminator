using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间工厂（核心创建器）
/// 功能：统一创建房间实例、随机生成候选房间、配置池读取 + 兜底生成
/// 被 GameFlow 下的流程控制器调用
/// </summary>
public class RoomFactory : MonoBehaviour
{
    [Header("房间配置池（可配置SO）")]
    [Tooltip("普通战斗房间候选池")]
    [SerializeField] private List<RoomDefinition> normalBattleRooms = new List<RoomDefinition>();

    [Tooltip("特殊联动房间候选池")]
    [SerializeField] private List<RoomDefinition> specialLinkageRooms = new List<RoomDefinition>();

    [Tooltip("遗物选择房间候选池")]
    [SerializeField] private List<RoomDefinition> relicChoiceRooms = new List<RoomDefinition>();

    [Header("兜底默认值（无配置时自动生成）")]
    [SerializeField] private string defaultRequiredEffectTag = "MentalField";
    [SerializeField] private string defaultSpecialEffectId = "AbsoluteSuppression";

    /// <summary>
    /// 创建运行时房间实例（供房间流程管理器调用）
    /// </summary>
    /// <param name="def">房间配置</param>
    /// <returns>运行时房间实例</returns>
    public RoomInstance Create(RoomDefinition def)
    {
        if (def == null)
        {
            Debug.LogWarning("[RoomFactory] 传入空房间配置，自动生成普通战斗房间");
            def = CreateRandomCandidate(RoomType.NormalBattle);
        }

        return new RoomInstance(def);
    }

    /// <summary>
    /// 生成随机房间候选（供房间选择器调用）
    /// </summary>
    /// <param name="type">要生成的房间类型</param>
    /// <returns>房间配置</returns>
    public RoomDefinition CreateRandomCandidate(RoomType type)
    {
        var pool = GetPool(type);
        
        // 优先从配置池中随机获取
        if (pool != null && pool.Count > 0)
        {
            return pool[UnityEngine.Random.Range(0, pool.Count)];
        }

        // 无配置池 → 自动生成临时房间（MVP兜底）
        return CreateFallbackDefinition(type);
    }

    /// <summary>
    /// 根据房间类型获取对应配置池
    /// </summary>
    private List<RoomDefinition> GetPool(RoomType type)
    {
        switch (type)
        {
            case RoomType.NormalBattle:
                return normalBattleRooms;
            case RoomType.SpecialLinkage:
                return specialLinkageRooms;
            case RoomType.RelicChoice:
                return relicChoiceRooms;
            default:
                return null;
        }
    }

    /// <summary>
    /// 创建临时兜底房间（无配置时保证游戏不崩）
    /// </summary>
    private RoomDefinition CreateFallbackDefinition(RoomType type)
    {
        var def = ScriptableObject.CreateInstance<RoomDefinition>();
        def.roomType = type;
        def.roomId = $"tmp_{type}_{Guid.NewGuid():N}".Substring(0, 20);

        // 特殊联动房自带标签与效果
        if (type == RoomType.SpecialLinkage)
        {
            def.requiredEffectTag = defaultRequiredEffectTag;
            def.specialEffectId = defaultSpecialEffectId;
        }
        else
        {
            def.requiredEffectTag = string.Empty;
            def.specialEffectId = string.Empty;
        }

        return def;
    }

    // TODO: 未来扩展
    // - 根据秩序值权重控制房间出现概率
    // - 房间类型冷却，避免连续重复
    // - 种子随机，支持复现局
    // - 支持“二选一”房间规则
}