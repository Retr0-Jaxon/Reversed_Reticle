using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 当前正在玩的关卡
    public static int CurrentLevel = 1;
    public const int FINAL_LEVEL = 6; // 定义最后一关是第6关

    // 已经解锁的最高关卡 (从本地存储读取)
    public static int MaxLevelReached 
    {
        get => PlayerPrefs.GetInt("MaxLevelReached", 1);
        set => PlayerPrefs.SetInt("MaxLevelReached", value);
        
    }
    
    public static GameManager instance;
    private LevelManager levelManager;
    
    [SerializeField]
    private OperateMode operateMode = OperateMode.ClickMode;
    
    
    public static OperateMode OperateMode
    {
        get => instance.operateMode;
        set => instance.operateMode = value;
    }
    
    
    
     public GameObject thanksPanel; // 第6关通关后的感谢面板
    private void Start()
    {
        // 确保感谢面板一开始是隐藏的
        if (thanksPanel != null)
            thanksPanel.SetActive(false);
    }
    private void Awake()
    {
        // 简单的单例模式
        if (instance == null) {
            instance = this;
            // 如果这个脚本挂在每个场景都有的物体上，且你不希望它消失：
            // DontDestroyOnLoad(gameObject); 
        }

        levelManager = new LevelManager();

        // 【关键修复】：根据当前场景的名字自动更新 CurrentLevel
        // 假设场景名是 "Level1", "Level2"...
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level"))
        {
            string levelNum = sceneName.Replace("Level", "");
            int.TryParse(levelNum, out CurrentLevel);
        }
    }

    public void checkLevelComplete()
    {
        if (levelManager.isLevelComplete())
        {
            levelClear();
        }
    }
    
    private void levelClear()
    {
        // 1. 如果当前关卡是最高进度，解锁下一关
        if (CurrentLevel >= MaxLevelReached)
        {
            MaxLevelReached = CurrentLevel + 1;
            PlayerPrefs.Save(); // 立即保存到硬盘
        }

        // 2. 计算下一关的关卡号
        int nextLevel = CurrentLevel + 1;

        // 3. 加载下一关
         // 2. 判断是否是最后一关
        if (CurrentLevel == FINAL_LEVEL)
        {
            // 如果是第6关，弹出感谢界面
            if (thanksPanel != null) thanksPanel.SetActive(true);
        }
        else
        {
            // 不是最后一关，加载下一关
            SceneManager.LoadScene("Level" + nextLevel);
        }
    }
}