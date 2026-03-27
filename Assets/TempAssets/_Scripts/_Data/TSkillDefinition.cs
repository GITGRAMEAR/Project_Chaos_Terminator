using UnityEngine;

/// <summary>
/// 技能底层效果标签
/// </summary>
[CreateAssetMenu(menuName = "ChaosTerminator/Data/Skill")]
public class SkillDefinition : ScriptableObject
{
    public string skillId;

    // 底层效果标签：用于“技能-房间联动”匹配
    public string baseEffectTag;

    // 基础数值（MVP 可简化）
    public float baseDamage = 10f;
    public float baseRadius = 1.2f;

    // TODO: 未来添加多技能系统（多个底层效果、连锁触发等）
}