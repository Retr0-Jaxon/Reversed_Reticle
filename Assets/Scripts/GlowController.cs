using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class GlowController : MonoBehaviour
{
    private Chessboard chessboard;
    [SerializeField]

    private GlowSequence sequence;

    public GlowSequence Sequence
    {
        get => sequence;
        set => sequence = value;
    }


    private void Start()
    {
        chessboard = Chessboard.instance;
        StartCoroutine(PlaySequence());
    }
    private IEnumerator PlaySequence()
    {
        Main.disabledMouse();
        foreach (var command in sequence.Commands)
        {
            switch (command.commandType)
            {
                case GlowCommandType.GlowUnits:
                    ExecuteGlow(command as GlowUnitsCommand);
                    yield return new WaitForSeconds(
                        ((GlowUnitsCommand)command).GlowTime
                    );
                    ExecuteUnglow(command as GlowUnitsCommand);
                    break;
                case GlowCommandType.Wait:
                    yield return new WaitForSeconds(
                        ((WaitCommand)command).WaitTime
                    );
                    break;
            }
        }
        Main.resumeMouse();
    }
    
    private void ExecuteGlow(GlowUnitsCommand command)
    {
        foreach (var unit in command.Tiles)
        {
            unit.setGlow(true);
            unit.TileType = TileType.LIGHT;
        }
    }
    
    private void ExecuteUnglow(GlowUnitsCommand command)
    {
        foreach (var unit in command.Tiles)
        {
            unit.setGlow(false);
        }
    }
    
    
    
    
}