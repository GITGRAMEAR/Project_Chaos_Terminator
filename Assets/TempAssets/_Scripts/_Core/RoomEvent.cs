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