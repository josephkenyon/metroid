using Library.Assets;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

using static Library.Domain.Constants;

namespace Library.State
{
    public class Level
    {
        public Dictionary<Point, TerrainBlock> BlockMap { get; private set; }

        public Level()
        {
            BlockMap = new Dictionary<Point, TerrainBlock>();
        }

        public Level(Dictionary<Point, TerrainBlock> BlockMap) {
            this.BlockMap = BlockMap;
        }
    }
}
