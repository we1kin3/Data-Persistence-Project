using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // 如果你使用了 TextMeshPro
using UnityEngine.UI; // 如果你使用了旧版 Text 和 InputField

public class MenuManager : MonoBehaviour
{
    // UI 元素的公共引用，需要从 Inspector 窗口拖拽关联
    public TextMeshProUGUI BestScoreText; // 或者 public Text BestScoreText;
    public TMPro.TMP_InputField NameInput; // 或者 public InputField NameInput;

    void Start()
    {
        // 在菜单场景启动时显示加载的最高分
        LoadBestScore();
    }

    // 从 DataManager 加载并显示最高分
    public void LoadBestScore()
    {
        // 检查 DataManager 实例是否存在
        if (DataManager.Instance != null)
        {
            // 更新 Best Score Text 显示
            // 如果 BestScoreText 是 TextMeshProUGUI，使用 text 属性
            BestScoreText.text = $"Best Score : {DataManager.Instance.BestPlayerName} : {DataManager.Instance.BestScore}";
            // 如果 BestScoreText 是旧版 Text，也使用 text 属性
            // BestScoreText.text = $"Best Score : {DataManager.Instance.BestPlayerName} : {DataManager.Instance.BestScore}";
        }
        else
        {
             // 如果 DataManager 实例不存在 (理论上不应该发生，除非DataManager没有DontDestroyOnLoad或场景顺序错误)
             Debug.LogError("DataManager Instance is null in MenuManager!");
             BestScoreText.text = "Best Score : N/A : 0";
        }
    }

    // 启动游戏按钮点击事件调用的函数
    public void StartGame()
    {
        // 检查 DataManager 实例是否存在
         if (DataManager.Instance != null)
         {
             // 获取玩家输入的名字
             string playerName = NameInput.text;

             // 可以添加一个简单的名字验证，例如不允许空名字
             if (string.IsNullOrWhiteSpace(playerName))
             {
                 Debug.LogWarning("Player name cannot be empty!");
                 // 可选：给用户一个提示，例如显示一个错误文本
                 return; // 不加载场景
             }

             // 将玩家名字保存到 DataManager 的临时变量中
             DataManager.Instance.CurrentPlayerName = playerName;

             // 加载主游戏场景 (场景名字必须与 Build Settings 中的一致)
             // 注意：你的主游戏场景名字是 "main"
             SceneManager.LoadScene("main");
         }
         else
         {
             Debug.LogError("DataManager Instance is null when trying to start game!");
         }
    }

    // 退出游戏按钮点击事件调用的函数
    public void QuitGame()
    {
        // 仅在 Editor 中测试时有用，发布后会直接退出应用
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // 发布版本中退出应用
#endif
    }
}