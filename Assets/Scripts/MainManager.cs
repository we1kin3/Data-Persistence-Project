using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    public GameObject GameOverText;
    public TextMeshProUGUI BestScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance != null)
        {
            // 我们可以直接在 Start 里面更新 BestScoreText 的显示
            BestScoreText.text = $"Best Score : {DataManager.Instance.BestPlayerName} : {DataManager.Instance.BestScore}";

            // 如果你需要在游戏过程中显示当前玩家的名字，可以修改 ScoreText 或者添加一个 PlayerNameText
            // ScoreText 已经被用于显示当前分数，所以我们让 BestScoreText 负责显示名字
            // 如果 ScoreText 也要显示名字，可以这样：
            // ScoreText.text = $"{DataManager.Instance.CurrentPlayerName} : Score : {m_Points}";
            // 这里我们只让 BestScoreText 显示最高分信息，ScoreText 只显示当前分数，符合 GIF 演示的效果

            // 初始化当前分数显示
            ScoreText.text = $"Score : {m_Points}";
        }
        else
        {
            Debug.LogError("DataManager Instance is null in MainManager!");
            BestScoreText.text = "Best Score : N/A : 0";
            ScoreText.text = "Score : 0";
        }
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        
 
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DataManager.Instance.CurrentPlayerName = ""; // 可选，看需求

                // 加载回菜单场景
                SceneManager.LoadScene("menu"); // 从主场景返回菜单场景
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        
        if (DataManager.Instance != null)
        {
            if (m_Points > DataManager.Instance.BestScore)
            {
                // 打破了最高分，更新 DataManager 中的最高分数据
                DataManager.Instance.BestScore = m_Points;
                // DataManager.Instance.CurrentPlayerName 存储了当前玩家从菜单输入的名字
                DataManager.Instance.BestPlayerName = DataManager.Instance.CurrentPlayerName;

                // 保存新的最高分数据到 PlayerPrefs
                DataManager.Instance.SaveHighScore();

                // 更新 Best Score Text 的显示，立即反映新的最高分
                BestScoreText.text = $"Best Score : {DataManager.Instance.BestPlayerName} : {DataManager.Instance.BestScore}";

                // 可选：在 GameOverText 附近显示 "New High Score!" 提示
            }
        }
        else
        {
            Debug.LogError("DataManager Instance is null when checking high score!");
        }
    }
}
