using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

[System.Serializable]
public class WaitCommand:GlowCommand
{
    [SerializeField]
    private float waitTime;

    public float WaitTime
    {
        get => waitTime;
        set => waitTime = value;
    }

    public WaitCommand():this(0)
    {
        
    }
    public WaitCommand(float waitTime)
    {
        this.waitTime = waitTime;
        commandType = GlowCommandType.Wait;
    }

}