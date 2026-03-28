using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 房间流程管理器（核心状态机骨架）
/// 负责：房间启动、进入、结束、流程调度
/// 是整个游戏关卡流程的总控制器
/// </summary>
public class TRoomFlowManager : MonoBehaviour
{
    public static TRoomFlowManager Instance;
    /// <summary>
    /// 房间工厂：负责创建不同类型的房间实例
    /// </summary>
    [SerializeField] private RoomFactory roomFactory;
    
    public RoomFactory GetRoomFactory() => roomFactory;
    
    void Awake()
    {
        // 单例保证全局唯一
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 已存在 → 销毁自己，防止重复
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 候选房间
    /// </summary>
    private List<RoomDefinition> currentCandidates;

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
        //清空所有之前的事件订阅
        EventBus.ClearAll();
        SubscribeRoomEvents();
        // TODO: 未来扩展：初始化运行时状态、生成第一批候选房间、重置秩序/混沌等
        GoToNextRoomPreview();
    }

    private void SubscribeRoomEvents()
    {
        //监听房间事件开始和结束
        EventBus.Subscribe<RoomStartedEvent>(OnRoomStarted);
        EventBus.Subscribe<RoomFinishedEvent>(OnRoomFinished);
    }
    private void OnRoomStarted(RoomStartedEvent evt)
    {
        // 这里可以做一些房间开始的全局逻辑
        Debug.Log($"[FlowMgr] 房间 {evt.Definition.roomId} 已开始");
    }
    
    private void OnRoomFinished(RoomFinishedEvent evt)
    {
        Debug.Log($"[FlowMgr] 房间 {evt.Definition.roomId} 结束，胜利: {evt.Won}");
        
        // 可以在这里触发下一个房间的生成
        if (evt.Won)
        {
            // 胜利后的逻辑
            ClearNegatives();
            AwardPlayer(evt.Definition);
        }
        else
        {
            // 失败后的逻辑
            ShowFailOptions();
        }
    }
    
    /// <summary>
    /// 进入下一个房间预览阶段
    /// MVP版本：简化为直接生成下一个房间
    /// </summary>
    private void GoToNextRoomPreview()
    {
        Debug.Log("房间预览");
        // MVP：简化为直接生成下一个房 + 让 UI 调用“选择进/退”
        // TODO: 未来实现“消耗秩序值预览两个房间类型中的一个”
        Debug.Log("<color=yellow>=== 生成 2 个房间预览 ===</color>");

        // 1. 生成2个随机房间（从普通战斗房随机拿）
        currentCandidates = new List<RoomDefinition>();
        currentCandidates.Add(roomFactory.CreateRandomCandidate(RoomType.NormalBattle));
        currentCandidates.Add(roomFactory.CreateRandomCandidate(RoomType.NormalBattle));

        // 2. 把这2个房间发给UI，让玩家选择
        EventBus.Publish(new RoomPreviewGeneratedEvent(currentCandidates));
    }

    /// <summary>
    /// 直接进入指定房间（开始房间流程）
    /// </summary>
    /// <param name="def">房间配置数据</param>
    public void EnterRoom(RoomDefinition def)
    {
        // 工厂创建房间实例
        currentRoom = roomFactory.Create(def);

        // 启动房间逻辑(启动时发布事件)
        currentRoom.Begin();
    }

    /// <summary>
    /// 结束当前房间（胜利/失败）
    /// </summary>
    /// <param name="won">true=胜利，false=失败</param>
    public void FinishRoom(bool won)
    {
       EventBus.Publish(new RoomFinishedEvent(currentRoom, currentRoom.def,won));
       //销毁房间
       if (currentRoom != null)
       {
           currentRoom.Destroy();
           currentRoom = null;
       }

        // TODO: 胜负结算：技能升级/遗物获取/失败选项
        // TODO: 秩序/混沌数值更新
        // TODO: 房间联动效果清算
        // TODO: 胜利时清理负面效果 ClearNegatives()
        // TODO: 失败时打开“放弃/奋力一搏”选择界面
    }

    /// <summary>
    /// 清除所有负面效果
    /// 在房间胜利后调用，用于移除玩家身上的所有负面状态
    /// </summary>
    public void ClearNegatives()
    {
        //清楚负面效果
        //TODO:后期肯会实现多种类型
    }

    private void AwardPlayer(RoomDefinition def)
    {
        //TODO:奖励目前没在so里面配置
        
    }

    private void ShowFailOptions()
    {
        //TODO:失败
    }
    
    //销毁时退订
    private void OnDestroy()
    {
        
        EventBus.Unsubscribe<RoomStartedEvent>(OnRoomStarted);
        EventBus.Unsubscribe<RoomFinishedEvent>(OnRoomFinished);
    }
}
