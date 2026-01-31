using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinished : MonoBehaviour
{
    [Header("关卡设置")]
    public int currentLevel;      // 当前是第几关？（例如：1）
    public string nextLevelName;  // 下一关的场景名字？（例如：Level2）

    // 当玩家触发过关（比如走到终点、打完Boss）时调用这个方法
    public void FinishLevel()
    {
        // 1. 读取当前存档进度
        int reachedLevel = PlayerPrefs.GetInt("levelReached", 1);

        // 2. 如果当前关卡就是最高进度，则解锁下一关
        // 例如：我正在打第1关，当前存档也是1，那么解锁到第2关
        if (currentLevel >= reachedLevel)
        {
            PlayerPrefs.SetInt("levelReached", currentLevel + 1);
            PlayerPrefs.Save(); // 确认保存到硬盘
            Debug.Log("进度已更新！现在解锁了第 " + (currentLevel + 1) + " 关");
        }

        // 3. 接下来做什么？（二选一）
        // 方案 A: 直接加载下一关
        SceneManager.LoadScene(nextLevelName);

        // 方案 B: 返回主菜单（让玩家在关卡选择界面看新解锁的关卡）
        //SceneManager.LoadScene("MainMenu");
    }

    // 示例：如果是触碰某个物体（终点）过关
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 假设玩家身上有 "Player" 标签
        if (collision.CompareTag("Player"))
        {
            FinishLevel();
        }
    }
}