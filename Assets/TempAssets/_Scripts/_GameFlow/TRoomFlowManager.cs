using UnityEngine;
using System;

/// <summary>
/// 房间流程管理器（核心状态机骨架）
/// 负责：房间启动、进入、结束、流程调度
/// 是整个游戏关卡流程的总控制器
/// </summary>
public class TRoomFlowManager : MonoBehaviour
{
    /// <summary>
    /// 房间正式开始时触发（UI、音效、状态同步可监听）
    /// </summary>
    public event Action OnRoomStarted;

    /// <summary>
    /// 房间结束时触发（结算、跳转、数据保存可监听）
    /// </summary>
    public event Action OnRoomFinished;

    /// <summary>
    /// 房间工厂：负责创建不同类型的房间实例
    /// </summary>
    [SerializeField] private RoomFactory roomFactory;

    /// <summary>
    /// 当前正在进行的房间实例
    /// </summary>
    private RoomInstance currentRoom;

    /// <summary>
    /// 开始一轮游戏（启动关卡流程）
    /// 初始化状态 → 进入房间预览
    /// </summary>
    public void StartRun()
    {
        // TODO: 未来扩展：初始化运行时状态、生成第一批候选房间、重置秩序/混沌等
        GoToNextRoomPreview();
    }

    /// <summary>
    /// 进入下一个房间预览阶段
    /// MVP版本：简化为直接生成下一个房间
    /// </summary>
    private void GoToNextRoomPreview()
    {
        // MVP：简化为直接生成下一个房 + 让 UI 调用“选择进/退”
        // TODO: 未来实现“消耗秩序值预览两个房间类型中的一个”
    }

    /// <summary>
    /// 进入指定房间（开始房间流程）
    /// </summary>
    /// <param name="def">房间配置数据</param>
    public void EnterRoom(RoomDefinition def)
    {
        // 通知：房间开始
        OnRoomStarted?.Invoke();

        // 工厂创建房间实例
        currentRoom = roomFactory.Create(def);

        // 启动房间逻辑
        currentRoom.Begin();
    }

    /// <summary>
    /// 结束当前房间（胜利/失败）
    /// </summary>
    /// <param name="won">true=胜利，false=失败</param>
    public void FinishRoom(bool won)
    {
        // 通知：房间结束
        OnRoomFinished?.Invoke();

        // TODO: 胜负结算：技能升级/遗物获取/失败选项
        // TODO: 秩序/混沌数值更新
        // TODO: 房间联动效果清算
        // TODO: 胜利时清理负面效果 ClearNegatives()
        // TODO: 失败时打开“放弃/奋力一搏”选择界面
    }
}

// ------------------------------------------------------------------------------

/// <summary>
/// 运行时房间实例
/// 作用：统一封装所有房间类型（战斗房、遗物房、特殊联动房等）
/// 遵循统一接口，方便流程管理
/// </summary>
public class RoomInstance
{
    /// <summary>
    /// 房间配置（类型、奖励、规则等）
    /// </summary>
    private readonly RoomDefinition def;

    /// <summary>
    /// 构造：传入房间配置
    /// </summary>
    public RoomInstance(RoomDefinition def)
    {
        this.def = def;
    }

    /// <summary>
    /// 启动房间逻辑（根据房间类型执行不同内容）
    /// </summary>
    public void Begin()
    {
        // TODO: 根据 def.roomType 启动对应逻辑（战斗/事件/商店/奖励等）
    }
}