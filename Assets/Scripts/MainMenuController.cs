using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用场景管理命名空间
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;       // 拖入 TitlePanel
    public GameObject levelSelectPanel; // 拖入 LevelSelectPanel
    public GameObject CreditsPanel;     // 拖入 CreditsPanel
    public GameObject sidebarpanel;
    public VideoManager videoManager;   // 拖入 VideoManager 脚本组件
    // 静态变量在场景切换时不会被重置
    public static bool showLevelSelectOnStart = false;
    // 点击 "Start Game" 按钮调用，切换到关卡选择界面
    void Start()
    {
        titlePanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        if (showLevelSelectOnStart)
        {
            OpenLevelSelect();
            showLevelSelectOnStart = false; // 重置为默认值
        }
    }

    private void ShowTitlePanel()
    {
        titlePanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }

    // 切换到关卡选择界面（带视频逻辑，供按钮调用）
    public void OpenLevelSelect()
    {
        showLevelSelectOnStart = true; // 记录状态
        titlePanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        sidebarpanel.SetActive(false);
        
        if (videoManager != null) 
            videoManager.OnLevelSelected();
    }

    // 内部调用，不重复触发视频逻辑（防止视频状态错乱）
    private void OpenLevelSelectFromScene()
    {
        titlePanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        
        // 如果返回关卡界面时视频也需要是暂停状态，这里也可以调用
        if (videoManager != null) 
            videoManager.OnLevelSelected();
    }

    // 点击 "Back" 按钮调用，返回主标题
    public void ReturnToMainMenu()
    {
        showLevelSelectOnStart = false; // 重置状态
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
    public static void BackToLevelSelect()
    {
        showLevelSelectOnStart = true; // 告诉主菜单：下次启动请直接开 LevelSelect
        SceneManager.LoadScene("MainMenu");
    }
        
    // 直接加载特定场景（通用方法）
    public void LoadLevelScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}