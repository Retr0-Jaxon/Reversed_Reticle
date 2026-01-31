using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static int CurLevel;

    public static int MaxLevelReached;
    
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
        if (CurLevel == MaxLevelReached)
        {
            MaxLevelReached++;
        }
    }
    






}