using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    private int x;
    private int y;
    public Location(int x, int y)
    {
        this.x = x;
        this.y = y;
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
}