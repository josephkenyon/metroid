using Game1.States;
using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Menu
{
    public static class SettingsControls
    {
        public static int index = 0;
        public static List<Control> Controls(Game1 game, SubMenu menu) => new List<Control> {
            { new Control(game, menu, "Audio", index = 1, ExecuteAction: AudioSelect)},
            { new Control(game, menu, "Video", ++index, ExecuteAction: VideoSelect)},
            { new Control(game, menu, "Back", ++index, ExecuteAction: BackSelect, font: menu.myMenuState.smallMenuFont)},
        };
        public static List<Control> VideoControls(Game1 game, SubMenu menu) => new List<Control> {
            { new Control(game, menu, "Fullscreen: Off", index = 1, ExecuteAction: SetFullscreenOff)},
            { new Control(game, menu, "Fullscreen: On", ++index, ExecuteAction: SetFullscreenOn)},
            //{ new Control(game, menu, "Resolution: 1920x1080", ++index, ExecuteAction: SetResolution1080)},
            //{ new Control(game, menu, "Resolution: 2560x1440", ++index, ExecuteAction: SetResolution1440)},
            { new Control(game, menu, "Back", ++index, ExecuteAction: BackToSettings, font: menu.myMenuState.smallMenuFont)},
        };
        public static List<Control> AudioControls(Game1 game, SubMenu menu) => new List<Control> {
            { new Control(game, menu, "Sound: Low", index = 1, ExecuteAction: SetSoundLow)},
            { new Control(game, menu, "Sound: Medium", ++index, ExecuteAction: SetSoundMedium)},
            { new Control(game, menu, "Sound: High", ++index, ExecuteAction: SetSoundHigh)},
            { new Control(game, menu, "Back", ++index, ExecuteAction: BackToSettings, font: menu.myMenuState.smallMenuFont)},
        };

        public static void AudioSelect(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            menuState.SetSelectedMenu(menuState.audioMenu);
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void SetResolution1080(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            game.SetResolution(new Point(1920, 1080));
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void SetSoundLow(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            game.soundLevel = 0.1f;
            MediaPlayer.Volume = game.soundLevel;
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void SetSoundMedium(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            game.soundLevel = 0.2f;
            MediaPlayer.Volume = game.soundLevel;
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void SetSoundHigh(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            game.soundLevel = 0.3f;
            MediaPlayer.Volume = game.soundLevel;
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void SetResolution1440(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            game.SetResolution(new Point(2560, 1440));
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void SetFullscreenOff(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            game.SetIsFullscreen(false);
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void SetFullscreenOn(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            game.SetIsFullscreen(true);
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void VideoSelect(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            menuState.SetSelectedMenu(menuState.videoMenu);
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void BackSelect(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            menuState.SetSelectedMenu(menuState.mainMenu);
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }

        public static void BackToSettings(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            menuState.SetSelectedMenu(menuState.settingsMenu);
            menuState.controlSelectSoundEffect.Play(game.soundLevel * 2f, 0f, 0f);
        }
    }
}
