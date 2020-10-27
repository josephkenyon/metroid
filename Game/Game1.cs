using Library.Assets;
using Library.Assets.Samus;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static Library.Domain.Constants;

namespace Game1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Samus samus;
        private SpriteFont consoleFont;
        private GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.IsBorderless = true;
            Window.Position = new Point(0 + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 1920) / 2, 0 + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 1080) / 2);
            graphics.PreferredBackBufferHeight = (int)(16.875 * tileSize);
            graphics.PreferredBackBufferWidth = 30 * tileSize;
            Content.RootDirectory = "Content";
            Mouse.WindowHandle = Window.Handle;

            IsFixedTimeStep = true;  //Force the game to update at fixed time intervals
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);  //Set the time interval to 1/30th of a second
        }

        protected override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            // load content
            consoleFont = Content.Load<SpriteFont>("Console");
            samus = new Samus(Content.Load<Texture2D>("Sprites\\samusOrange"));

            // create blocks for level
            List<TerrainBlock> blocks = new List<TerrainBlock>();
            for ( int i = 0; i < 60; i++ )
            {
                for ( int l = 1; l < 8; l++ )
                {
                    blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(2, 0), new Vector2(tileSize * i, floor + (tileSize * l)), 16, true));
                };
                blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 4), new Vector2(tileSize * i, floor), 16, true));
            };

            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(6, 3), new Vector2(tileSize * 12, floor - tileSize * 2), 16, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(6, 2), new Vector2(tileSize * 12, floor - tileSize * 3), 16, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 3), new Vector2(tileSize * 13, floor - tileSize * 2), 16, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 2), new Vector2(tileSize * 13, floor - tileSize * 3), 16, true));


            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(6, 3), new Vector2(tileSize * 15, floor - tileSize * 4), 16, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(6, 2), new Vector2(tileSize * 15, floor - tileSize * 5), 16, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 3), new Vector2(tileSize * 16, floor - tileSize * 4), 16, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 2), new Vector2(tileSize * 16, floor - tileSize * 5), 16, true));

            // add to level
            gameState = new GameState(Window);
            foreach ( TerrainBlock block in blocks )
            {
                gameState.CurrentLevel.BlockMap.Add(block.CurrentQuadrant, block);
            }
            gameState.SetFocusObject(samus);
        }


        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            Vector2 MovingInput = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;

            if ( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) )
            {
                Exit();
            }

            samus.Update(gameState, GamePad.GetState(PlayerIndex.One));

            gameState.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(new Color(10, 10, 20));
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            samus.Draw(spriteBatch, gameState);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            foreach ( TerrainBlock block in gameState.CurrentLevel.BlockMap.Values )
            {
                block.Draw(spriteBatch, gameState);
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.DrawString(consoleFont, samus.CurrentAnimation.CurrentFrame.ToString(), new Vector2(10, Window.ClientBounds.Height - 20), Color.White);
            /*
                       

            Texture2D rect = new Texture2D(graphics.GraphicsDevice, tileSize, tileSize);
            Color[] data = new Color[tileSize * tileSize];
            for ( int i = 0; i < data.Length; ++i ) data[i] = Color.Chocolate;
            rect.SetData(data);

            spriteBatch.Draw(rect, new Vector2((int)(samus.Position.X / tileSize) * tileSize, samus.GetFloor()) - gameState.CameraLocation, Color.White);
            spriteBatch.Draw(rect, samus.GetCollisionBox().Location.ToVector2(), Color.White);
            spriteBatch.DrawString(consoleFont, "FPS: " + (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(10, Window.ClientBounds.Height - 20), Color.White);


            */
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
