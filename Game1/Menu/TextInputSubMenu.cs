using Game1.States;
using Library.Domain;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using static Library.Domain.Enums;

namespace Game1.Menu
{
    public class TextInputSubMenu : SubMenu
    {
        public MenuState menuState;
        public int stringCharacterIndex;
        public int characterListIndex;
        public List<char> characterList;
        public List<char> stringCharacterList;
        public int inputDelay = 0;
        public string Message { get; private set; }
        private SubMenu previousSubMenu;
        private Action<Game1, MenuState, string> ExecuteComplete { get; set; }
        public string InputString => new string(stringCharacterList.ToArray());

        public void TriggerInputDelay()
        {
            inputDelay = 10;
        }

        public TextInputSubMenu(MenuState myMenuState, Color controlColor, Color controlBackgroundColor,
            GraphicsDevice graphicsDevice, SpriteFont font, Texture2D backgroundTexture, SubMenu previousSubMenu,
            Action<Game1, MenuState, string> ExecuteComplete, string message)
             : base(myMenuState, controlColor, controlBackgroundColor, graphicsDevice, font, backgroundTexture)
        {
            Message = message;
            this.ExecuteComplete = ExecuteComplete;
            this.previousSubMenu = previousSubMenu;
            menuState = myMenuState;
            characterList = "abcdefghijklmnopqrstuvwxyz ".ToList();
            stringCharacterList = new List<char> { 'a' };
        }

        public override void Update(Game1 game)
        {
            if (inputDelay > 0)
                inputDelay--;

            var gamePadState = GamePad.GetState(PlayerIndex.One);

            if (gamePadState.ThumbSticks.Left.Y > 0.7f && inputDelay == 0)
            {
                TriggerInputDelay();
                characterListIndex = characterListIndex == 0 ? characterList.Count - 1 : characterListIndex - 1;
                stringCharacterList[stringCharacterIndex] = characterList[characterListIndex];
            }
            else if (gamePadState.ThumbSticks.Left.Y < -0.7f && inputDelay == 0)
            {
                TriggerInputDelay();
                characterListIndex = characterListIndex == characterList.Count - 1 ? 0 : characterListIndex + 1;
                stringCharacterList[stringCharacterIndex] = characterList[characterListIndex];
            }
            else if (gamePadState.ThumbSticks.Left.X > 0.7f && inputDelay == 0)
            {
                TriggerInputDelay();
                if (stringCharacterIndex == stringCharacterList.Count - 1)
                {
                    stringCharacterList.Add(characterList[characterListIndex]);
                    stringCharacterIndex++;
                }
                else
                    stringCharacterIndex++;
            }
            else if (gamePadState.ThumbSticks.Left.X < -0.7f && inputDelay == 0)
            {
                TriggerInputDelay();
                stringCharacterIndex = stringCharacterIndex == 0 ? stringCharacterIndex : stringCharacterIndex - 1;
            }
            else if (gamePadState.Buttons.B == ButtonState.Pressed && inputDelay == 0)
            {
                TriggerInputDelay();
                if (stringCharacterList.Count > 1)
                {
                    stringCharacterList.RemoveAt(stringCharacterIndex);
                    stringCharacterIndex--;
                }
            }
            else if (gamePadState.Buttons.Back == ButtonState.Pressed && inputDelay == 0)
            {
                TriggerInputDelay();
                menuState.TriggerInputDelay();
                menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                characterList = "abcdefghijklmnopqrstuvwxyz ".ToList();
                stringCharacterIndex = 0;
                stringCharacterList = new List<char> { 'a' };
                previousSubMenu.inputDialogIsActive = false;
            }
            else if (gamePadState.Buttons.Start == ButtonState.Pressed && inputDelay == 0)
            {
                TriggerInputDelay();
                menuState.TriggerInputDelay();
                if (stringCharacterList.Count > 0)
                {
                    menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                    menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                    ExecuteComplete.Invoke(game, menuState, InputString);
                }
                else
                {
                    menuState.controlSelectFailSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (stringCharacterIndex < 0) stringCharacterIndex = 0;
            var charSize = font.MeasureString(stringCharacterList[stringCharacterIndex].ToString()).ToPoint();
            var screenSize = menuState.game.GraphicsDevice.Viewport;
            var position = new Point(
                screenSize.Width / 2 - (int)(font.MeasureString(InputString).X / 2),
                screenSize.Height / 2 - charSize.Y / 2
            );

            var selectedCharRectangle = new Rectangle(
                new Point(position.X + (int)font.MeasureString(new string(stringCharacterList.GetRange(0, stringCharacterIndex).ToArray())).X, position.Y),
                new Point((int)(charSize.X * 1.1f), charSize.Y)
            );

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(controlTexture, selectedCharRectangle, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.DrawString(font, InputString, position.ToVector2(), controlColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.DrawString(menuState.smallMenuFont, Message, new Vector2(screenSize.Width / 2 - (int)(menuState.smallMenuFont.MeasureString(Message).X / 2), position.Y - charSize.Y), controlColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();

        }

        public static void CreateNewLevel(Game1 game, MenuState menuState, string levelName)
        {
            menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
            MediaPlayer.Stop();
            game.ChangeState(new EngineState(game, game.GraphicsDevice, game.Content, levelName));
        }
    }
}
