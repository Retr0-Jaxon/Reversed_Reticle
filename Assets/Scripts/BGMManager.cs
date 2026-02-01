using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; // 单例模式，方便全局访问

    public AudioSource introSource;
    public AudioSource loopSource;
    private bool _isStarted = false;

    void Awake()
    {
        // --- 核心逻辑：确保切换场景不断开 ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 告诉 Unity：切场景别关了我
        }
        else
        {
            Destroy(gameObject); // 如果新场景里又有一个 BGMManager，直接销毁多余的
            return;
        }
    }

    void Start()
    {
        if (!_isStarted)
        {
            PlayBGM();
            _isStarted = true;
        }
    }

    void PlayBGM()
    {
        double startTime = AudioSettings.dspTime + 0.2;
        introSource.PlayScheduled(startTime);

        double loopStartTime = startTime + (double)introSource.clip.length;
        loopSource.loop = true;
        loopSource.PlayScheduled(loopStartTime);
    }
}