using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件总线（增强版） - 全局发布/订阅系统
/// 核心优化点：
/// 1. 线程安全的订阅/取消订阅
/// 2. 异常隔离与错误处理
/// 3. 调试与监控能力
/// 4. 内存泄漏防护
/// 5. 性能优化
/// </summary>
public static class EventBus
{
    // 核心数据结构：按事件类型存储回调委托
    // 使用 static readonly 确保线程安全初始化
    private static readonly Dictionary<Type, Delegate> _eventDictionary = new();
    
    // 调试开关：控制是否输出日志
    public static bool EnableLogging = true;
    
    // 监控事件：外部可监听事件发布情况（用于调试面板）
    public static event Action<string> OnEventPublished;
    
    // 订阅统计：用于性能分析和内存泄漏检测
    private static readonly Dictionary<string, int> _subscriptionStats = new();
    
    /// <summary>
    /// 安全订阅事件
    /// 优化点：空回调检查、重复订阅检测、统计信息
    /// </summary>
    /// <typeparam name="T">事件类型（必须是结构体，避免装箱）</typeparam>
    /// <param name="callback">事件处理方法</param>
    public static void Subscribe<T>(Action<T> callback) where T : struct
    {
        if (callback == null)
        {
            Debug.LogError("[EventBus] 尝试订阅空回调，可能是脚本未初始化");
            return;
        }
        
        Type eventType = typeof(T);
        
        // 使用 lock 确保线程安全（多线程环境）
        lock (_eventDictionary)
        {
            if (_eventDictionary.ContainsKey(eventType))
            {
                // 检查是否已订阅（防止重复订阅导致内存泄漏）
                Delegate existing = _eventDictionary[eventType];
                if (existing != null)
                {
                    Delegate[] invocations = existing.GetInvocationList();
                    foreach (Delegate d in invocations)
                    {
                        if (d.Equals(callback))
                        {
                            Debug.LogWarning($"[EventBus] 重复订阅事件: {eventType.Name}, 目标: {callback.Target}");
                            return;
                        }
                    }
                }
                
                // 追加回调
                _eventDictionary[eventType] = Delegate.Combine(_eventDictionary[eventType], callback);
            }
            else
            {
                // 新增事件类型
                _eventDictionary.Add(eventType, callback);
            }
        }
        
        // 更新统计信息
        UpdateStats(eventType.Name, "subscribe");
        
        if (EnableLogging)
        {
            Debug.Log($"[EventBus] 订阅 {eventType.Name}, 目标: {callback.Target}, 方法: {callback.Method.Name}, 当前订阅数: {GetSubscriberCount(eventType)}");
        }
    }
    
    /// <summary>
    /// 安全发布事件
    /// 优化点：异常隔离、空事件检测、发布监控
    /// </summary>
    public static void Publish<T>(T message) where T : struct
    {
        Type eventType = typeof(T);
        
        if (EnableLogging)
        {
            Debug.Log($"[EventBus] 发布 {eventType.Name}");
            OnEventPublished?.Invoke(eventType.Name);
        }
        
        Delegate allCallbacks;
        lock (_eventDictionary)
        {
            if (!_eventDictionary.TryGetValue(eventType, out allCallbacks) || allCallbacks == null)
            {
                // 没有订阅者，避免不必要的处理
                if (EnableLogging)
                {
                    Debug.LogWarning($"[EventBus] 事件 {eventType.Name} 没有订阅者");
                }
                return;
            }
        }
        
        // 安全调用：捕获每个回调的异常，防止一个回调失败影响其他
        Delegate[] callbacks = allCallbacks.GetInvocationList();
        int successCount = 0;
        int errorCount = 0;
        
        foreach (Delegate callback in callbacks)
        {
            try
            {
                (callback as Action<T>)?.Invoke(message);
                successCount++;
            }
            catch (Exception e)
            {
                errorCount++;
                Debug.LogError($"[EventBus] 事件 {eventType.Name} 回调执行出错\n目标: {callback.Target}\n方法: {callback.Method.Name}\n错误: {e.Message}\n堆栈: {e.StackTrace}");
                
                // 可选：移除出错的订阅者
                // Unsubscribe<T>((Action<T>)callback);
            }
        }
        
        if (EnableLogging && errorCount > 0)
        {
            Debug.LogWarning($"[EventBus] 事件 {eventType.Name} 执行完成: {successCount} 成功, {errorCount} 失败");
        }
    }
    
