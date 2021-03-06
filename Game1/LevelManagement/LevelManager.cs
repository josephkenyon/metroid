﻿using Library.Assets;
using Library.State;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Game1.LevelManagement
{
    public class LevelManager
    {
        public static string _filename = "levels.xml";

        public List<Level> Levels { get; private set; }

        public LevelManager()
            : this(new List<Level>())
        {

        }

        public LevelManager(List<Level> levels)
        {
            Levels = levels;
        }

        public static LevelManager Load()
        {
            if (!File.Exists(_filename))
                return new LevelManager();

            using var reader = new StreamReader(new FileStream(_filename, FileMode.Open));

            var serializer = new XmlSerializer(typeof(List<Level>));

            var levels = (List<Level>)serializer.Deserialize(reader);

            //levels.Find(a => a.Name == "depths").TintColor = new Color(115, 115, 255, 255);
            /*
            
            foreach (TerrainBlock block in level.BlockMap.Values)
            {
                block.Position *= 64;
            }

            foreach (TerrainBlock block in level.BackgroundBlockMap.Values)
            {
                block.Position *= 64;
            }
            */


            return new LevelManager(levels);
        }

        public static void Save(LevelManager levelManager)
        {
            using var writer = new StreamWriter(new FileStream(_filename, FileMode.Create));

            var serializer = new XmlSerializer(typeof(List<Level>));

            serializer.Serialize(writer, levelManager.Levels);

        }

        public void Sort()
        {
            Levels.Sort();
        }
    }
}
