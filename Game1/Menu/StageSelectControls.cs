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
    public static class StageSelectControls
    {
        public static List<Control> ControlsForGame(Game1 game, SubMenu menu, List<string> levelNames)
        {
            var list = new List<Control>();

            var index = 1;

            foreach (string levelName in levelNames)
            {
                list.Add(new Control(game, menu, levelName, index++, ExecuteAction: StageSelectForGame, ExecuteHover: StageHover));
            }

            list.Add(new Control(game, menu, "Back", index++, font: menu.myMenuState.smallMenuFont, ExecuteAction: Back));

            return list;
        }

        public static List<Control> ControlsForEngine(Game1 game, SubMenu menu, List<string> levelNames)
        {
            var list = new List<Control>();

            var index = 1;

            foreach (string levelName in levelNames)
            {
                list.Add(new Control(game, menu, levelName, index++, ExecuteAction: StageSelectForEngine, ExecuteHover: StageHover, ExecuteCancel: DeleteStageDialog, font: menu.myMenuState.mediumMenuFont));
            }

            list.Add(new Control(game, menu, "Create New Level", index++, ExecuteAction: CreateNewLevel, ExecuteHover: StageHover, font: menu.myMenuState.smallMenuFont));


            list.Add(new Control(game, menu, "Back", index++, font: menu.myMenuState.smallMenuFont, ExecuteAction: BacktoMainMenu));

            return list;
        }

        public static void StageSelectForGame(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
            MediaPlayer.Stop();
            game.ChangeState(new GameState(game, game.GraphicsDevice, game.Content, menuState.charSelectMenu.selectedColors, game.GetLevels().Find(a => a.Name == me.text)));
        }

        public static void DeleteStageDialog(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            if (game.levelManager.Levels.Find(l => l.Name == me.text).Deletable)
            {
                menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                menu.decisionDialogIsActive = true;
                var dialog = (DecisionInputSubMenu)menu.DecisionDialog;
                dialog.SetMessageAndTag("Are you sure you want to delete " + me.text + "?", me.text);
            }
            else { 
                menuState.controlSelectFailSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
            }
        }

        public static void StageSelectForEngine(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
            MediaPlayer.Stop();
            game.ChangeState(new EngineState(game, game.GraphicsDevice, game.Content, game.GetLevels().Find(a => a.Name == me.text).Name));
        }

        public static void CreateNewLevel(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
            menuState.TriggerInputDelay();
            menu.inputDialogIsActive = true;
        }

        public static void StageHover(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {

        }

        public static void Back(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            if (menuState.InputDelay == 0)
            {
                menuState.SetSelectedMenu(menuState.charSelectMenu);
                menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
            }
        }

        public static void BacktoMainMenu(Game1 game, MenuState menuState, SubMenu menu, Control me, PlayerIndex? playerIndex = null)
        {
            if (menuState.InputDelay == 0)
            {
                menuState.SetSelectedMenu(menuState.mainMenu);
                menuState.controlSelectSoundEffect.Play(Constants.soundLevel * 2f, 0f, 0f);
                menuState.TriggerInputDelay();
            }
        }

    }
}
