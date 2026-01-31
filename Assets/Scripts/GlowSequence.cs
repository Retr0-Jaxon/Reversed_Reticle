using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlowSequence
{
    [SerializeReference]
    private List<GlowCommand> commands= new List<GlowCommand>();

    public List<GlowCommand> Commands
    {
        get => commands;
        set => commands = value;
    }
}