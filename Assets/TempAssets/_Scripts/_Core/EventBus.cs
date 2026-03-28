using System;
using System.Collections.Generic;

/// <summary>
/// 事件总线（全局静态广播中心）
/// 作用：让各个模块（玩家/战斗/UI/房间流程）互相不引用，也能发送消息
/// </summary>
public static class EventBus
{
    // 核心字典：按【事件类型】存储所有监听回调
    private static readonly Dictionary<Type, Delegate> _eventDictionary = new Dictionary<Type, Delegate>();

    /// <summary>
    /// 订阅事件（监听某个消息）
    /// 用法：谁需要接收消息，谁就调用 Subscribe
    /// </summary>
    /// <typeparam name="T">事件结构体（比如 RoomClearEvent）</typeparam>
    /// <param name="callback">收到消息时执行的方法</param>
    public static void Subscribe<T>(Action<T> callback)
    {
        Type eventType = typeof(T);

        if (_eventDictionary.ContainsKey(eventType))
        {
            // 已存在：追加回调
            _eventDictionary[eventType] = Delegate.Combine(_eventDictionary[eventType], callback);
        }
        else
        {
            // 不存在：新增
            _eventDictionary.Add(eventType, callback);
        }
    }

    /// <summary>
    /// 发布事件（发送消息）
    /// 用法：事情发生后，调用 Publish 广播给所有人
    /// </summary>
    public static void Publish<T>(T message)
    {
        Type eventType = typeof(T);

        if (_eventDictionary.TryGetValue(eventType, out Delegate allCallbacks))
        {
            // 把存储的委托 转成 对应类型的 Action，并执行
            Action<T> action = allCallbacks as Action<T>;
            action?.Invoke(message);
        }
    }

    /// <summary>
    /// 取消订阅（停止监听）
    /// 重要：物体销毁时必须取消，防止空引用报错
    /// </summary>
    public static void Unsubscribe<T>(Action<T> callback)
    {
        Type eventType = typeof(T);

        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] = Delegate.Remove(_eventDictionary[eventType], callback);
        }
    }

    /// <summary>
    /// 清空所有事件（游戏重启/场景切换时用）
    /// </summary>
    public static void ClearAll()
    {
        _eventDictionary.Clear();
    }
}