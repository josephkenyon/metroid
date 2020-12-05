using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Assets;
using Library.Assets.Samus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Library.Domain.Enums;
using static Library.Domain.Constants;
using System.IO;
using Library.State;
using Game1.LevelManagement;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.Xna.Framework.Audio;
using Library.Domain;

namespace Game1.States
{
    public class EditorState : State
    {
        LevelManager levelManager;
        Level level;
        const int tileSize = 40;
        private Texture2D terrainPallete;
        private Texture2D powerUpTexture;
        private List<Color> colors;
        private Game1 game;
        private int backgroundColorIndex = 0;
        private Point cursorLocation = Point.Zero;
        private Point spriteLocation = Point.Zero;
        private SoundEffect successSoundEffect;
        private Texture2D whiteTexture;
        private readonly Point empty = new Point(-1, -1);
        private int inputDelay = 6;

        public EditorState(Game1 game, GraphicsDevice graphicsDevice, ContentManager Content, string levelName = "")
          : base(game, graphicsDevice, Content)
        {
            levelManager = game.levelManager;
            this.game = game;
            level = new Level();
            whiteTexture = new Texture2D(_graphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });

            successSoundEffect = Content.Load<SoundEffect>("Sound\\controlSelect");

            level.PowerUpSpawners = new List<PowerUpSpawner>();
            level.BackgroundColor = Color.CornflowerBlue;
            level.SpawnLocations = new List<Point> {
                empty,
                empty,
                empty,
                empty
            };

            colors = backgroundColors;

            if (levelManager.Levels.Select(l => l.Name).Contains(levelName))
            {
                level = levelManager.Levels.Find(l => l.Name == levelName);
            }
            else
            {
                level.BackgroundBlockMap = new SerializableDictionary<Point, TerrainBlock>();
                level.BlockMap = new SerializableDictionary<Point, TerrainBlock>();
                level.Name = levelName;
                level.Deletable = true;
                level.TintColor = Color.White;
                level.Gravity = 0.0084f;
            }
            level.LoadTextures(Content);

            powerUpTexture = Content.Load<Texture2D>("Sprites\\powerUps");
            foreach (PowerUpSpawner powerUpSpawner in level.PowerUpSpawners)
            {
                powerUpSpawner.Load(powerUpSpawner.PowerUpType, powerUpSpawner.Location, powerUpTexture, tileSize);
            }

            terrainPallete = Content.Load<Texture2D>("Sprites\\terrainPallete");

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(level.BackgroundColor);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            spriteBatch.Draw(
                texture: terrainPallete,
                position: cursorLocation.ToVector2() * tileSize - new Vector2(0, 4 * tileSize / 16f),
                sourceRectangle: new Rectangle((spriteLocation.ToVector2() * 16).ToPoint(), new Point(16, 16)),
                color: Color.White * 0.35f,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: tileSize / 16f,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );

