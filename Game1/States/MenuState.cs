using System;
using System.Collections.Generic;
using System.Linq;
using Game1.Menu;
using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static Library.Domain.Enums;

namespace Game1.States
{
    public class MenuState : State
    {
        public readonly Game1 game;

        public readonly SubMenu mainMenu;
        public readonly SubMenu stageSelectMenuForGame;
        public readonly SubMenu stageSelectMenuForEngine;
        public readonly CharSelectSubMenu charSelectMenu;

        public readonly SpriteFont menuFont;
        public readonly SpriteFont smallMenuFont;
        public readonly SpriteFont mediumMenuFont;

        public readonly SoundEffect controlSelectFailSoundEffect;
        public readonly SoundEffect controlSelectSoundEffect;
        private readonly Song menuMusic;
        public int InputDelay { get; protected set; } = 0;
        public int HoverInputDelay { get; protected set; } = 0;

        public List<PlayerIndex> PlayerIndices { get; private set; }
        public SubMenu selectedMenu { get; private set; }
        public void SetSelectedMenu(SubMenu newMenu)
        {
            selectedMenu = newMenu;
        }

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            this.game = game;

            PlayerIndices = GetPlayerIndices();

            menuFont = content.Load<SpriteFont>("Fonts\\menu");
            smallMenuFont = content.Load<SpriteFont>("Fonts\\smallMenu");
            mediumMenuFont = content.Load<SpriteFont>("Fonts\\mediumMenu");
            menuMusic = content.Load<Song>("Sound\\menuMusic");
            controlSelectSoundEffect = content.Load<SoundEffect>("Sound\\controlSelect");
            controlSelectFailSoundEffect = content.Load<SoundEffect>("Sound\\controlSelectFail");

            mainMenu = new SubMenu(
                this, Color.White, Color.Crimson, graphicsDevice, menuFont,
                content.Load<Texture2D>("Sprites\\titleScreen")
            );
            mainMenu.LoadControls(MainMenuControls.Controls(game, mainMenu));


            var textureDictionary = new Dictionary<int, PartialTexture>();
            foreach (SamusColor samusColor in SamusColorStringConversions.Values)
                textureDictionary.Add((int)samusColor, new PartialTexture(content.Load<Texture2D>("Sprites\\Samus\\samus" + samusColor.ToString()), new Rectangle(808, 118, 48, 64)));

            charSelectMenu = new CharSelectSubMenu(
                this, Color.White, Color.Crimson, graphicsDevice, smallMenuFont, content.Load<Texture2D>("Sprites\\titleScreen"),
                textureDictionary);
            charSelectMenu.LoadControls(PlayerIndices, CharSelectControls.Controls(game, mainMenu));

            stageSelectMenuForGame = new SubMenu(
                this, Color.White, Color.Crimson, graphicsDevice, menuFont,
                content.Load<Texture2D>("Sprites\\titleScreen")
            );
            stageSelectMenuForGame.LoadControls(StageSelectControls.ControlsForGame(game, mainMenu, game.GetLevels().Select(l => l.Name).ToList()));

            stageSelectMenuForEngine = new SubMenu(
                this, Color.White, Color.Crimson, graphicsDevice, mediumMenuFont,
                content.Load<Texture2D>("Sprites\\titleScreen")
            );
            stageSelectMenuForEngine.SetInputDialog(
                new TextInputSubMenu(
                    this, Color.White, Color.Crimson, graphicsDevice, menuFont,
                    content.Load<Texture2D>("Sprites\\titleScreen"), stageSelectMenuForEngine, TextInputSubMenu.CreateNewLevel, "Enter a name:"
                )
            );
            stageSelectMenuForEngine.SetDecisionDialog(
                new DecisionInputSubMenu(
                    this, Color.White, Color.Crimson, graphicsDevice, menuFont,
                    content.Load<Texture2D>("Sprites\\titleScreen"), stageSelectMenuForEngine, DecisionInputSubMenu.DeleteLevel, "Are you sure you want to delete ", ""
                )
            );
            stageSelectMenuForEngine.LoadControls(StageSelectControls.ControlsForEngine(game, stageSelectMenuForEngine, game.GetLevels().Select(l => l.Name).ToList()));

            selectedMenu = mainMenu;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Constants.soundLevel < 0.5f ? Constants.soundLevel * 2 : Constants.soundLevel;
            MediaPlayer.Play(menuMusic);
        }
        public void TriggerInputDelay()
        {
            InputDelay = 12;
        }
        public void TriggerHoverInputDelay()
        {
            HoverInputDelay = 12;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.GraphicsDevice.Clear(Color.Black);

            selectedMenu.Draw(spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            var players = GetPlayerIndices();

            if (InputDelay > 0)
                InputDelay--;

            if (HoverInputDelay > 0)
                HoverInputDelay--;

            if (PlayerIndices.Count() != players.Count())
                charSelectMenu.LoadControls(PlayerIndices, CharSelectControls.Controls(game, mainMenu));

            selectedMenu.Update(_game);
        }

        private List<PlayerIndex> GetPlayerIndices()
        {
            var players = new List<PlayerIndex>();

            foreach (PlayerIndex playerIndex in new List<PlayerIndex> { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four })
                if (GamePad.GetState(playerIndex).IsConnected)
                    players.Add(playerIndex);

            return players;
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}