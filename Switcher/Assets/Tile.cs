﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Tile
    {
        public int x;
        public int y;

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Path : Tile
    {
        public Path(int x, int y) : base(x, y) { }
    }

    public class Spawn : Tile
    {
        public Spawn(int x, int y) : base(x, y) { }
    }

    public class Exit : Tile
    {
        public Exit(int x, int y) : base(x, y) { }
    }

    public class Intersection : Tile
    {
        public Transform Instance;

        public Intersection(int x, int y) : base(x, y) { }
    }
}