using NUnit.Framework;
using UnityEngine;

public class OrderChaosRulesTest
{
    // 测试1：普通房间胜利 → 秩序值不变
    [Test]
    public void CalcOrderGainOnWin_NormalRoom_ReturnBase()
    {
        float result = TOrderChaosRules.CalcOrderGainOnWin(false, 100f);
        Assert.AreEqual(100f, result);
    }

    // 测试2：联动房间胜利 → 秩序值 ×1.5
    [Test]
    public void CalcOrderGainOnWin_LinkageRoom_Return1_5Times()
    {
        float result = TOrderChaosRules.CalcOrderGainOnWin(true, 100f);
        Assert.AreEqual(150f, result);
    }

    // 测试3：失败混沌值（默认倍率）
    [Test]
    public void CalcChaosOnFail_DefaultMult_ReturnBase()
    {
        float result = TOrderChaosRules.CalcChaosOnFail(50f);
        Assert.AreEqual(50f, result);
    }

    // 测试4：失败混沌值（2倍倍率）
    [Test]
    public void CalcChaosOnFail_Mult2_ReturnDouble()
    {
        float result = TOrderChaosRules.CalcChaosOnFail(50f, 2f);
        Assert.AreEqual(100f, result);
    }

    // 测试5：负面强度计算
    [Test]
    public void CalcNegativeStrength_Tier5_Return1_5()
    {
        float result = TOrderChaosRules.CalcNegativeStrength(0, 5);
        Assert.AreEqual(1.5f, result);
    }

    // 测试6：混沌衰减
    [Test]
    public void CalcChaosDecay_Base10_Return0_5()
    {
        float result = TOrderChaosRules.CalcChaosDecay(10f);
        Assert.AreEqual(0.5f, result);
    }
}