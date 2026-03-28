using UnityEngine;

public class RoomTestButton : MonoBehaviour
{
   
    
    [SerializeField]
    private RoomDefinition testDef;

    void Start()
    {
        // 游戏启动时先初始化流程
        TRoomFlowManager.Instance.StartRun();
    }

    // 按钮绑定：测试 1 次完整房间流程
    public void TestFullRoomFlow()
    {
        Debug.Log("<color=yellow>=== 开始测试：完整房间流程 ===</color>");

        // 1. 让工厂随机生成一个房间配置（预览功能）
        
        TRoomFlowManager.Instance.EnterRoom(testDef);
        Debug.Log($"<color=cyan>【预览】随机生成房间：{testDef.roomId} 类型：{testDef.roomType}</color>");

        // 2. 进入房间（创建 + 激活）
        TRoomFlowManager.Instance.EnterRoom(testDef);

        // 3. 等待 2 秒 → 自动结束房间（模拟胜利）
        Invoke(nameof(TestFinishRoom), 2f);
    }

    void TestFinishRoom()
    {
        Debug.Log("<color=green>=== 测试：房间胜利 ===</color>");
        TRoomFlowManager.Instance.FinishRoom(testDef);
    }
}