using UnityEngine;
using System.Collections;
using System;

public class MapGenerator {

    public int Width = 10;
    public int Height = 10;

    private int[,] Map;

    public MapGenerator(int width, int height)
    {
        Width = width;
        Height = height;

        Map = new int[10, 10];

        for(var i = 0; i < 10; i++)
        {
            Map[4, i] = 1;
            Map[7, i] = 1;
            Map[i, 4] = 1;
        }
    }

    public void Iterate(Action<int, int> operation)
    {
        for(var x = 0; x < Width; x++)
        {
            for(var y = 0; y < Height; y++)
            {
                if (Map[x, y] == 1)
                {
                    operation(x, y);
                }
            }
        }
    }
}
