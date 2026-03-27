using System;
using UnityEngine;

/// <summary>
/// 房间选择控制器
/// 功能：负责房间预览、房间选择、重掷房间流程
/// 属于 UI 与 流程管理层之间的中间层
/// </summary>
public class TRoomSelectionController : MonoBehaviour
{
    /// <summary>
    /// 当房间预览生成完毕时抛出事件
    /// 外部（UI）监听后显示两个房间选项
    /// </summary>
    public event Action<RoomDefinition, RoomDefinition> OnPreviewGenerated;

    /// <summary>
    /// 房间工厂：用于生成随机房间候选
    /// </summary>
    [SerializeField] private RoomFactory roomFactory;

    /// <summary>
    /// 请求生成房间预览（消耗秩序值进行预览）
    /// </summary>
    /// <param name="orderValue">当前秩序值（未来用于判断是否可预览/消耗）</param>
    public void RequestPreview(float orderValue)
    {
        // MVP 演示逻辑：固定生成 普通战斗房 + 特殊联动房
        // TODO: 未来实现逻辑：
        // 1. 消耗秩序值
        // 2. 根据规则随机房间类型（普通/特殊/遗物）
        // 3. 生成两个不同房间供选择
        var a = roomFactory.CreateRandomCandidate(RoomType.NormalBattle);
        var b = roomFactory.CreateRandomCandidate(RoomType.SpecialLinkage);

        // 通知外部：房间预览已生成
        OnPreviewGenerated?.Invoke(a, b);
    }

    /// <summary>
    /// 玩家选择进入某个房间
    /// </summary>
    /// <param name="chosen">玩家选中的房间配置</param>
    /// <param name="onDone">选择完成后的回调（关闭UI、跳转等）</param>
    public void ChooseEnter(RoomDefinition chosen, Action onDone)
    {
        // TODO: 这里真正调用 RoomFlowManager.EnterRoom 进入房间
        onDone?.Invoke();
    }

    /// <summary>
    /// 玩家选择重掷（重新生成房间）
    /// </summary>
    /// <param name="onDone">重掷完成回调</param>
    public void ChooseReRoll(Action onDone)
    {
        // 可扩展：消耗秩序/道具重掷
        onDone?.Invoke();
    }
}