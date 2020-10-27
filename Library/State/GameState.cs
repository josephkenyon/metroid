using Library.Assets;
using Microsoft.Xna.Framework;
using static Library.Domain.Constants;

namespace Library.State
{
    public class GameState
    {
        public Level CurrentLevel { get; private set; }
        public GameWindow Window;
        public Character focusObject;
        public Vector2 DrawTransform => new Vector2(
            focusObject.Position.X - (focusObject.SpriteSize.X * focusObject.SpriteTileSize * tileSize / focusObject.SpriteTileSize / 2),
            focusObject.Position.Y - (focusObject.SpriteSize.Y * focusObject.SpriteTileSize * tileSize / focusObject.SpriteTileSize))
            - new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2)
            + new Vector2(focusObject.SpriteNumber.X * tileSize / 0.078f / tileSize, focusObject.SpriteNumber.Y * tileSize / 0.078f / tileSize);
        public Vector2 CameraLocation { get; private set; }
        private Vector2 CameraSpeed =>  new Vector2(tileSize / 4f, tileSize / 2f);

        public GameState(GameWindow Window)
        {
            CurrentLevel = new Level();
            this.Window = Window;
        }

        public void Update()
        {
            /*if ( CameraLocation.X < DrawTransform.X )
            {
                CameraLocation = CameraLocation + new Vector2(CameraSpeed.X, 0);
                if ( CameraLocation.X > DrawTransform.X )
                {
                    CameraLocation = new Vector2(DrawTransform.X, CameraLocation.Y);
                }
            }
            else if ( CameraLocation.X > DrawTransform.X )
            {
                CameraLocation = CameraLocation - new Vector2(CameraSpeed.X, 0);
                if ( CameraLocation.X < DrawTransform.X )
                {
                    CameraLocation = new Vector2(DrawTransform.X, CameraLocation.Y);
                }
            }

            if ( CameraLocation.Y < DrawTransform.Y )
            {
                CameraLocation = CameraLocation + new Vector2(0, CameraSpeed.Y);
                if ( CameraLocation.Y > DrawTransform.Y )
                {
                    CameraLocation = new Vector2(CameraLocation.X, DrawTransform.Y);
                }
            }
            else if ( CameraLocation.Y > DrawTransform.Y )
            {
                CameraLocation = CameraLocation - new Vector2(0, CameraSpeed.Y);
                if ( CameraLocation.Y < DrawTransform.Y )
                {
                    CameraLocation = new Vector2(CameraLocation.X, DrawTransform.Y);
                }
            }*/

            CameraLocation -= (CameraLocation - DrawTransform) / 2;

        }

        public void SetFocusObject(Character focusObject)
        {
            this.focusObject = focusObject;
            CameraLocation = DrawTransform;
        }

    }
}
