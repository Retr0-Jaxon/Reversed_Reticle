using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class GlowController : MonoBehaviour
{
    private Chessboard chessboard;
    [SerializeReference]
    [SerializeField]
    private List<GlowCommand> commands= new List<GlowCommand>();


    public List<GlowCommand> Commands
    {
        get => commands;
        set => commands = value;
    }

    private void Start()
    {
        chessboard = Chessboard.instance;
        StartCoroutine(PlaySequence());
    }
    private IEnumerator PlaySequence()
    {
        Main.disabledMouse();
        foreach (var command in Commands)
        {
            switch (command.CommandType)
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
                case GlowCommandType.Hint:
                    ExecuteHint(command as HintCommand);
                    // 只有当hintTime > 0时才自动取消提示
                    if (((HintCommand)command).HintTime > 0)
                    {
                        yield return new WaitForSeconds(((HintCommand)command).HintTime);
                        //ExecuteUnhint(command as HintCommand);
                    }
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

    private void ExecuteHint(HintCommand command)
    {
        foreach (var tile in command.Tiles)
        {
            tile.GetComponent<TileVisualStateManager>().SetHint();
        }
    }

    // private void ExecuteUnhint(HintCommand command)
    // {
    //     foreach (var tile in command.Tiles)
    //     {
    //         tile.GetComponent<TileVisualStateManager>().SetHint(false);
    //     }
    // }
}