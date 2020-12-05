using Game1.LevelManagement;
using Game1.States;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private State _currentState;

        private State _nextState;

        public float soundLevel = 0.2f;
        public Point resolution { get; private set; }
        
        public LevelManager levelManager { get; private set; }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public void ReloadLevels() { 
            levelManager = LevelManager.Load();
        }

        public void SetResolution(Point newResolution) 
        {
            resolution = newResolution;
            graphics.PreferredBackBufferWidth = resolution.X;
            graphics.PreferredBackBufferHeight = resolution.Y;
            graphics.ApplyChanges();
        }

        public void SetIsFullscreen(bool set)
        {
            graphics.IsFullScreen = set;
            graphics.ApplyChanges();
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Mouse.WindowHandle = Window.Handle;

            IsFixedTimeStep = true;  //Force the game to update at fixed time intervals
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);  //Set the time interval to 1/30th of a second
            levelManager = new LevelManager();
        }


        protected override void Initialize()
        {
            IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            resolution = new Point(1920, 1080);

            graphics.PreferredBackBufferWidth = resolution.X;
            graphics.PreferredBackBufferHeight = resolution.Y;

            Window.IsBorderless = true;
            graphics.IsFullScreen = false;
            Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 1920) / 2, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 1080) / 2);

            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            levelManager = LevelManager.Load();
            _currentState = new MenuState(this, GraphicsDevice, Content);

            /*
            var levels = new List<Level>();
            foreach (string levelName in levelNames)
            {
                var level = new Level();
                level.ReadFile(Content, levelName);
                levels.Add(level);
            }
            var levelManager = new LevelManagement.LevelManager(levels);
            LevelManagement.LevelManager.Save(levelManager);
            */

        }


        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _currentState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }

        public List<Level> GetLevels() {
            return levelManager.Levels;
        }
    }
}
