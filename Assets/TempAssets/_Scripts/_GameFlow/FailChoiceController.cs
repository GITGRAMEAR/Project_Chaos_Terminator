using System;
using UnityEngine;

/// <summary>
/// 失败选择控制器
/// 功能：房间战败后，提供【放弃】和【奋力一搏】两个选项的逻辑处理
/// 负责抛出事件，由外部系统监听并执行结算、混沌叠加、负面效果等
/// </summary>
public class FailChoiceController : MonoBehaviour
{
    /// <summary>
    /// 玩家选择【放弃】时触发
    /// 外部监听：执行退出、结算、返回主界面等逻辑
    /// </summary>
    public event Action OnAbandon;

    /// <summary>
    /// 玩家选择【奋力一搏】时触发
    /// 外部监听：执行混沌叠加、附着负面效果、继续游戏等逻辑
    /// </summary>
    public event Action OnBraveFail;

    /// <summary>
    /// 选择放弃按钮调用
    /// </summary>
    public void ChooseAbandon()
    {
        OnAbandon?.Invoke();
    }

    /// <summary>
    /// 选择奋力一搏按钮调用
    /// 后续扩展：叠加混沌值、根据房间联动结果附着永久负面
    /// </summary>
    public void ChooseBraveFail()
    {
        // TODO: 奋力一搏逻辑：混沌值叠加 + 根据联动匹配情况永久附着负面
        OnBraveFail?.Invoke();
    }
}