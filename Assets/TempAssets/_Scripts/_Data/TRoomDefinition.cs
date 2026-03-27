using UnityEngine;

public enum RoomType
{
    NormalBattle,//普通房间
    SpecialLinkage,//特殊联动房间
    RelicChoice//遗物选择
}

/// <summary>
/// 房间属性标签
/// </summary>
[CreateAssetMenu(menuName = "ChaosTerminator/Data/Room")]
public class RoomDefinition : ScriptableObject
{
    public string roomId;
    public RoomType roomType;

    // 特殊联动房会用这个 tag 去匹配技能底层效果
    public string requiredEffectTag;

    // 专属属性示例：绝对压制（视野限制/行动限制等）
    public string specialEffectId;
}