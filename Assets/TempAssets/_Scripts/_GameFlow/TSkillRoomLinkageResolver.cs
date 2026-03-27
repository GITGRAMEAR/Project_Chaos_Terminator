/// <summary>
/// 联动结果枚举：描述技能与房间联动后的最终结果类型
/// </summary>
public enum LinkageResult
{
    /// <summary>
    /// 无匹配 → 没有特殊效果
    /// </summary>
    NoMatchNormal,

    /// <summary>
    /// 匹配成功 +触发负面效果+ 战斗胜利 → 技能进化/升级+获取秩序值
    /// </summary>
    MatchedWinEvolve,

    /// <summary>
    /// 匹配成功 + 触发负面效果+战斗失败 →保留负面效果+ 混沌层数叠加
    /// </summary>
    MatchedFailChaosStack
}

/// <summary>
/// 技能房间联动解析器
/// 功能：判断技能与房间是否匹配，并根据胜负输出最终联动结果
/// </summary>
public static class TSkillRoomLinkageResolver
{
    /// <summary>
    /// 判断技能的基础效果标签 是否与 房间要求的标签 匹配
    /// </summary>
    /// <param name="skillBaseEffectTag">技能自身携带的基础效果Tag</param>
    /// <param name="requiredEffectTag">房间要求的效果Tag</param>
    /// <returns>true=匹配成功</returns>
    public static bool IsMatched(string skillBaseEffectTag, string requiredEffectTag)
        => !string.IsNullOrEmpty(requiredEffectTag) && requiredEffectTag == skillBaseEffectTag;

    /// <summary>
    /// 核心联动逻辑：根据【是否特殊联动房 + 是否匹配 + 是否胜利】得出最终结果
    /// </summary>
    /// <param name="isSpecialLinkageRoom">是否为特殊联动房间</param>
    /// <param name="isMatch">技能Tag与房间要求是否匹配</param>
    /// <param name="roomWon">房间是否胜利</param>
    /// <returns>LinkageResult 联动结果</returns>
    public static LinkageResult Resolve(
        bool isSpecialLinkageRoom,
        bool isMatch,
        bool roomWon)
    {
        // 不是特殊联动房 → 直接走【无匹配，只触发负面】
        if (!isSpecialLinkageRoom)
            return LinkageResult.NoMatchNormal;

        // 是特殊联动房
        // 匹配 + 胜利 → 进化
        if (isMatch && roomWon)
            return LinkageResult.MatchedWinEvolve;
        
        // 匹配 + 失败 → 混沌叠加
        if (isMatch && !roomWon)
            return LinkageResult.MatchedFailChaosStack;

        // 不匹配 → 只触发负面
        return LinkageResult.NoMatchNormal;
    }
}