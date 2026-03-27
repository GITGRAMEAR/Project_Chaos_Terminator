using UnityEngine;

/// <summary>
/// 遗物属性
/// </summary>
[CreateAssetMenu(menuName = "ChaosTerminator/Data/Relic")]
public class RelicDefinition : ScriptableObject
{
    public string relicId;
    public string nameZh;

    public string effectId; // e.g. "OrderGainSpeedUp"

    // MVP：用简单参数承载
    public float orderMultiplier = 1.1f;

    // TODO: 未来添加遗物组合与联动
}