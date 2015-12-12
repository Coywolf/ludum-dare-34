using UnityEngine;
using System.Collections;
using System;
using Assets;

public class MapGenerator {

    public int Width = 10;
    public int Height = 10;

    private Tile[,] Map;

    public MapGenerator(int width, int height)
    {
        Width = width;
        Height = height;

        Map = new Tile[10, 10];

        for(var i = 0; i < 10; i++)
        {
            Map[4, i] = new Path(4, i);
            Map[7, i] = new Path(7, i);
            Map[i, 4] = new Path(i, 4);

            Map[4, 9] = new Spawn(4, 9);
            Map[0, 4] = new Spawn(0, 4);

            Map[4, 4] = new Intersection(4, 4);
            Map[7, 4] = new Intersection(7, 4);

            Map[7, 0] = new Exit(7, 0);
            Map[7, 9] = new Exit(7, 9);            
        }
    }

    public void Iterate(Action<Tile> operation)
    {
        for(var x = 0; x < Width; x++)
        {
            for(var y = 0; y < Height; y++)
            {
                if ( Map[x, y] != null && Map[x, y].GetType().IsSubclassOf(typeof(Tile)) )
                {
                    operation(Map[x, y]);
                }
            }
        }
    }
}
