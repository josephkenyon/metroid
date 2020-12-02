using System.Collections.Generic;
using Library.Assets;
using Library.Assets.Samus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Library.Domain.Enums;
using static Library.Domain.Constants;
using Microsoft.Xna.Framework.Media;
using Library.State;
using Game1.Menu;
using Library.Domain;

namespace Game1.States
{
    public class GameState : State
    {
        private SpriteFont consoleFont;
        private SpriteFont font;
        private GameProperties gameState;
        private CharacterSounds characterSounds;
        private ScoreScreen scoreScreen;
        private Game1 game;
        private Dictionary<PlayerIndex, SamusColor> selectedColors;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content, Dictionary<PlayerIndex, SamusColor> selectedColors, Level level)
          : base(game, graphicsDevice, Content)
        {

            this.selectedColors = selectedColors;
            this.game = game;

            // load content
            consoleFont = Content.Load<SpriteFont>("Fonts\\Console");
            font = Content.Load<SpriteFont>("Fonts\\smallMenu");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = soundLevel / 1.8f;
            MediaPlayer.Play(Content.Load<Song>("Sound\\gameMusic"));

            var players = new Dictionary<PlayerIndex, Samus>();

            characterSounds = new CharacterSounds(Content);

            gameState = new GameProperties(game.Window, level) { players = players };
            gameState.CurrentLevel.LoadTextures(Content);

            foreach (PowerUpSpawner powerUpSpawner in gameState.CurrentLevel.PowerUpSpawners)
            {
                powerUpSpawner.Load(powerUpSpawner.PowerUpType, powerUpSpawner.Location, Content.Load<Texture2D>("Sprites\\powerUps"));
            }

            //selectedColors.Add(PlayerIndex.Three, SamusColor.Red);
            foreach (PlayerIndex playerIndex in selectedColors.Keys)
            {
                players.Add(playerIndex, new Samus(playerIndex, selectedColors[playerIndex], gameState.CurrentLevel.SpawnLocations[(int)playerIndex], Content, characterSounds));
            }

        }

        public void Restart()
        {
            game.ChangeState(new GameState(game, game.GraphicsDevice, game.Content, selectedColors, game.GetLevels().Find(a => a.Name == gameState.CurrentLevel.Name)));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.GraphicsDevice.Clear(gameState.CurrentLevel.BackgroundColor);

            //Draw Terrain
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            foreach (TerrainBlock block in gameState.CurrentLevel.BackgroundBlockMap.Values)
            {
                block.Draw(spriteBatch, gameState);
            }

            spriteBatch.End();

            //Draw Samus
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            foreach (Samus player in gameState.players.Values)
            {
                player.Draw(spriteBatch, gameState);
            }

            spriteBatch.End();

            //Draw PowerUps
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            foreach (PowerUpSpawner powerUpSpawner in gameState.CurrentLevel.PowerUpSpawners)
            {
                powerUpSpawner.Draw(spriteBatch, gameState);
            }

            spriteBatch.End();

            //Draw Terrain
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            foreach (TerrainBlock block in gameState.CurrentLevel.BlockMap.Values)
            {
                block.Draw(spriteBatch, gameState);
            }

            spriteBatch.End();

            //Draw Projectiles
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            foreach (Samus player in gameState.players.Values)
            {
                player.DrawWeapons(spriteBatch, gameState);
            }
            spriteBatch.End();

            if (scoreScreen != null)
            {
                scoreScreen.Draw(spriteBatch);
            }

            //Draw Samus Health bar
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            foreach (Samus player in gameState.players.Values)
            {
                player.DrawHealthBar(spriteBatch);
            }

            spriteBatch.End();

            /*
                       
            spriteBatch.DrawString(consoleFont, "Mouse: " + (Mouse.GetState().Position.ToVector2() + gameState.CameraLocation).ToString(), new Vector2(10, _game.Window.ClientBounds.Height - 20), Color.White);
            spriteBatch.DrawString(consoleFont, samus.CurrentAnimation.CurrentFrame.ToString(), new Vector2(10, Window.ClientBounds.Height - 20), Color.White);

            Texture2D rect = new Texture2D(graphics.GraphicsDevice, tileSize, tileSize);
            Color[] data = new Color[tileSize * tileSize];
            for ( int i = 0; i < data.Length; ++i ) data[i] = Color.Chocolate;
            rect.SetData(data);

            spriteBatch.Draw(rect, new Vector2((int)(samus.Position.X / tileSize) * tileSize, samus.GetFloor()) - gameState.CameraLocation, Color.White);
            spriteBatch.Draw(rect, samus.GetCollisionBox().Location.ToVector2(), Color.White);
            spriteBatch.DrawString(consoleFont, "FPS: " + (1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(10, Window.ClientBounds.Height - 20), Color.White);


            
             */
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _game.Content));
                return;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                Restart();
                return;
            }

            if (gameState.ScoreScreen)
            {
                if (scoreScreen == null)
                {
                    var list = new Dictionary<PlayerIndex, CharacterStats>();
                    foreach (Samus player in gameState.players.Values)
                    {
                        list.Add(player.playerIndex, player.characterStats);
                    }
                    scoreScreen = new ScoreScreen(_game, list, font);
                }
                else
                {
                    scoreScreen.Update();
                }
            }

            if (!gameState.GameOver || (gameState.GameOver && gameState.frameSkip == 0))
            {
                gameState.frameSkip = 3;
                foreach (PowerUpSpawner powerUpSpawner in gameState.CurrentLevel.PowerUpSpawners)
                {
                    powerUpSpawner.Update();
                }

                foreach (Samus player in gameState.players.Values)
                {
                    player.Update(gameState, gameState.GameOver ? GamePadState.Default : GamePad.GetState(player.playerIndex));
                }
            }
            else
            {
                gameState.frameSkip--;
            }

            gameState.Update();
        }
    }
}