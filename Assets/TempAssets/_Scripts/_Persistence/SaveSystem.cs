using System.IO;
using UnityEngine;

/// <summary>
/// 存档系统（核心持久化）
/// 功能：将单局游戏数据（RunSaveData）保存为 JSON 文件
/// 存储路径：应用持久化数据目录 + saves 文件夹
/// </summary>
public class SaveSystem : MonoBehaviour
{
    [Header("存档设置")]
    [Tooltip("存档文件夹名称")]
    public string saveFolderName = "saves";

    /// <summary>
    /// 将单局数据保存为 JSON 文件
    /// </summary>
    /// <param name="data">要保存的游戏数据</param>
    public void SaveToJson(RunSaveData data)
    {
        // 拼接存档文件夹路径（应用持久化路径 + 自定义文件夹）
        string dir = Path.Combine(Application.persistentDataPath, saveFolderName);
        
        // 如果文件夹不存在，自动创建
        Directory.CreateDirectory(dir);

        // 拼接最终文件路径：run_编号.json
        string path = Path.Combine(dir, $"run_{data.runId}.json");

        // 将数据序列化为格式化 JSON
        string json = JsonUtility.ToJson(data, true);

        // 写入文件
        File.WriteAllText(path, json);

        // 记录最后一次存档的ID（方便继续游戏）
        PlayerPrefs.SetString("last_run_id", data.runId.ToString());
        PlayerPrefs.Save();
    }

    // TODO: 未来扩展
    // - 加载存档 LoadFromJson(runId)
    // - 删除存档
    // - 云同步（Unity Cloud Save / 第三方服务）
}