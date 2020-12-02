using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1.Menu
{
    public class ScoreScreen
    {
        public Dictionary<PlayerIndex, CharacterStats> characterStats;
        public SpriteFont font;
        public Game1 game;
        public int timer = 0;
        public Texture2D whiteTexture;
        public Dictionary<PlayerIndex, int> xPositions;

        public ScoreScreen(Game1 game, Dictionary<PlayerIndex, CharacterStats> characterStats, SpriteFont font)
        {
            this.game = game;
            this.font = font;
            this.characterStats = characterStats;
            whiteTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });

            xPositions = new Dictionary<PlayerIndex, int>
            {
                {PlayerIndex.One, (int)(game.GraphicsDevice.Viewport.Width * 1f / 8f) },
                {PlayerIndex.Two, (int)(game.GraphicsDevice.Viewport.Width * 3f / 8f) },
                {PlayerIndex.Three, (int)(game.GraphicsDevice.Viewport.Width * 5f / 8f) },
                {PlayerIndex.Four, (int)(game.GraphicsDevice.Viewport.Width * 7f / 8f) }
            };

        }

        public void Update()
        {
            if (timer < 75)
            {
                timer++;
            }
        }

        internal void DrawString(SpriteBatch spriteBatch, Point pos, SpriteFont font, string str)
        {
            var position = pos.ToVector2();
            position -= font.MeasureString(str) / 2f;

            spriteBatch.DrawString(font, str, position, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float color = timer / 100f;

            var screenSize = game.GraphicsDevice.Viewport;
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(whiteTexture, new Rectangle(Point.Zero, screenSize.Bounds.Size.ToVector2().ToPoint()), Color.Black * color);
            spriteBatch.End();


            foreach (PlayerIndex playerIndex in characterStats.Keys)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
                var accuracy = "Accuracy: " + characterStats[playerIndex].AccuracyPercentage.ToString("0.0") + "%";
                DrawString(spriteBatch, new Point(xPositions[playerIndex], (int)(screenSize.Height / 6f)), font, "Player " + playerIndex.ToString());
                DrawString(spriteBatch, new Point(xPositions[playerIndex], (int)(screenSize.Height / 3f)), font, accuracy);
                spriteBatch.End();
            }

        }
    }
}
