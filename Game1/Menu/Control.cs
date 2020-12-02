using Game1.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1.Menu
{
    public class Control
    {
        public readonly string text;
        private readonly SubMenu menu;
        private readonly SpriteFont font;
        public bool selected { get; private set; }
        public readonly Point position;
        public MenuState menuState;
        public PlayerIndex? playerIndex;
        private Action<Game1, MenuState, SubMenu, Control, PlayerIndex?> ExecuteAction { get; set; }
        private Action<Game1, MenuState, SubMenu, Control, PlayerIndex?> ExecuteCancel { get; set; }
        public Action<Game1, MenuState, SubMenu, Control, PlayerIndex?> ExecuteHover { get; set; }

        public Control(Game1 game, SubMenu menu, string text, int index, SpriteFont font = null, PlayerIndex? playerIndex = null,
            Action<Game1, MenuState, SubMenu, Control, PlayerIndex?> ExecuteAction = null,
            Action<Game1, MenuState, SubMenu, Control, PlayerIndex?> ExecuteHover = null,
            Action<Game1, MenuState, SubMenu, Control, PlayerIndex?> ExecuteCancel = null
            )
        {
            this.menu = menu;
            this.text = text;
            this.font = font ?? menu.font;
            this.ExecuteCancel = ExecuteCancel;
            this.ExecuteAction = ExecuteAction;
            this.playerIndex = playerIndex;
            menuState = menu.myMenuState;

            position = playerIndex == null
                ? new Point(
                    menu.xCoordinate == null ? game.Window.ClientBounds.Width / 2 : (int)menu.xCoordinate,
                    (int)(game.Window.ClientBounds.Height / 2.3f) + (int)(menu.font.MeasureString("A").Y * 1.2 * index)
                  )
                : new Point(
                    (int)(game.Window.ClientBounds.Width / 8f + game.Window.ClientBounds.Width / 4f * (int)playerIndex),
                    (int)(game.Window.ClientBounds.Height / 2.3f) + (int)(this.font.MeasureString("A").Y * 1.2 * index)
                  );
        }

        public void ExecuteDebouncedAction(GamePadState gamePadState, Game1 game, MenuState menuState, SubMenu group, Control control, PlayerIndex? playerIndex)
        {
            if (menuState.InputDelay == 0)
            {
                if (gamePadState.Buttons.A == ButtonState.Pressed)
                    ExecuteAction?.Invoke(game, menuState, group, control, playerIndex);
                else
                    ExecuteCancel?.Invoke(game, menuState, group, control, playerIndex);

                menuState.TriggerInputDelay();
            }
        }

        public void DeSelect()
        {
            selected = false;
        }

        public void Select()
        {
            selected = true;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 textSize = font.MeasureString(text);
            var bounds = new Rectangle(position, (textSize + (font.MeasureString("M") * 2) * new Vector2(1, 0)).ToPoint());
            bounds.X -= bounds.Width / 2;
            bounds.Y -= bounds.Height / 2;

            if (selected)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(menu.controlTexture, bounds, Color.White);
                spriteBatch.End();
            }

            Vector2 pos = bounds.Center.ToVector2();
            Vector2 origin = textSize * 0.5f;

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.DrawString(font, text, pos, menu.controlColor, 0, origin, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }

    }
}
