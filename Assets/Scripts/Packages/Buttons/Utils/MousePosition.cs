using UnityEngine;

namespace com.startech.Buttons
{
    /// <summary>
    /// 工具类，用来获取鼠标坐标
    /// </summary>
    public static class MousePosition
    {
        // 设置深度值（相机到鼠标位置的距离），一般不用调
        const int MOUSE_DEPTH = 10;
        public static Vector3 GetMousePosition()
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = MOUSE_DEPTH; 
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            // Debug.Log(mouseScreenPosition);
            return mouseWorldPosition;
        }
    }
}
