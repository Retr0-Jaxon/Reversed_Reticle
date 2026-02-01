using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static int CurrentLevel;

    public static int MaxLevelReached = 1;
    
    public static GameManager instance;
    
    private LevelManager levelManager;
    
    private void Awake()
    {
        instance = this;
        levelManager = new LevelManager();
    }

    /**
     * 检测关卡是否完成
     */
    public void checkLevelComplete()
    {
        if (levelManager.isLevelComplete())
        {
            levelClear();
        }
    }
    
    
    
    /**
     * 通关所执行的逻辑
     */
    private void levelClear()
    {
        Debug.Log("通关了");
        /*// 1. 读取当前存档进度
        int reachedLevel = GameManager.MaxLevelReached;

        // 2. 如果当前关卡就是最高进度，则解锁下一关
        // 例如：我正在打第1关，当前存档也是1，那么解锁到第2关
        if (GameManager.CurrentLevel >= reachedLevel)
        {
            GameManager.MaxLevelReached += 1;
            Debug.Log("进度已更新！现在解锁了第 " + (GameManager.CurrentLevel  + 1) + " 关");
        }

        SceneManager.LoadScene("Level"+(GameManager.CurrentLevel+1));*/
    }
    






}