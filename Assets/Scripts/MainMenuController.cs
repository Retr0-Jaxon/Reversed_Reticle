using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用场景管理命名空间
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;       // 拖入 TitlePanel
    public GameObject levelSelectPanel; // 拖入 LevelSelectPanel
    public GameObject CreditsPanel;     // 拖入 CreditsPanel

    // 点击 "Start Game" 按钮调用，切换到关卡选择界面
    public void OpenLevelSelect()
    {
        titlePanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }

    // 点击 "Back" 按钮调用，返回主标题
    public void ReturnToMainMenu()
    {
        // 假设你的主菜单场景名字叫 "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
    // 点击 "Quit" 按钮调用
    public void QuitGame()
    {
        Debug.Log("Quit Game!"); // 编辑器中无法真正退出，打印日志测试
        Application.Quit();
    }
    //点击 "Credits" 按钮调用
    public void OpenCredits()
    {
        CreditsPanel.SetActive(true);
        titlePanel.SetActive(false);
        levelSelectPanel.SetActive(false);
    }
        
    // 直接加载特定场景（通用方法）
    public void LoadLevelScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void Start()
    {    // 游戏开始时，确保只有标题面板显示
    titlePanel.SetActive(true);
    levelSelectPanel.SetActive(false);
    CreditsPanel.SetActive(false);
    }
}