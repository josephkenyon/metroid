using Library.Assets;
using Library.Assets.Samus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Library.Domain.Constants;

namespace Library.State
{
    public class GameProperties
    {
        public Level CurrentLevel { get; private set; }
        public GameWindow Window;
        public Dictionary<PlayerIndex, Samus> players;
        public Character focusObject;
        public bool GameOver = false;
        public bool ScoreScreen = false;
        public int frameSkip = 3;
        public Vector2 DrawTransform => new Vector2(0, 0);
        public Vector2 DrawTransform2 => new Vector2(
            focusObject.Position.X + (focusObject.SpriteSize.X * tileSize / focusObject.SpriteTileSize / 2),
            focusObject.Position.Y - (focusObject.SpriteSize.Y * tileSize / focusObject.SpriteTileSize * 4))
            - new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
        public Vector2 CameraLocation { get; private set; }

        public GameProperties(GameWindow Window, Level level)
        {
            CurrentLevel = level;
            this.Window = Window;
        }

        public void Update()
        {
            CameraLocation -= (CameraLocation - DrawTransform) / 2;
        }

        public void SetFocusObject(Character focusObject)
        {
            this.focusObject = focusObject;
            CameraLocation = DrawTransform;
        }

    }
}
