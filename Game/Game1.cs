using Library.Assets;
using Library.Assets.Samus;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            samus = new Samus(Content.Load<Texture2D>("Sprites\\samus"));

            // create blocks for level
            List<TerrainBlock> blocks = new List<TerrainBlock>();
            for ( int i = 0; i < 60; i++ )
            {
                for ( int l = 1; l < 8; l++ )
                {
                    blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(2, 0), new Vector2(tileSize * i, floor + (tileSize * l)), 64, true));
                };
                blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 4), new Vector2(tileSize * i, floor), 64, true));
            };

            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(6, 3), new Vector2(tileSize * 12, floor - tileSize * 2), 64, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(6, 2), new Vector2(tileSize * 12, floor - tileSize * 3), 64, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 3), new Vector2(tileSize * 13, floor - tileSize * 2), 64, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 2), new Vector2(tileSize * 13, floor - tileSize * 3), 64, true));


            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(6, 3), new Vector2(tileSize * 15, floor - tileSize * 4), 64, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(6, 2), new Vector2(tileSize * 15, floor - tileSize * 5), 64, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 3), new Vector2(tileSize * 16, floor - tileSize * 4), 64, true));
            blocks.Add(new TerrainBlock(Content.Load<Texture2D>("Sprites\\terrainpallete"), new Vector2(7, 2), new Vector2(tileSize * 16, floor - tileSize * 5), 64, true));

            // add to level
            gameState = new GameState();
            foreach ( TerrainBlock block in blocks )
            {
                gameState.CurrentLevel.BlockMap.Add(block.CurrentQuadrant, block);
            }
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            samus.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach ( TerrainBlock block in gameState.CurrentLevel.BlockMap.Values )
            {
                block.Draw(spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            Texture2D rect = new Texture2D(graphics.GraphicsDevice, samus.GetCollisionBox().Width, samus.GetCollisionBox().Height);
            Color[] data = new Color[samus.GetCollisionBox().Width * samus.GetCollisionBox().Height];
            for ( int i = 0; i < data.Length; ++i ) data[i] = Color.Chocolate;
            rect.SetData(data);

            // spriteBatch.Draw(rect, samus.GetCollisionBox().Location.ToVector2(), Color.White);

            spriteBatch.DrawString(consoleFont, (Mouse.GetState().Position.ToVector2() / tileSize).ToPoint().ToString(), new Vector2(10, 10), Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
