using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 必须引入这个命名空间

public class RestartGame : MonoBehaviour
{
    public void ReloadScene()
    {
        // 获取当前活动场景的名字
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 重新加载该场景
        SceneManager.LoadScene(currentSceneName);
    }

    
}