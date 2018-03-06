namespace Galaga
{
    //отвечает за всевозможные состояния игры
    //на состояния, по сути могут повлиять только воод пользователя и процесс игры
    //вероятнее всего содержит только глобальные переменные и их частичные обработчики

    //предположительно примерно всё, единственное что вероятно надобно будет добавить всякой мелочи из глобальных переменных

    public enum gameState
    {
        MainMenu,
        StartNewGame,
        LevelLoad,
        Game,
        GameOver,
        Pause,
        Exit,
        Settings
    };

    public enum menuChoice
    {
        StartGame,
        Settings,
        Exit,
        Resume,
        ExitToMenu
    };

    static class GameStates
    {
        public static gameState GameState { get; private set; }
        public static bool IsGame => GameState == gameState.Game;
        public static bool IsMenu => GameState == gameState.MainMenu || GameState == gameState.Pause || GameState == gameState.GameOver;

        static GameStates()
        {
            GameState = gameState.MainMenu;
            Menu.MenuGenerator();
        }

        public static void KeyboardStateChanger()
        {
            //заглушка, пока не напишу меню настроек
            if (GameState == gameState.Settings)
            {
                GameState = gameState.MainMenu;
                Menu.MenuGenerator();
                return;
            }

            if (IsGame)
            {
                GameState = gameState.Pause;
            }
            else if (IsMenu)
            {
                switch (Menu.CurrentChoice) //         !!!ДОПИСАТЬ!!!
                {
                    case menuChoice.StartGame:
                    {
                        GameState = gameState.LevelLoad;
                        break;
                    }
                    case menuChoice.Exit:
                    {
                        GameState = gameState.Exit;
                        break;
                    }
                    case menuChoice.ExitToMenu:
                    {
                        GameState = gameState.MainMenu;
                        break;
                    }
                    case menuChoice.Resume:
                    {
                        GameState = gameState.Game;
                        break;
                    }
                    case menuChoice.Settings:
                    {
                        GameState = gameState.Settings;
                        break;
                    }
                    default:
                    {
                        GameState = gameState.Exit;
                        break;
                    }
                }
            }
            Menu.MenuGenerator();
        }

        public static void LevelStateChanger()
        {
            if (GameState == gameState.LevelLoad)
            {
                GameState = gameState.Game;
            }
            else if (GameState == gameState.Game)
            {
                GameState = gameState.GameOver;
            }
            Menu.MenuGenerator();
        }
    }
}