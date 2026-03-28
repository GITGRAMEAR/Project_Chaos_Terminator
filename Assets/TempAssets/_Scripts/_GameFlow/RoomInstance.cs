using System;
using UnityEngine;

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
    public RoomDefinition def { get; private set; } // 原始配置（SO）
    public GameObject RoomObject { get; private set; } // 实例化后的预制体（场景中的房间）
    public RoomType Type => def.roomType; // 快捷访问房间类型

    /// <summary>
    /// 构造：传入房间配置
    /// 传入SO配置，立即实例化预制体
    /// </summary>
    public RoomInstance(RoomDefinition def)
    {
        this.def = def ?? throw new ArgumentNullException(nameof(def), "房间配置不能为空");
        
        // 实例化预制体（核心！让房间出现在场景中）
        if (def.roomPrefab != null)
        {
            RoomObject = UnityEngine.Object.Instantiate(def.roomPrefab);
            RoomObject.name = $"Room_{def.roomId}"; // 命名规范，方便调试
            RoomObject.SetActive(false); // 初始隐藏，等EnterRoom时激活
            
            // 发布房间创建事件
            EventBus.Publish(new RoomCreatedEvent(this, def));
        }
        else
        {
            Debug.LogError($"[RoomInstance] 房间{def.roomId}未关联预制体！");
        }
    }
    
    /// <summary>
    /// 启动房间逻辑（根据房间类型执行不同内容）
    /// </summary>
    public void Begin()
    {
        Debug.Log("启用房间");
        if (RoomObject == null) return;
        RoomObject.SetActive(true);
        
        // 发布房间开始事件
        EventBus.Publish(new RoomStartedEvent(this, def));
        
        // 根据房间类型启动不同逻辑
        // switch (Type)
        // {
        //     case RoomType.NormalBattle:
        //         EventBus.Publish(new BattleRoomStartedEvent(this, def));
        //         break;
        //     case RoomType.SpecialLinkage:
        //         EventBus.Publish(new SpecialRoomStartedEvent(this, def));
        //         break;
        //     case RoomType.RelicChoice:
        //         EventBus.Publish(new ChoiceRoomStartedEvent(this, def));
        //         break;
        // }
        // TODO: 根据 def.roomType 启动对应逻辑（战斗/事件/商店/奖励等）
    }
    
    // 销毁房间时回收资源
    public void Destroy()
    {
        Debug.Log("销毁房间");
        
        // 先发布房间销毁事件
        EventBus.Publish(new RoomDestroyedEvent(def.roomId, this));
        
        if (RoomObject != null) 
        {
            UnityEngine.Object.Destroy(RoomObject);
        }
    }
    
    
}