using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
public class Main
{
    public static bool MouseEnabled=true;

    public static OperateMode OperateMode => GameManager.OperateMode;


    public static void disabledMouse()
    {
        MouseEnabled=false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        
    }

    public static void resumeMouse()
    {
        MouseEnabled=true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
}