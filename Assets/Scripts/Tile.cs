using System.Collections;
using System.Collections.Generic;
using com.startech.Buttons;
using Enums;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private TileType tileType;
    
    private TileVisualStateManager tileVisualStateManager;

    private MaskBlock parentMaskBlock;

    public MaskBlock ParentMaskBlock
    {
        get => parentMaskBlock;
        set => parentMaskBlock = value;
    }

    public TileVisualStateManager TileVisualStateManager
    {
        get => tileVisualStateManager;
        set => tileVisualStateManager = value;
    }

    /**
     * 发光的组号
     */
    private int glowGroup;
    

    [SerializeField]
    private int x;
    [SerializeField]
    private int y;

    public TileType TileType
    {
        get => tileType;
        set => tileType = value;
    }

    public int X
    {
        get => x;
        set => x = value;
    }

    public int Y
    {
        get => y;
        set => y = value;
    }
    
    private void Awake()
    {
        tileVisualStateManager = GetComponent<TileVisualStateManager>();
    }

    public int GlowGroup
    {
        get => glowGroup;
        set => glowGroup = value;
    }
    [SerializeField]
    private bool isSelected=false;
    public bool IsSelected { get=>isSelected;private set=>isSelected=value; }

    


    public void onClick()
    {
        onSelected();
        if (Main.OperateMode==OperateMode.ClickMode)
        {
            
        }
        else if (Main.OperateMode==OperateMode.DragMode)
        {
            if (parentMaskBlock!=null)
            {
                parentMaskBlock.OnClick();
            }
            
        }
        
        
    }

    public void onSelected()
    {
        if (IsSelected)
        {
            unSelect();
        }
        else
        {
            select();
        }
    }


    public void select()
    {
        IsSelected = true;
        tileVisualStateManager.OnSelectedEffect();
        GameManager.instance.checkLevelComplete();
    }
    
    public void unSelect()
    {
        IsSelected = false;
        tileVisualStateManager.UnSelectedEffect();
        
        
    }

    /**
     * 发光方法
     */
    public void glow()
    {
        tileVisualStateManager.SetLuminous( true);
        
    }
    
    public void stopGlow()
    {
        tileVisualStateManager.SetLuminous(false);
    }


    public void setGlow(bool glow)
    {
        tileVisualStateManager.SetLuminous(glow);
    }

    public void setHint()
    {
        tileVisualStateManager.SetHint();
    }





}