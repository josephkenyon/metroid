using Game1.States;
using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static Library.Domain.Enums;

namespace Game1.Menu
{
    public class ScoreScreen
    {
        public Dictionary<PlayerIndex, CharacterStats> characterStats;
        public SpriteFont font;
        public SpriteFont smallFont;
        public Game1 game;
        public int timer = 0;
        public Texture2D whiteTexture;
        public Texture2D buttonTexture;
        public Dictionary<PlayerIndex, int> xPositions;
        private Dictionary<PlayerIndex, SamusColor> selectedColors;

        public ScoreScreen(Game1 game, Dictionary<PlayerIndex, CharacterStats> characterStats, SpriteFont font, SpriteFont smallFont, Dictionary<PlayerIndex, SamusColor> selectedColors, Texture2D buttonTexture)
        {
            this.selectedColors = selectedColors;
            this.game = game;
            this.font = font;
            this.smallFont = smallFont;
            this.characterStats = characterStats;
            whiteTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });
            this.buttonTexture = buttonTexture;

            xPositions = new Dictionary<PlayerIndex, int>
            {
                {PlayerIndex.One, (int)(game.GraphicsDevice.Viewport.Width * 1f / 8f) },
                {PlayerIndex.Two, (int)(game.GraphicsDevice.Viewport.Width * 3f / 8f) },
                {PlayerIndex.Three, (int)(game.GraphicsDevice.Viewport.Width * 5f / 8f) },
                {PlayerIndex.Four, (int)(game.GraphicsDevice.Viewport.Width * 7f / 8f) }
            };

        }

        public void Update(Game1 game, GameState gameState)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                game.ChangeState(new MenuState(game, game.GraphicsDevice, game.Content));
                return;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
            {
                game.ChangeState(new MenuState(game, game.GraphicsDevice, game.Content, selectedColors));
                return;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                gameState.Restart();
                return;
            }

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


            int yPosition = (int)(screenSize.Height / 6f);

            foreach (PlayerIndex playerIndex in characterStats.Keys)
            {
                int index = 0;
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
                var accuracy = "Accuracy: " + characterStats[playerIndex].AccuracyPercentage.ToString("0.0") + "%";
                var damageDealt = "Damage Dealt: " + characterStats[playerIndex].damageDealt.ToString("0.0");
                var hitPointsHealed = "Hitpoints Healed: " + characterStats[playerIndex].hitPointsHealed.ToString("0.0");
                DrawString(spriteBatch, new Point(xPositions[playerIndex], yPosition + index++ * (int)(font.MeasureString("A").Y * 2)), font, "Player " + playerIndex.ToString());
                DrawString(spriteBatch, new Point(xPositions[playerIndex], yPosition + index++ * (int)(font.MeasureString("A").Y * 2)), smallFont, accuracy);
                DrawString(spriteBatch, new Point(xPositions[playerIndex], yPosition + index++ * (int)(font.MeasureString("A").Y * 2)), smallFont, damageDealt);
                DrawString(spriteBatch, new Point(xPositions[playerIndex], yPosition + index++ * (int)(font.MeasureString("A").Y * 2)), smallFont, hitPointsHealed);

                spriteBatch.End();

            }


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            DrawString(spriteBatch, new Point(xPositions[PlayerIndex.One], screenSize.Height - yPosition), smallFont, "Replay Level: ");
            //spriteBatch.Draw(buttonTexture)

            DrawString(spriteBatch, new Point(xPositions[PlayerIndex.Two], screenSize.Height - yPosition), smallFont, "Pick new Stage: ");

            DrawString(spriteBatch, new Point(xPositions[PlayerIndex.Three], screenSize.Height - yPosition), smallFont, "Back to Main Menu: ");
            spriteBatch.End();

        }
    }
}
