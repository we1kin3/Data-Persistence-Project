using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 确保引入这个命名空间

public class DataManager : MonoBehaviour
{
    // 单例实例
    public static DataManager Instance;

    // 当前玩家名字 (场景间传递)
    public string CurrentPlayerName;

    // 最高分玩家名字 (跨会话持久化)
    public string BestPlayerName;
    // 最高分数 (跨会话持久化)
    public int BestScore;

    private void Awake()
    {
        // 单例模式检查
        if (Instance != null)
        {
            // 如果已经有实例存在，销毁这个新的实例
            Destroy(gameObject);
            return;
        }

        // 设置单例实例
        Instance = this;
        // 防止在加载新场景时被销毁
        DontDestroyOnLoad(gameObject);

        // 在游戏启动时加载最高分数据
        LoadHighScore();
    }

    // 加载最高分数据
    public void LoadHighScore()
    {
        // 使用 PlayerPrefs 读取数据
        // PlayerPrefs.GetString("Key", "默认值")
        // PlayerPrefs.GetInt("Key", 默认值)
        BestPlayerName = PlayerPrefs.GetString("BestPlayerName", ""); // 如果没有保存过，默认名字为空
        BestScore = PlayerPrefs.GetInt("BestScore", 0);             // 如果没有保存过，默认分数为0
    }

    // 保存最高分数据
    public void SaveHighScore()
    {
        // 使用 PlayerPrefs 写入数据
        PlayerPrefs.SetString("BestPlayerName", BestPlayerName);
        PlayerPrefs.SetInt("BestScore", BestScore);
        // 立即保存到磁盘（可选，但建议在重要数据变化时调用）
        PlayerPrefs.Save();
    }

    // 用于测试清理 PlayerPrefs
    // [ContextMenu("Clear High Score")]
    // public void ClearHighScore()
    // {
    //     PlayerPrefs.DeleteAll(); // 小心使用，会删除所有 PlayerPrefs 数据
    //     Debug.Log("PlayerPrefs Cleared");
    //     LoadHighScore(); // 重新加载默认值
    // }
}