using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI测试脚本：点击按钮 → 生成预览 → 选择房间
/// </summary>
public class TestRoomSelectUI : MonoBehaviour
{
    [SerializeField] private TRoomFlowManager flowManager;
    [Header("按钮")]
    public Button btnGeneratePreview; // 生成预览
    public Button btnChooseRoom1;     // 选房间1
    public Button btnChooseRoom2;     // 选房间2
    public Button btnReRoll;          // 重掷

    private List<RoomDefinition> currentCandidates;

    private void OnEnable()
    {
        // 监听：房间预览生成完毕
        EventBus.Subscribe<RoomPreviewGeneratedEvent>(OnPreviewGenerated);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<RoomPreviewGeneratedEvent>(OnPreviewGenerated);
    }

    void TestFinishRoom()
    {
        Debug.Log("<color=green>=== 测试：房间胜利 ===</color>");
        flowManager.FinishRoom(true);
    }
    private void Start()
    {
        // 按钮绑定事件
        btnGeneratePreview.onClick.AddListener(OnClickGenerate);
        btnChooseRoom1.onClick.AddListener(() => ChooseRoom(0));
        btnChooseRoom2.onClick.AddListener(() => ChooseRoom(1));
        btnReRoll.onClick.AddListener(OnClickReRoll);
    }

    // 点击生成预览
    private void OnClickGenerate()
    {
        Debug.Log("[UI] 请求生成房间预览");
        EventBus.Publish(new RequestRoomPreviewEvent(10));
    }

    // 收到预览房间列表
    private void OnPreviewGenerated(RoomPreviewGeneratedEvent evt)
    {
        currentCandidates = evt.candidates;
        Debug.Log($"[UI] 收到 {currentCandidates.Count} 个房间");
    }

    // 选择房间
    private void ChooseRoom(int index)
    {
        if (currentCandidates == null || index >= currentCandidates.Count) return;

        EventBus.Publish(new PlayerChooseRoomEvent(
            currentCandidates[index],
            () => Debug.Log("[UI] 选择完成，关闭面板")
        ));
    }

    // 点击重掷
    private void OnClickReRoll()
    {
        EventBus.Publish(new PlayerReRollRoomEvent(
            () => Debug.Log("[UI] 重掷完成")
        ));
    }
}