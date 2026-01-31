using UnityEngine;
using UnityEngine.UI;
using TMPro; // 如果你用了 TMP

public class LevelButton : MonoBehaviour
{
    [Header("设置关卡编号")]
    public int levelID; // 在 Inspector 面板里填入 1, 2, 3...
    
    public string levelPrefix = "Level";
    
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
    }

    void Start()
    {
        UpdateUIState();
    }

    // 更新按钮是否可点击的状态
    public void UpdateUIState()
    {
        // 从 GameManager 获取当前解锁进度
        int maxReached = GameManager.MaxLevelReached;

        // 如果当前按钮编号大于已解锁编号，则禁用
        bool isUnlocked = levelID <= maxReached;
        btn.interactable = isUnlocked;

        // 视觉反馈：如果没解锁，颜色变暗（可选）
        Image img = GetComponent<Image>();
        img.color = isUnlocked ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);

        // 绑定点击事件
        btn.onClick.RemoveAllListeners();
        if (isUnlocked)
        {
            btn.onClick.AddListener(OnClicked);
        }
    }

    void OnClicked()
    {
        string sceneName = levelPrefix + levelID;
        Debug.Log("Loading Level: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);}
    }