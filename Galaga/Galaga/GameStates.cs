namespace Galaga
{
    //отвечает за всевозможные состояния игры
    //на состояния, по сути могут повлиять только воод пользователя и процесс игры
    //вероятнее всего содержит только глобальные переменные и их частичные обработчики

    //предположительно примерно всё, единственное что вероятно надобно будет добавить всякой мелочи из глобальных переменных

    //вероятно сюда, надо добавить переключение уровней, или что то в подобном духе
    public enum GameState
    {
        MainMenu,
//        StartNewGame,//хм, а не лишнее ли это?
        LevelLoad,
        Game,
        LevelEnd,
        GameOver,
        Pause,
        Exit,
        Settings
    };

    public enum MenuChoice
    {
        StartGame,
        Settings,
        Exit,
        Resume,
        ExitToMenu
    };

    static class GameStates
    {
        public static GameState GameState { get; private set; }
        public static bool IsGame => GameState == GameState.Game || GameState == GameState.LevelLoad || GameState == GameState.LevelEnd;
        public static bool IsMenu => GameState == GameState.MainMenu || GameState == GameState.Pause || GameState == GameState.GameOver;

        static GameStates()
        {
            GameState = GameState.MainMenu;
            Menu.MenuGenerator();
        }

        public static void KeyboardStateChanger()
        {
            //заглушка, пока не напишу меню настроек
            if (GameState == GameState.Settings)
            {
                GameState = GameState.MainMenu;
                Menu.MenuGenerator();
                return;
            }

            if (IsGame)
            {
                GameState = GameState.Pause;
            }
            else if (IsMenu)
            {
                switch (Menu.CurrentChoice) //         !!!ДОПИСАТЬ!!!
                {
                    case MenuChoice.StartGame:
                    {
                        GameState = GameState.LevelLoad;
                        break;
                    }
                    case MenuChoice.Exit:
                    {
                        GameState = GameState.Exit;
                        break;
                    }
                    case MenuChoice.ExitToMenu:
                    {
                        GameState = GameState.MainMenu;
                        break;
                    }
                    case MenuChoice.Resume:
                    {
                        GameState = GameState.Game;
                        break;
                    }
                    case MenuChoice.Settings:
                    {
                        GameState = GameState.Settings;
                        break;
                    }
                    default:
                    {
                        GameState = GameState.Exit;
                        break;
                    }
                }
            }
            Menu.MenuGenerator();
        }

        public static void LevelStateChanger()
        {
            if (GameState == GameState.LevelLoad)
            {
                GameState = GameState.Game;
            }
            else if (GameState == GameState.Game)
            {
                GameState = GameState.GameOver;
            }
            Menu.MenuGenerator();
        }
    }
}