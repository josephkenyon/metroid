using Game1.LevelManagement;
using Game1.States;
using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game1.Menu
{
    public class DecisionInputSubMenu : SubMenu
    {
        public MenuState menuState;
        public int inputDelay = 0;
        public string Message { get; private set; }
        private SubMenu previousSubMenu;
        private static List<string> options = new List<string> { "No", "Yes" };
        int selectedOption = 0;
        private string tag;
        private Action<Game1, MenuState, SubMenu, string> ExecuteYes { get; set; }

        public void TriggerInputDelay()
        {
            inputDelay = 10;
            menuState.TriggerInputDelay();
        }

        public void SetMessageAndTag(string message, string tag) {
            Message = message;
            this.tag = tag;
            TriggerInputDelay();
        }

        public DecisionInputSubMenu(MenuState myMenuState, Color controlColor, Color controlBackgroundColor,
            GraphicsDevice graphicsDevice, SpriteFont font, Texture2D backgroundTexture, SubMenu previousSubMenu,
            Action<Game1, MenuState, SubMenu, string> ExecuteYes, string message, string tag)
             : base(myMenuState, controlColor, controlBackgroundColor, graphicsDevice, font, backgroundTexture)
        {
            this.previousSubMenu = previousSubMenu;
            Message = message;
            this.tag = tag;
            this.ExecuteYes = ExecuteYes;
            menuState = myMenuState;
        }

        public override void Update(Game1 game)
        {
            if (inputDelay > 0)
                inputDelay--;

            var gamePadState = GamePad.GetState(PlayerIndex.One);

            if (Math.Abs(gamePadState.ThumbSticks.Left.X) > 0.7f && inputDelay == 0)
            {
                TriggerInputDelay();
                if (selectedOption == 0)
                    selectedOption = 1;
                else
                    selectedOption = 0;
            }
            else if ((gamePadState.Buttons.B == ButtonState.Pressed || gamePadState.Buttons.Back == ButtonState.Pressed) && inputDelay == 0)
            {
                TriggerInputDelay();
                selectedOption = 0;
                previousSubMenu.decisionDialogIsActive = false;
            }
            else if (gamePadState.Buttons.A == ButtonState.Pressed && inputDelay == 0)
            {
                TriggerInputDelay();
                if (selectedOption == 0)
                {
                    selectedOption = 0;
                    previousSubMenu.decisionDialogIsActive = false;
                }
                else
                    ExecuteYes.Invoke(game, menuState, previousSubMenu, tag);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var charSize = font.MeasureString("W").ToPoint();
            var screenSize = menuState.game.GraphicsDevice.Viewport;
            var position = new Point(
                screenSize.Width / 2,
                screenSize.Height / 2 - charSize.Y / 2
            );

            var wordLength = font.MeasureString(options[selectedOption]).ToPoint();
            var selectedOptionRectangle = new Rectangle(
                new Point(position.X + (selectedOption == 0 ? -(wordLength.X + charSize.X) : 0), position.Y),
                new Point((int)(wordLength.X * 1.1f), charSize.Y)
            );

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(controlTexture, selectedOptionRectangle, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            wordLength = font.MeasureString(options[0]).ToPoint();
            spriteBatch.DrawString(font, options[0], new Vector2(position.X - (wordLength.X + charSize.X), position.Y), controlColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, options[1], new Vector2(position.X, position.Y), controlColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.DrawString(menuState.smallMenuFont, Message, new Vector2(screenSize.Width / 2 - (int)(menuState.smallMenuFont.MeasureString(Message).X / 2), position.Y - charSize.Y), controlColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

        }

        public static void DeleteLevel(Game1 game, MenuState menuState, SubMenu previousMenu, string levelName)
        {
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
            previousMenu.decisionDialogIsActive = false;
            var levels = game.levelManager.Levels;
            levels.RemoveAt(levels.FindIndex(l => l.Name == levelName));
            LevelManager.Save(new LevelManager(levels));
            game.ReloadLevels();
            menuState.stageSelectMenuForGameMenu.LoadControls(StageSelectControls.ControlsForGame(game, menuState.mainMenu, game.GetLevels().Select(l => l.Name).ToList()));
            menuState.stageSelectMenuForEngine.LoadControls(StageSelectControls.ControlsForEngine(game, menuState.mainMenu, game.GetLevels().Select(l => l.Name).ToList()));
            previousMenu.decisionDialogIsActive = false;
        }
    }
}
