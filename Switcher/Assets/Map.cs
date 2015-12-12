﻿using UnityEngine;
using System.Collections;
using System;
using Assets;

public class Map {

    public int Width = 10;
    public int Height = 10;

    private Tile[,] Tiles;

    public Map(int width, int height)
    {
        Width = width;
        Height = height;

        Tiles = new Tile[10, 10];

        for(var i = 0; i < 10; i++)
        {
            Tiles[4, i] = new Path(4, i);
            Tiles[7, i] = new Path(7, i);
            Tiles[i, 4] = new Path(i, 4);
            
            Tiles[4, 9] = new Spawn(4, 9);
            Tiles[0, 4] = new Spawn(0, 4);
            
            Tiles[4, 4] = new Intersection(4, 4);
            Tiles[7, 4] = new Intersection(7, 4);
            
            Tiles[7, 0] = new Exit(7, 0);
            Tiles[7, 9] = new Exit(7, 9);            
        }
    }

    public void Iterate(Action<Tile> operation)
    {
        for(var x = 0; x < Width; x++)
        {
            for(var y = 0; y < Height; y++)
            {
                if (Tiles[x, y] != null && Tiles[x, y].GetType().IsSubclassOf(typeof(Tile)) )
                {
                    operation(Tiles[x, y]);
                }
            }
        }
    }

    public bool GetNextTarget(ref int x, ref int y, ref int direction)
    {
        x = 1;
        y = 4;
        return true;
    }
}
