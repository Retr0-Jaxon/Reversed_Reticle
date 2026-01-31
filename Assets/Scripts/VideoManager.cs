using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    
    // 标记：是否需要在播放完后暂停
    private bool pauseAtEndRequested = false;

    void Awake()
    {
        // 注册视频播放结束的事件回调
        videoPlayer.loopPointReached += OnReachEnd;
    }

    /// <summary>
    /// 当选择关卡时调用此方法
    /// </summary>
    public void OnLevelSelected()
    {
        if (videoPlayer.isPlaying)
        {
            // 如果正在播放，标记“播完请暂停”
            pauseAtEndRequested = true;
            
            // 关键：必须关闭循环，否则不会触发 loopPointReached 事件
            videoPlayer.isLooping = false;
            
            Debug.Log("关卡已选，视频将在当前播放完成后暂停。");
        }
        else
        {
            // 如果视频本来就没在播，或者已经播完了，直接确保它停在最后一帧
            PauseAtLastFrame();
        }
    }

    // 视频自然播放到结尾时自动触发
    void OnReachEnd(VideoPlayer vp)
    {
        if (pauseAtEndRequested)
        {
            PauseAtLastFrame();
            pauseAtEndRequested = false; // 重置标记
        }
    }

    void PauseAtLastFrame()
    {
        // 暂停视频，此时画面会停在最后一帧
        videoPlayer.Pause();
        
        // 如果你希望彻底锁定在最后一帧，可以强制设置帧索引
        // videoPlayer.frame = (long)videoPlayer.frameCount - 1;
        
        Debug.Log("视频已在最后一帧暂停。");
    }

    // 记得在销毁脚本时取消订阅，防止内存泄漏
    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnReachEnd;
    }
}