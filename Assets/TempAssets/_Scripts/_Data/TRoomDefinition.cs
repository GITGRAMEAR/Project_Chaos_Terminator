using UnityEngine;

public enum RoomType
{
    NormalBattle,     // 普通战斗房间
    SpecialLinkage,   // 特殊联动房间
    RelicChoice       // 遗物选择房间
}

/// <summary>
/// 房间配置数据（ScriptableObject）
/// </summary>
[CreateAssetMenu(menuName = "ChaosTerminator/Data/Room Definition")]
public class RoomDefinition : ScriptableObject
{
    [Header("=== 房间基础配置 ===")]
    [Tooltip("房间唯一ID，用于流程识别")]
    public string roomId;
    
    [Header("预制体关联")]
    public GameObject roomPrefab; 
    
    [Tooltip("房间类型，决定生成逻辑")]
    public RoomType roomType;

    [Header("=== 特殊联动房间专用 ===")]
    [Tooltip("只有匹配对应EffectTag的技能才能触发该房间")]
    public string requiredEffectTag;

    [Header("=== 房间特殊效果 ===")]
    [Tooltip("视野限制/行动限制/绝对压制等效果ID")]
    public string specialEffectId;
}