    /// <summary>
    /// 安全取消订阅
    /// 优化点：空回调处理、线程安全、统计更新
    /// </summary>
    public static void Unsubscribe<T>(Action<T> callback) where T : struct
    {
        if (callback == null) return;
        
        Type eventType = typeof(T);
        
        lock (_eventDictionary)
        {
            if (_eventDictionary.ContainsKey(eventType))
            {
                Delegate current = _eventDictionary[eventType];
                if (current != null)
                {
                    _eventDictionary[eventType] = Delegate.Remove(current, callback);
                    
                    // 如果该事件类型已无订阅者，清理字典项
                    if (_eventDictionary[eventType] == null)
                    {
                        _eventDictionary.Remove(eventType);
                    }
                }
                
                UpdateStats(eventType.Name, "unsubscribe");
                
                if (EnableLogging)
                {
                    Debug.Log($"[EventBus] 取消订阅 {eventType.Name}, 目标: {callback.Target}, 剩余订阅数: {GetSubscriberCount(eventType)}");
                }
            }
        }
    }
    
    /// <summary>
    /// 批量取消订阅（按目标对象）
    /// 优化点：解决对象销毁时忘记取消订阅的问题
    /// </summary>
    /// <param name="target">要取消订阅的对象（通常是 MonoBehaviour）</param>
    public static void UnsubscribeAllFrom(object target)
    {
        if (target == null) return;
        
        lock (_eventDictionary)
        {
            List<Type> toRemove = new List<Type>();
            
            foreach (var kvp in _eventDictionary)
            {
                Type eventType = kvp.Key;
                Delegate handlers = kvp.Value;
                
                if (handlers != null)
                {
                    Delegate[] invocations = handlers.GetInvocationList();
                    bool removedAny = false;
                    
                    foreach (Delegate handler in invocations)
                    {
                        if (handler.Target == target)
                        {
                            // 创建泛型方法进行取消订阅
                            var unsubscribeMethod = typeof(EventBus).GetMethod("Unsubscribe")
                                .MakeGenericMethod(eventType);
                            unsubscribeMethod.Invoke(null, new object[] { handler });
                            removedAny = true;
                        }
                    }
                    
                    if (removedAny)
                    {
                        Debug.Log($"[EventBus] 已移除对象 {target} 的所有 {eventType.Name} 订阅");
                    }
                }
                
                // 如果该事件已无订阅者，标记为待删除
                if (handlers == null || handlers.GetInvocationList().Length == 0)
                {
                    toRemove.Add(eventType);
                }
            }
            
            // 清理空事件类型
            foreach (Type eventType in toRemove)
            {
                _eventDictionary.Remove(eventType);
            }
        }
    }
    
    /// <summary>
    /// 清空所有事件（场景切换/游戏重启时使用）
    /// 优化点：防止内存泄漏
    /// </summary>
    public static void ClearAll()
    {
        lock (_eventDictionary)
        {
            int count = _eventDictionary.Count;
            _eventDictionary.Clear();
            _subscriptionStats.Clear();
            
            if (EnableLogging)
            {
                Debug.Log($"[EventBus] 清空所有事件，共 {count} 种事件类型");
            }
        }
    }
    
    /// <summary>
    /// 获取事件订阅者数量
    /// 优化点：用于性能监控
    /// </summary>
    public static int GetSubscriberCount(Type eventType)
    {
        lock (_eventDictionary)
        {
            if (_eventDictionary.TryGetValue(eventType, out Delegate del))
            {
                return del?.GetInvocationList()?.Length ?? 0;
            }
        }
        return 0;
    }
    
    /// <summary>
    /// 获取所有事件类型（调试用）
    /// </summary>
    public static List<Type> GetAllEventTypes()
    {
        lock (_eventDictionary)
        {
            return new List<Type>(_eventDictionary.Keys);
        }
    }
    
    /// <summary>
    /// 获取订阅统计信息（用于监控面板）
    /// </summary>
    public static Dictionary<string, int> GetSubscriptionStats()
    {
        return new Dictionary<string, int>(_subscriptionStats);
    }
    
    /// <summary>
    /// 检查是否已订阅特定事件
    /// 优化点：避免重复订阅
    /// </summary>
    public static bool IsSubscribed<T>(Action<T> callback) where T : struct
    {
        Type eventType = typeof(T);
        
        lock (_eventDictionary)
        {
            if (_eventDictionary.TryGetValue(eventType, out Delegate del))
            {
                if (del != null)
                {
                    foreach (Delegate d in del.GetInvocationList())
                    {
                        if (d.Equals(callback))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    
    /// <summary>
    /// 更新订阅统计（内部使用）
    /// </summary>
    private static void UpdateStats(string eventName, string action)
    {
        string key = $"{eventName}_{action}";
        lock (_subscriptionStats)
        {
            if (_subscriptionStats.ContainsKey(key))
            {
                _subscriptionStats[key]++;
            }
            else
            {
                _subscriptionStats[key] = 1;
            }
        }
    }
}