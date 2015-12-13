using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
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

        while (!Generate()) { };
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

        var point = new Point(x, y);
        var targetPoint = point.Move(direction);

        // reached the edge of the map
        if (targetPoint.X < 0 || targetPoint.X > Width - 1 || targetPoint.Y < 0 || targetPoint.Y > Height - 1)
        {
            return false;
        }

        // this is not a path, check for the neighboring path
        if ( Tiles[targetPoint.X, targetPoint.Y] == null )
        {
            try
            {
                // there should always be one that doesn't backtrack
                var dir = direction;
                targetPoint = point.GetNeighbors(Width, Height).Where(p => Tiles[p.X, p.Y] != null && p.GetDirection(point) != dir).SingleOrDefault(p => !p.Equals(point));
            }
            catch(Exception e)
            {
                return false;
            }

            // still not a path, something went wrong
            if (targetPoint == null || Tiles[targetPoint.X, targetPoint.Y] == null)
            {
                return false;
            }
            else
            {
                direction = point.GetDirection(targetPoint);
            }
        }

        x = targetPoint.X;
        y = targetPoint.Y;

        return true;
    }

    public Tile GetTile(int x, int y)
    {
        return Tiles[x, y];
    }

    private bool Generate()
    {
        Tiles = new Tile[10, 10];

        // generate out the intersections
        var intersections = new List<Intersection>();
        for(var i = 0; i < 3; i++)
        {
            var x = UnityEngine.Random.Range(1, Width - 1);
            var y = UnityEngine.Random.Range(1, Width - 1);

            if(Tiles[x, y] != null)
            {
                // duplicate location, try again
                i--;
            }
            else
            {
                var tooClose = false;
                foreach(var inter in intersections)
                {
                    if(Math.Abs(inter.x - x) <= 1 && Math.Abs(inter.y - y) <= 1)
                    {
                        //too close to an existing intersection
                        tooClose = true;
                    }
                }

                if (tooClose)
                {
                    i--;
                }
                else
                {
                    var intersection = new Intersection(x, y);
                    intersections.Add(intersection);
                    Tiles[x, y] = intersection;
                }
            }
        }

        // foreach intersection except the last one, make a path to the next intersection
        for(var i = 0; i < intersections.Count-1; i++)
        {
            var currentPoint = new Point(intersections[i].x, intersections[i].y);
            var nextIntersection = new Point(intersections[i + 1].x, intersections[i + 1].y);

            var directions = new List<int>();
            if (currentPoint.X < nextIntersection.X) directions.Add(0);
            else if (currentPoint.X > nextIntersection.X) directions.Add(2);
            if (currentPoint.Y < nextIntersection.Y) directions.Add(3);
            else if (currentPoint.Y > nextIntersection.Y) directions.Add(1);

            // while we're not at the target yet
            while(!currentPoint.Equals(nextIntersection))
            {
                // pick a random direction
                var direction = directions[UnityEngine.Random.Range(0, directions.Count)];

                // find the next location in that direction
                var targetPoint = currentPoint.Move(direction);

                if(Tiles[targetPoint.X, targetPoint.Y] == null && !targetPoint.GetNeighbors(Width, Height).Any(p => !currentPoint.Equals(p) && Tiles[p.X, p.Y] is Path))
                {
                    // if nothing is there yet, make a path tile
                    Tiles[targetPoint.X, targetPoint.Y] = new Path(targetPoint.X, targetPoint.Y);
                }
                else if(directions.Count > 1)
                {
                    direction = directions.Find(d => d != direction);

                    var targetPoint2 = currentPoint.Move(direction);

                    if (Tiles[targetPoint2.X, targetPoint2.Y] == null)
                    {
                        if(targetPoint2.GetNeighbors(Width, Height).Any(p => !currentPoint.Equals(p) && Tiles[p.X, p.Y] is Path))
                        {
                            return false;
                        }

                        // if nothing is there yet, make a path tile
                        Tiles[targetPoint2.X, targetPoint2.Y] = new Path(targetPoint2.X, targetPoint2.Y);
                        targetPoint = targetPoint2;
                    }
                    else if(Tiles[targetPoint2.X, targetPoint2.Y] is Path)
                    {
                        return false;
                    }
                }
                else if (Tiles[targetPoint.X, targetPoint.Y] is Path)
                {
                    return false;
                }

                if (targetPoint.X == nextIntersection.X)
                {
                    // reached the same x value, remove the x directions as a possibility
                    directions.Remove(0);
                    directions.Remove(2);
                }
                if (targetPoint.Y == nextIntersection.Y)
                {
                    // reached the same y value, remove the y directions as a possibility
                    directions.Remove(1);
                    directions.Remove(3);
                }

                currentPoint = targetPoint;
            }
        }

        // for every intersection, find 1 or 2 directions to draw a path out to a wall
        foreach(var inter in intersections)
        {
            var point = new Point(inter.x, inter.y);

            foreach(var validNeighbor in point.GetNeighbors(Width, Height).Where(p => Tiles[p.X, p.Y] == null && !p.GetNeighbors(Width, Height).Any(p2 => !p.Equals(p2) && Tiles[p2.X, p2.Y] is Path)).Shuffle().Take(2))
            {
                int direction = 0;

                if(validNeighbor.X < point.X)
                {
                    direction = 2;
                }
                else if(validNeighbor.Y > point.Y)
                {
                    direction = 3;
                }
                else if (validNeighbor.Y < point.Y)
                {
                    direction = 1;
                }

                var nextPoint = validNeighbor;
                while(nextPoint.X >= 0 && nextPoint.X < Width && nextPoint.Y >= 0 && nextPoint.Y < Height)
                {
                    Tiles[nextPoint.X, nextPoint.Y] = new Path(nextPoint.X, nextPoint.Y);
                    nextPoint = nextPoint.Move(direction);
                }
            }
        }

        // validation step to check for paths with more than 2 neighbors and intersections with less than 3 neighbors
        var valid = true;
        Iterate(tile =>
        {
            if (tile is Path)
            {
                var point = new Point(tile.x, tile.y);
                if(point.GetNeighbors(Width, Height).Select(p => Tiles[p.X, p.Y]).Count(t => t != null) > 2)
                {
                    valid = false;
                }
            }
            else if(tile is Intersection)
            {
                var point = new Point(tile.x, tile.y);
                if (point.GetNeighbors(Width, Height).Select(p => Tiles[p.X, p.Y]).Count(t => t != null) < 3)
                {
                    valid = false;
                }
            }
        });

        if (!valid)
        {
            return false;
        }

        var edges = new List<Tile>();
        for(var x = 0; x < Width; x++)
        {
            for(var y = 0; y < Height; y++)
            {
                if(x == 0 || x == Width-1 || y == 0 || y == Height - 1)
                {
                    if(Tiles[x, y] != null)
                    {
                        edges.Add(Tiles[x, y]);
                    }
                }
            }
        }

        if(edges.Count < 4)
        {
            return false;
        }
        else
        {
            edges = edges.Shuffle().ToList();

            var i = 0;
            foreach(var edge in edges.Take(2))
            {
                Tiles[edge.x, edge.y] = new Exit(edge.x, edge.y, Colors[i]);
                i++;
            }

            foreach(var edge in edges.Skip(2).Take(2))
            {
                Tiles[edge.x, edge.y] = new Spawn(edge.x, edge.y);
            }
        }
        
        return true;
    }
}