            spriteBatch.Draw(
                texture: terrainPallete,
                position: new Vector2(tileSize * 41f, tileSize * 2),
                sourceRectangle: null,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 2f,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            for (int i = 0; i < 40; i++)
            {
                for (int l = 0; l < 23; l++)
                {
                    if (level.BackgroundBlockMap.ContainsKey(new Point(i, l)) && new Point(i, l) != cursorLocation)
                    {
                        var terrainBlock = level.BackgroundBlockMap[new Point(i, l)];
                        spriteBatch.Draw(
                            texture: terrainBlock.Texture,
                            position: new Vector2(i, l) * tileSize - new Vector2(0, 4 * tileSize / 16f),
                            sourceRectangle: new Rectangle((terrainBlock.SpriteLocation * 16f).ToPoint(), new Point(16, 16)),
                            color: Color.White * 0.5f,
                            rotation: 0f,
                            origin: Vector2.Zero,
                            scale: tileSize / 16f,
                            effects: SpriteEffects.None,
                            layerDepth: 0f
                        );
                    }
                }
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            for (int i = 0; i < 40; i++)
            {
                for (int l = 0; l < 23; l++)
                {
                    if (level.BlockMap.ContainsKey(new Point(i, l)) && new Point(i, l) != cursorLocation)
                    {
                        var terrainBlock = level.BlockMap[new Point(i, l)];
                        spriteBatch.Draw(
                            texture: terrainBlock.Texture,
                            position: new Vector2(i, l) * tileSize - new Vector2(0, 4 * tileSize / 16f),
                            sourceRectangle: new Rectangle((terrainBlock.SpriteLocation * 16f).ToPoint(), new Point(16, 16)),
                            color: terrainBlock.Impenetrable ? Color.White : Color.White * 0.75f,
                            rotation: 0f,
                            origin: Vector2.Zero,
                            scale: tileSize / 16f,
                            effects: SpriteEffects.None,
                            layerDepth: 0f
                        );
                    }
                }
            }

            foreach (PowerUpSpawner powerUpSpawner in level.PowerUpSpawners)
            {
                if (powerUpSpawner.Location != cursorLocation)
                {
                    spriteBatch.Draw(
                        texture: powerUpTexture,
                        position: powerUpSpawner.Location.ToVector2() * tileSize - new Vector2(0, 4 * tileSize / 16f),
                        sourceRectangle: new Rectangle((new PowerUpProperties(powerUpSpawner.PowerUpType).DrawCoordinates.ToVector2() * 16).ToPoint(), new Point(16, 16)),
                        color: Color.White,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: tileSize / 16f,
                        effects: SpriteEffects.None,
                        layerDepth: 0f
                    );
                }
            }

            foreach (Point spawnLocation in level.SpawnLocations)
            {
                if (spawnLocation != empty)
                    spriteBatch.Draw(
                        texture: whiteTexture,
                        position: spawnLocation.ToVector2() * tileSize - new Vector2(0, 4 * tileSize / 16f),
                        sourceRectangle: new Rectangle(0, 0, 1, 1),
                        color: Color.Aqua,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: tileSize,
                        effects: SpriteEffects.None,
                        layerDepth: 0f
                    );
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(
                texture: whiteTexture,
                position: new Vector2(tileSize * 41f, tileSize * 2) + (spriteLocation.ToVector2() * 32),
                sourceRectangle: null,
                color: Color.Red * 0.2f,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 32,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
            spriteBatch.End();

        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public bool PointIsEmpty(Point point)
        {
            return !level.SpawnLocations.Contains(point) && !level.PowerUpSpawners.Select(p => p.Location).Contains(point);
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                _game.ReloadLevels();
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }

            if (inputDelay > 0)
                inputDelay--;
            else
            {

                if (Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X) > 0.7)
                {
                    var direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0 ? 1 : -1;
                    cursorLocation.X += direction;

                    if (cursorLocation.X < 0 || cursorLocation.X >= 40)
                        cursorLocation.X -= direction;

                    inputDelay = 6;
                }
                else if (Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y) > 0.7)
                {
                    var direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0 ? 1 : -1;
                    cursorLocation.Y += direction;

                    if (cursorLocation.Y < 0 || cursorLocation.Y >= 23)
                        cursorLocation.Y -= direction;

                    inputDelay = 6;
                }
                else if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed && backgroundColorIndex > 0)
                {
                    backgroundColorIndex--;
                    level.BackgroundColor = colors[backgroundColorIndex];
                    inputDelay = 6;
                }
                else if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed && backgroundColorIndex < colors.Count - 1)
                {
                    backgroundColorIndex++;
                    level.BackgroundColor = colors[backgroundColorIndex];
                    inputDelay = 6;
                }

                if (Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X) > 0.7)
                {
                    var direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0 ? 1 : -1;
                    spriteLocation.X += direction;

                    if (spriteLocation.X < 0 || spriteLocation.X >= 8)
                        spriteLocation.X -= direction;

                    inputDelay = 6;
                }
                else if (Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y) > 0.7)
                {
                    var direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < 0 ? 1 : -1;
                    spriteLocation.Y += direction;

                    if (spriteLocation.Y < 0 || spriteLocation.Y >= 10)
                        spriteLocation.Y -= direction;

                    inputDelay = 6;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && PointIsEmpty(cursorLocation))
                {
                    level.BlockMap[cursorLocation] = new TerrainBlock(
                         terrainPallete, spriteLocation.ToVector2(), cursorLocation.ToVector2(), 16, true, tileSize);
                    inputDelay = 6;
                    successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                }
                else if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed && PointIsEmpty(cursorLocation))
                {
                    level.BlockMap[cursorLocation] = new TerrainBlock(
                         terrainPallete, spriteLocation.ToVector2(), cursorLocation.ToVector2(), 16, false, tileSize);
                    successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                    inputDelay = 6;
                }
                else if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed && PointIsEmpty(cursorLocation))
                {
                    level.BackgroundBlockMap[cursorLocation] = new TerrainBlock(
                         terrainPallete, spriteLocation.ToVector2(), cursorLocation.ToVector2(), 16, false, tileSize, true);
                    successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                    inputDelay = 6;
                }
                else if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                {
                    level.BlockMap.Remove(cursorLocation);
                    level.BackgroundBlockMap.Remove(cursorLocation);
                    if (level.SpawnLocations.Contains(cursorLocation))
                    {
                        level.SpawnLocations[level.SpawnLocations.FindIndex(a => a == cursorLocation)] = empty;
                    }
                    else if (level.PowerUpSpawners.Select(p => p.Location).Contains(cursorLocation))
                    {
                        level.PowerUpSpawners.RemoveAt(level.PowerUpSpawners.FindIndex(p => p.Location == cursorLocation));
                    }

                    inputDelay = 6;
                    successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                {
                    if (PointIsEmpty(cursorLocation))
                    {
                        var powerUpSpawner = new PowerUpSpawner();
                        powerUpSpawner.Load(PowerUpType.Health, cursorLocation, powerUpTexture, tileSize);
                        level.PowerUpSpawners.Add(powerUpSpawner);
                        successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                    }
                    inputDelay = 6;
                    successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                {
                    if (PointIsEmpty(cursorLocation))
                    {
                        var powerUpSpawner = new PowerUpSpawner();
                        powerUpSpawner.Load(PowerUpType.RocketAmmo, cursorLocation, powerUpTexture, tileSize);
                        level.PowerUpSpawners.Add(powerUpSpawner);
                        successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                    }
                    inputDelay = 6;
                    successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                {
                    if (level.SpawnLocations.Contains(empty))
                    {
                        var index = level.SpawnLocations.FindIndex(a => a == empty);
                        level.SpawnLocations[index] = cursorLocation;
                    }
                    inputDelay = 6;
                    successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                }
                else if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                {
                    var levels = levelManager.Levels;
                    if (levels.Select(l => l.Name).Contains(level.Name))
                    {
                        levels[levels.FindIndex(l => l.Name == level.Name)] = level;
                    }
                    else
                    {
                        levels.Add(level);
                    }
                    LevelManager.Save(new LevelManager(levels));
                    inputDelay = 6;
                    successSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
                }
            }
        }
    }
}