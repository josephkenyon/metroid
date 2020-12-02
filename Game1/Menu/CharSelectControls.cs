using Game1.States;
using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.Enums;

namespace Game1.Menu
{
    public static class CharSelectControls
    {
        public static int index = 0;
        public static List<Control> Controls(Game1 game, SubMenu menu, PlayerIndex? playerIndex = null)
           => new List<Control> {
                { new Control(game, menu, "Blue", index = 2, playerIndex: playerIndex, ExecuteAction: CharSelect, ExecuteHover: CharHover)},
                { new Control(game, menu, "Green", ++index, playerIndex: playerIndex, ExecuteAction: CharSelect, ExecuteHover: CharHover)},
                { new Control(game, menu, "Orange", ++index, playerIndex: playerIndex, ExecuteAction: CharSelect, ExecuteHover: CharHover)},
                { new Control(game, menu, "Purple", ++index, playerIndex: playerIndex, ExecuteAction: CharSelect, ExecuteHover: CharHover)},
                { new Control(game, menu, "Red", ++index, playerIndex: playerIndex, ExecuteAction: CharSelect, ExecuteHover: CharHover)},
                { new Control(game, menu, "Yellow", ++index, playerIndex: playerIndex, ExecuteAction: CharSelect, ExecuteHover: CharHover)},
                { new Control(game, menu, "White", ++index, playerIndex: playerIndex, ExecuteAction: CharSelect, ExecuteHover: CharHover)},
                { new Control(game, menu, "Black", ++index, playerIndex: playerIndex, ExecuteAction: CharSelect, ExecuteHover: CharHover)},
            };

        public static Control NextControl(Game1 game, SubMenu menu, PlayerIndex? playerIndex = null)
           => new Control(game, menu, "Next", index = 12, ExecuteAction: Next);

        public static Control BackControl(Game1 game, SubMenu menu, PlayerIndex? playerIndex = null)
           => new Control(game, menu, "Back", index = 13, ExecuteAction: Back);

        public static void CharSelect(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            var charSelectMenu = (CharSelectSubMenu)menu;
            var colorString = SamusColorStringConversions[me.text];

            if (!charSelectMenu.selectedColors.ContainsValue(colorString))
            {
                charSelectMenu.selectedColors[(PlayerIndex)playerIndex] = colorString;
                menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
            }
            else
                menuState.controlSelectFailSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
        }

        public static void Next(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            if (menuState.InputDelay == 0)
            {
                menuState.SetSelectedMenu(menuState.stageSelectMenuForGame);
                menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                menuState.TriggerInputDelay();
            }
        }

        public static void Back(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            if (menuState.InputDelay == 0)
            {
                menuState.SetSelectedMenu(menuState.mainMenu);
                menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                menuState.TriggerInputDelay();
            }
        }

        public static void CharHover(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            var charSelectMenu = (CharSelectSubMenu)menu;
            charSelectMenu.selectedColors[(PlayerIndex)playerIndex] = SamusColorStringConversions[me.text];
        }

    }
}
