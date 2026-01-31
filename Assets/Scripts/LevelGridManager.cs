using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons; 

    public string levelPrefix = "Level";

    void Start()
    {
        // 1. 获取玩家进度（从 PlayerPrefs 读取，默认值为 1）
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        // 2. 遍历所有按钮进行初始化
        for (int i = 0; i < levelButtons.Length; i++)
        {
            // 关卡编号通常从 1 开始，而数组索引从 0 开始
            int levelNum = i + 1;

            if (levelNum > levelReached)
            {
                // --- 情况 A: 关卡未解锁 ---
                levelButtons[i].interactable = false; // 禁用按钮交互
                
                // 可选：在此处添加变灰或显示锁图标的代码
                // levelButtons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            else
            {
                // --- 情况 B: 关卡已解锁 ---
                levelButtons[i].interactable = true;

                // 核心技巧：必须使用一个临时局部变量来传递给 Lambda 表达式
                // 否则点击任何按钮都会加载最后一个关卡
                int capturedLevelNum = levelNum;

                // 清除旧的监听器，防止在编辑器中手动设置过的冲突
                levelButtons[i].onClick.RemoveAllListeners();

                // 自动绑定点击事件：点击加载对应场景
                levelButtons[i].onClick.AddListener(() => LoadLevelByName(levelPrefix + capturedLevelNum));
            }
        }
    }

    // 方法 1：按场景名称加载（用于自动绑定）
    public void LoadLevelByName(string sceneName)
    {
        Debug.Log("Loading: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // 方法 2：按 Build Index 加载（保留 LevelSelector 的功能，可用于手动调用）
    public void LoadLevelByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // 调试辅助：如果你想重置进度测试游戏，可以在控制台调用这个或绑定到一个测试按钮上
    [ContextMenu("Reset Player Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.SetInt("levelReached", 1);
        PlayerPrefs.Save();
        Debug.Log("进度已重置为第 1 关");
    }
}