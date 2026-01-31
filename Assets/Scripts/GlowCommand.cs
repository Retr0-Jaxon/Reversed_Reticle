using Enums;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public abstract class GlowCommand
{ 
    [SerializeField]
    [HideInInspector]
    private GlowCommandType commandType;

    public GlowCommandType CommandType
    {
        get => commandType;
        set => commandType = value;
    }
}