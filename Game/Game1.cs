using Library.Assets.Samus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Library.Domain.Constants;

namespace Game1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Samus samus;
        private SpriteFont consoleFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.IsBorderless = true;
            Window.Position = new Point(0 + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 1920) / 2, 0 + (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 1080) / 2);
            graphics.PreferredBackBufferHeight = (int)(16.875 * tileSize);
            graphics.PreferredBackBufferWidth = 30 * tileSize;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            consoleFont = Content.Load<SpriteFont>("Console");
            samus = new Samus(Content.Load<Texture2D>("Sprites\\samus"));
        }


        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            Vector2 MovingInput = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            samus.Update(GamePad.GetState(PlayerIndex.One));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //spriteBatch.DrawString(consoleFont, (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(10, 10), Color.White);

            samus.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
