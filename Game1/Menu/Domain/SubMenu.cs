using Game1.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game1.Menu
{
    public class SubMenu
    {
        protected List<Control> controls;
        public readonly MenuState myMenuState;
        public Control SelectedControl { get; protected set; }
        public readonly SpriteFont font;
        public Texture2D backgroundTexture;
        public Texture2D controlTexture;
        public readonly Color controlBackgroundColor;
        public readonly Color controlColor;
        public readonly int? xCoordinate;
        public SubMenu InputDialog { get; private set; }
        public SubMenu DecisionDialog { get; private set; }
        public bool inputDialogIsActive = false;
        public bool decisionDialogIsActive = false;

        public SubMenu(MenuState myMenuState, Color controlColor, Color controlBackgroundColor, GraphicsDevice graphicsDevice, SpriteFont font, Texture2D backgroundTexture, int? xCoordinate = null, SubMenu inputDialog = null)
        {
            this.myMenuState = myMenuState;
            this.controlColor = controlColor;
            this.controlBackgroundColor = controlBackgroundColor;
            this.font = font;
            this.xCoordinate = xCoordinate;

            controlTexture = new Texture2D(graphicsDevice, 1, 1);

            controlTexture.SetData(new Color[] { controlBackgroundColor });

            this.backgroundTexture = backgroundTexture;
        }

        public void SetInputDialog(SubMenu inputDialog) { 
            InputDialog = inputDialog;
        }
        public void SetDecisionDialog(SubMenu decisionDialog)
        {
            DecisionDialog = decisionDialog;
        }

        public virtual void LoadControls(List<Control> controls)
        {
            this.controls = controls;
            SelectedControl = controls.First();
            SelectedControl.Select();
        }

        public virtual void Update(Game1 game)
        {
            if (inputDialogIsActive) {
                InputDialog.Update(game);
                return;
            }
            if (decisionDialogIsActive)
            {
                DecisionDialog.Update(game);
                return;
            }
            var gamePadState = GamePad.GetState(PlayerIndex.One);

            if (gamePadState.Buttons.A == ButtonState.Pressed || gamePadState.Buttons.B == ButtonState.Pressed)
            {
                SelectedControl.ExecuteDebouncedAction(gamePadState, game, myMenuState, this, SelectedControl, null);
                return;
            }

            if (Math.Abs(gamePadState.ThumbSticks.Left.Y) > 0.6 && myMenuState.HoverInputDelay == 0)
            {
                myMenuState.TriggerHoverInputDelay();
                int increment = gamePadState.ThumbSticks.Left.Y > 0 ? 1 : -1;

                var selectedControlIndex = controls.FindIndex(c => c.text == SelectedControl.text);

                if (increment == -1)
                    SelectControl(selectedControlIndex == controls.Count() - 1 ? controls.First() : controls[selectedControlIndex + 1]);
                else
                    SelectControl(selectedControlIndex == 0 ? controls.Last() : controls[selectedControlIndex - 1]);
            }

            foreach (Control control in controls)
            {
                control.Update();
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var viewport = myMenuState.game.GraphicsDevice.Viewport;
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(backgroundTexture, new Rectangle(Point.Zero, viewport.Bounds.Size), Color.White);
            spriteBatch.End();
            if (controls != null)
            {
                foreach (Control control in controls)
                {
                    control.Draw(spriteBatch);
                }
            }

            if (inputDialogIsActive) {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(controlTexture, new Rectangle((viewport.Bounds.Size.ToVector2() / 4f).ToPoint(), (viewport.Bounds.Size.ToVector2() / 2f).ToPoint()), Color.Black * 0.75f);
                spriteBatch.End();
                InputDialog.Draw(spriteBatch);
            }
            if (decisionDialogIsActive)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(controlTexture, new Rectangle((viewport.Bounds.Size.ToVector2() / 4f).ToPoint(), (viewport.Bounds.Size.ToVector2() / 2f).ToPoint()), Color.Black * 0.75f);
                spriteBatch.End();
                DecisionDialog.Draw(spriteBatch);
            }
        }
        protected void SelectControl(Control newControl)
        {
            SelectedControl.DeSelect();
            SelectedControl = newControl;
            SelectedControl.Select();
        }

    }
}
