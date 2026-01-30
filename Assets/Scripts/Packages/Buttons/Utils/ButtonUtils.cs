using UnityEditor;

namespace com.startech.Buttons
{
    public static class ButtonUtils 
    {
        /// <summary>
        /// 当处于编辑器时，停止程序并弹窗
        /// </summary>
        /// <param name="title">弹窗标题</param>
        /// <param name="message">弹窗正文</param>
        /// <param name="ok">确定按钮文字</param>
        public static void StopApplicationInEditor(string title,string message,string ok)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false; // 停止播放模式
            EditorUtility.DisplayDialog(
                title,
                message,
                ok
            );
#endif
        }
    }
}