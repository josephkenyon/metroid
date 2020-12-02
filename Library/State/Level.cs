using Library.Assets;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.State
{
    public class Level : IComparable
    {
        public string Name { get; set; }
        public bool Deletable { get; set; }
        public SerializableDictionary<Point, TerrainBlock> BlockMap { get; set; }
        public SerializableDictionary<Point, TerrainBlock> BackgroundBlockMap { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TintColor { get; set; }
        public float Gravity { get; set; }
        public List<Point> SpawnLocations { get; set; }
        public List<PowerUpSpawner> PowerUpSpawners { get; set; }

        public void LoadTextures(ContentManager Content)
        {
            var blockTexture = Content.Load<Texture2D>("Sprites\\terrainpallete");

            foreach (TerrainBlock terrainBlock in BlockMap.Values)
            {
                terrainBlock.SetTexture(blockTexture);
            }

            foreach (TerrainBlock terrainBlock in BackgroundBlockMap.Values)
            {
                terrainBlock.SetTexture(blockTexture);
            }

        }

        public int CompareTo(object obj)
        {
            Level c1 = (Level)obj;

            return string.Compare(Name, c1.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
        }
    }
}
