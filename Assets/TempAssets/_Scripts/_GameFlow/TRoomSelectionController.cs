using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 功能：生成房间预览、处理玩家选择、处理重掷逻辑
/// </summary>
public class TRoomSelectionController : MonoBehaviour
{
    [Header("房间工厂")]
    [Tooltip("用于生成随机房间配置")]
    [SerializeField] private RoomFactory roomFactory;

    #region 事件订阅（必须写）
    private void OnEnable()
    {
        // 订阅：外部请求生成房间预览
        EventBus.Subscribe<RequestRoomPreviewEvent>(OnRequestPreviewGenerated);

        // 订阅：玩家选择房间并进入
        EventBus.Subscribe<PlayerChooseRoomEvent>(OnPlayerChooseRoom);

        // 订阅：玩家重掷房间
        EventBus.Subscribe<PlayerReRollRoomEvent>(OnPlayerReRollRoom);
    }

    private void OnDisable()
    {
        // 取消订阅（防止内存泄漏）
        EventBus.Unsubscribe<RequestRoomPreviewEvent>(OnRequestPreviewGenerated);
        EventBus.Unsubscribe<PlayerChooseRoomEvent>(OnPlayerChooseRoom);
        EventBus.Unsubscribe<PlayerReRollRoomEvent>(OnPlayerReRollRoom);
    }
    #endregion

    #region 生成房间预览
    /// <summary>
    /// 收到生成预览请求 → 生成房间列表 → 发给UI
    /// </summary>
    private void OnRequestPreviewGenerated(RequestRoomPreviewEvent evt)
    {
        Debug.Log($"[房间选择器] 收到预览请求，当前秩序值：{evt.orderValue}");

        // ==============================
        // 核心：生成 2 个房间（可扩展N个）
        // ==============================
        List<RoomDefinition> candidates = new List<RoomDefinition>();
        candidates.Add(roomFactory.CreateRandomCandidate(RoomType.NormalBattle));
        candidates.Add(roomFactory.CreateRandomCandidate(RoomType.SpecialLinkage));
        // ==============================
        // 发布事件 → UI 显示房间选项
        // ==============================
        EventBus.Publish(new RoomPreviewGeneratedEvent(candidates));
    }
    #endregion

    #region 玩家选择房间
    /// <summary>
    /// 玩家选中房间 → 通知流程管理器进入房间
    /// </summary>
    private void OnPlayerChooseRoom(PlayerChooseRoomEvent evt)
    {
        if (evt.selectedRoomDef == null)
        {
            Debug.LogError("[房间选择器] 选中的房间配置为空！");
            return;
        }

        Debug.Log($"[房间选择器] 玩家选择房间：{evt.selectedRoomDef.roomId}");

        // 通知房间流程管理器 → 正式进入房间
        TRoomFlowManager.Instance.EnterRoom(evt.selectedRoomDef);

        // 执行回调：关闭UI、播放动画等
        evt.onComplete?.Invoke();
    }
    #endregion

    #region 玩家重掷房间
    /// <summary>
    /// 玩家重掷房间（可扩展消耗逻辑）
    /// </summary>
    private void OnPlayerReRollRoom(PlayerReRollRoomEvent evt)
    {
        Debug.Log("[房间选择器] 玩家执行重掷房间");

        // TODO：这里加消耗秩序值/道具逻辑

        // 重掷完成后回调
        evt.onComplete?.Invoke();

        // 重掷后重新生成一波房间
        OnRequestPreviewGenerated(new RequestRoomPreviewEvent(10));
    }
    #endregion
}