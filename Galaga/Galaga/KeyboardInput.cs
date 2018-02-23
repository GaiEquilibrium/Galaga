using System.Collections.Generic;
using OpenTK.Input;

namespace Galaga
{
    public enum gameState { MainMenu, StartNewGame, LevelLoad, Game, GameOver, Pause, Exit, Settings };
    public enum menuChoice { StartGame, Settings, Exit, Resume, ExitToMenu };

    static class KeyboardInput
    {
        private static bool isMovingRight = false;
        private static bool isMovingLeft = false;
        private static Dictionary<int, menuChoice> currentMenu;
        private static int currentMenuChoice;
        private static gameState currentGameState;
        private static gameState prevGameState;

        static KeyboardInput()
        {
            currentMenu = new Dictionary<int, menuChoice>();
            currentGameState = gameState.MainMenu;
            currentMenuChoice = 0;
            prevGameState = gameState.Exit;
            MenuChanger();
        }
        public static void KeyDown(object sender, KeyboardKeyEventArgs E, out int action)
        {
            if (currentMenuChoice >= currentMenu.Count) currentMenuChoice = 0;
            if (currentMenuChoice < 0) currentMenuChoice = currentMenu.Count-1;
            action = int.MinValue;
            switch (currentGameState)
            {
                case gameState.MainMenu:
                    switch (E.Key)
                    {
                        case Key.Down:
                            currentMenuChoice++;
                            if (currentMenuChoice >= currentMenu.Count) { currentMenuChoice = 0; }
                            break;
                        case Key.Up:
                            currentMenuChoice--;
                            if (currentMenuChoice < 0) { currentMenuChoice = currentMenu.Count-1; }
                            break;
                        case Key.Enter:
                            switch (currentMenu[currentMenuChoice])
                            {
                                case menuChoice.StartGame:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.StartNewGame;
                                    break;
                                case menuChoice.Settings:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.Settings;
                                    break;
                                case menuChoice.Exit:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.Exit;
                                    break;
                                default: break;
                            }
                            break;
                        default: break;
                    }
                    break;
                case gameState.Game:
                    switch (E.Key)
                    {
                        case Key.Left:
                            if (!isMovingLeft) action = -1;
                            isMovingRight = false;
                            isMovingLeft = true;
                            break;
                        case Key.Right:
                            if (!isMovingRight) action = 1;
                            isMovingLeft = false;
                            isMovingRight = true;
                            break;
                        case Key.Space:
                            action = 0;
                            break;
                        case Key.Escape:
                            currentGameState = gameState.Pause;
                            break;
                        default:
                            break;
                    }
                    break;
                case gameState.Pause:
                    switch (E.Key)
                    {
                        case Key.Down:
                            currentMenuChoice++;
                            if (currentMenuChoice >= currentMenu.Count) { currentMenuChoice = 0; }
                            break;
                        case Key.Up:
                            currentMenuChoice--;
                            if (currentMenuChoice < 0) { currentMenuChoice = currentMenu.Count-1; }
                            break;
                        case Key.Enter:
                            switch (currentMenu[currentMenuChoice])
                            {
                                case menuChoice.Resume:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.Game;
                                    break;
                                case menuChoice.ExitToMenu:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.MainMenu;
                                    break;
                                case menuChoice.Exit:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.Exit;
                                    break;
                                default: break;
                            }
                            break;
                        default: break;
                    }
                    break;
                case gameState.GameOver:
                    switch (E.Key)
                    {
                        case Key.Down:
                            currentMenuChoice++;
                            if (currentMenuChoice >= currentMenu.Count) { currentMenuChoice = 0; }
                            break;
                        case Key.Up:
                            currentMenuChoice--;
                            if (currentMenuChoice < 0) { currentMenuChoice = currentMenu.Count-1; }
                            break;
                        case Key.Enter:
                            switch (currentMenu[currentMenuChoice])
                            {
                                case menuChoice.StartGame:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.StartNewGame;
                                    break;
                                case menuChoice.ExitToMenu:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.MainMenu;
                                    break;
                                case menuChoice.Exit:
                                    currentMenuChoice = 0;
                                    currentGameState = gameState.Exit;
                                    break;
                                default: break;
                            }
                            break;
                        default: break;
                    }
                    break;
                case gameState.Settings:
                    switch (E.Key)
                    {
                        case Key.Escape:
                            currentMenuChoice = 0;
                            currentGameState = gameState.MainMenu;
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }
        public static void KeyUp(object sender, KeyboardKeyEventArgs E)
        {
            switch (currentGameState)
            {
                case gameState.Game:
                    switch (E.Key)
                    {
                        case Key.Left:
                            isMovingLeft = false;
                            break;
                        case Key.Right:
                            isMovingRight = false;
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }

        }
        public static void MenuChanger()
        {
            if (currentGameState == prevGameState) return;
            switch (currentGameState)
            {
                case gameState.MainMenu:
                    currentMenu.Clear();
                    currentMenu.Add(0, menuChoice.StartGame);
                    currentMenu.Add(1, menuChoice.Settings);
                    currentMenu.Add(2, menuChoice.Exit);
                    break;
                case gameState.GameOver:
                    currentMenu.Clear();
                    currentMenu.Add(0, menuChoice.StartGame);
                    currentMenu.Add(1, menuChoice.ExitToMenu);
                    currentMenu.Add(2, menuChoice.Exit);
                    break;
                case gameState.Pause:
                    currentMenu.Clear();
                    currentMenu.Add(0, menuChoice.Resume);
                    currentMenu.Add(1, menuChoice.ExitToMenu);
                    currentMenu.Add(2, menuChoice.Exit);
                    break;
                default: break;
            }
            prevGameState = currentGameState;
        }
        public static void PlayerStop()
        {
            isMovingLeft = false;
            isMovingRight = false;
        }

        public static IDictionary<int, menuChoice> CurrentMenu
        {
            get { return currentMenu; }
        }
        public static bool IsMovingRight
        {
            get { return isMovingRight; }
        }
        public static bool IsMovingLeft
        {
            get { return isMovingLeft; }
        }
        public static menuChoice CurrentMenuChoice
        {
            get { return currentMenu[currentMenuChoice]; }
        }
        public static gameState CurrentGameState
        {
            set { currentGameState = value; }
            get { return currentGameState; }
        }
    }
}
