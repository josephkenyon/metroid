using Game1.States;
using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Menu
{
    public static class MainMenuControls
    {
        public static int index = 0;
        public static List<Control> Controls(Game1 game, SubMenu menu) => new List<Control> {
            { new Control(game, menu, "Multiplayer", index = 1, ExecuteAction: CharSelect)},
            { new Control(game, menu, "Level Editor", ++index, ExecuteAction: RunEngine)},
            { new Control(game, menu, "Exit Game", ++index, ExecuteAction: ExitGame)}
        };

        public static void ExitGame(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            game.Exit();

        }
        public static void CharSelect(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            if (menuState.InputDelay == 0)
            {
                menuState.SetSelectedMenu(menuState.charSelectMenu);
                menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                menuState.TriggerInputDelay();
            }
        }

        public static void RunEngine(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
            menuState.SetSelectedMenu(menuState.stageSelectMenuForEngine);
        }
    }
}
