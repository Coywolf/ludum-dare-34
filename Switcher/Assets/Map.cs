using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets;

public class Map {

    public int Width = 10;
    public int Height = 10;

    public List<Color> Colors;

    private Tile[,] Tiles;

    public Map(int width, int height)
    {
        Width = width;
        Height = height;

        Colors = new List<Color>() { Color.red, Color.blue };

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
            
            Tiles[7, 0] = new Exit(7, 0, Color.red);
            Tiles[7, 9] = new Exit(7, 9, Color.blue);            
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
        if(Tiles[x, y] is Intersection)
        {
            direction = ((Intersection)Tiles[x, y]).Direction;
        }
        
        int targetX = x;
        int targetY = y;

        if(direction % 2 == 0)
        {
            targetX = x - (direction - 1);
        }
        else
        {
            targetY = y + direction - 2;
        }

        // reached the edge of the map
        if (targetX < 0 || targetX > Width - 1 || targetY < 0 || targetY > Height - 1)
        {
            return false;
        }

        // this is not a path, something went wrong
        if ( Tiles[targetX, targetY] == null )
        {
            return false;
        }

        x = targetX;
        y = targetY;

        return true;
    }

    public Tile GetTile(int x, int y)
    {
        return Tiles[x, y];
    }
}
