using Game1.States;
using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Library.Domain.Enums;

namespace Game1.Menu
{
    public class CharSelectSubMenu : SubMenu
    {
        public MenuState menuState;
        public Dictionary<int, PartialTexture> extraTextures;
        public Dictionary<PlayerIndex, int> inputDelays;
        public Dictionary<PlayerIndex, SamusColor> selectedColors;
        public new Dictionary<PlayerIndex, Control> SelectedControl;
        protected new Dictionary<PlayerIndex, List<Control>> controls;

        public CharSelectSubMenu(MenuState myMenuState, Color controlColor, Color controlBackgroundColor, GraphicsDevice graphicsDevice, SpriteFont font, Texture2D backgroundTexture, Dictionary<int, PartialTexture> extraTextures = null)
             : base(myMenuState, controlColor, controlBackgroundColor, graphicsDevice, font, backgroundTexture)
        {
            menuState = myMenuState;
            controls = new Dictionary<PlayerIndex, List<Control>>();
            SelectedControl = new Dictionary<PlayerIndex, Control>();
            selectedColors = new Dictionary<PlayerIndex, SamusColor>();
            this.extraTextures = extraTextures;
        }
        public void LoadControls(List<PlayerIndex> playerIndices, List<Control> controls)
        {
            this.controls = new Dictionary<PlayerIndex, List<Control>>();
            inputDelays = new Dictionary<PlayerIndex, int>();

            foreach (PlayerIndex index in playerIndices)
            {
                this.controls.Add(index, CharSelectControls.Controls(menuState.game, this, index));
                if (index == PlayerIndex.One)
                {
                    this.controls[index].Add(CharSelectControls.NextControl(menuState.game, this));
                    this.controls[index].Add(CharSelectControls.BackControl(menuState.game, this));
                }
                inputDelays.Add(index, (int)index);
                SelectedControl[index] = this.controls[index][(int)index];
                SelectedControl[index].Select();
                selectedColors[index] = (SamusColor)(int)index;
            }

        }

        public override void Update(Game1 game)
        {
            var gamePadStates = new Dictionary<PlayerIndex, GamePadState>();

            foreach (PlayerIndex playerIndex in controls.Keys)
            {
                gamePadStates.Add(playerIndex, GamePad.GetState(playerIndex));

                var controls = this.controls[playerIndex];
                var gamePadState = gamePadStates[playerIndex];

                if (gamePadState.Buttons.A == ButtonState.Pressed)
                {
                    SelectedControl[playerIndex].ExecuteDebouncedAction(gamePadState, game, myMenuState, this, SelectedControl[playerIndex], playerIndex);
                }
                else if (gamePadState.Buttons.B == ButtonState.Pressed || gamePadState.Buttons.Back == ButtonState.Pressed)
                {
                    menuState.SetSelectedMenu(menuState.mainMenu);
                }

                if (Math.Abs(gamePadState.ThumbSticks.Left.Y) > 0.6 && myMenuState.HoverInputDelay == 0)
                {
                    myMenuState.TriggerHoverInputDelay();
                    int increment = gamePadState.ThumbSticks.Left.Y > 0 ? 1 : -1;

                    var selectedControlIndex = controls.FindIndex(c => c.text == SelectedControl[playerIndex].text);

                    if (increment == -1)
                        SelectControl(playerIndex, selectedControlIndex == controls.Count() - 1 ? controls.First() : controls[selectedControlIndex + 1]);
                    else
                        SelectControl(playerIndex, selectedControlIndex == 0 ? controls.Last() : controls[selectedControlIndex - 1]);
                }

                foreach (Control control in controls)
                {
                    control.Update();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(backgroundTexture, new Rectangle(Point.Zero, myMenuState.game.GraphicsDevice.Viewport.Bounds.Size), Color.White);
            spriteBatch.End();
            foreach (PlayerIndex playerIndex in controls.Keys)
            {
                var controls = this.controls[playerIndex];
                foreach (Control control in controls)
                {
                    control.Draw(spriteBatch);
                }
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

                var firstControl = controls[0];

                var height = controls[SamusColorStringConversions.Count - 1].position.Y + font.MeasureString("A").Y - firstControl.position.Y;

                var destinationRectangle = new Rectangle(
                    firstControl.position.X + (int)(font.MeasureString(firstControl.text).X * 1.1f),
                    firstControl.position.Y - (int)font.MeasureString(firstControl.text).Y / 4, (int)(height * 0.75f), (int)height);

                var partialTexture = extraTextures[(int)selectedColors[playerIndex]];

                spriteBatch.Draw(
                    texture: partialTexture.sourceTexture,
                    position: destinationRectangle.Location.ToVector2(),
                    sourceRectangle: partialTexture.textureLocation,
                    color: Color.White, rotation: 0f, origin: Vector2.Zero,
                    scale: Vector2.One * destinationRectangle.Height / partialTexture.textureLocation.Height,
                    effects: SpriteEffects.None, layerDepth: 0f
                );
                spriteBatch.End();
            }
        }

        protected void SelectControl(PlayerIndex playerIndex, Control newControl)
        {
            SelectedControl[playerIndex].DeSelect();
            SelectedControl[playerIndex] = newControl;
            SelectedControl[playerIndex].Select();
        }
    }
}
