using System;
using System.Collections.Generic;

/// <summary>
/// 房间相关事件定义
/// 遵循：事件名 + Event 后缀
/// </summary>
public struct RoomCreatedEvent
{
    public RoomInstance RoomInstance;
    public RoomDefinition Definition;
    
    public RoomCreatedEvent(RoomInstance room, RoomDefinition def)
    {
        RoomInstance = room;
        Definition = def;
    }
}

public struct RoomStartedEvent
{
    public RoomInstance RoomInstance;
    public RoomDefinition Definition;
    
    public RoomStartedEvent(RoomInstance room, RoomDefinition def)
    {
        RoomInstance = room;
        Definition = def;
    }
}

public struct RoomFinishedEvent
{
    public RoomInstance RoomInstance;
    public RoomDefinition Definition;
    public bool Won; // 是否胜利
    
    public RoomFinishedEvent(RoomInstance room, RoomDefinition def, bool won)
    {
        RoomInstance = room;
        Definition = def;
        Won = won;
    }
}

public struct RoomDestroyedEvent
{
    public string RoomId;
    public RoomInstance RoomInstance;
    
    public RoomDestroyedEvent(string id, RoomInstance room)
    {
        RoomId = id;
        RoomInstance = room;
    }
}

public struct RoomPreviewGeneratedEvent
{
    public List<RoomDefinition> candidates;

    public RoomPreviewGeneratedEvent(List<RoomDefinition> candidates)
    {
        this.candidates = candidates;
    }
}

/// <summary>
/// 外部请求生成房间预览
/// </summary>
public struct RequestRoomPreviewEvent
{
    public float orderValue; // 秩序值（未来可用）
    public RequestRoomPreviewEvent(float orderValue) => this.orderValue = orderValue;
}

/// <summary>
/// 玩家选择了某个房间
/// </summary>
public struct PlayerChooseRoomEvent
{
    public RoomDefinition selectedRoomDef;
    public Action onComplete; // 选择完成后的回调

    public PlayerChooseRoomEvent(RoomDefinition selectedRoomDef, Action onComplete)
    {
        this.selectedRoomDef = selectedRoomDef;
        this.onComplete = onComplete;
    }
}


/// <summary>
/// 玩家请求重掷房间
/// </summary>
public struct PlayerReRollRoomEvent
{
    public Action onComplete;

    public PlayerReRollRoomEvent(Action onComplete)
    {
        this.onComplete = onComplete;
    }
}
