using System.Collections.Generic;

namespace Galaga
{
    internal static class Menu
    {
        private static Dictionary<int, MenuChoice> _menu; //какого лешего оно хочет тут readonly O.o
        private static GameState _prevState;
        private static MenuChoice _currentChoice;

        static Menu()
        {
            _prevState = GameState.Exit;
            CurrentKey = 0;
            _currentChoice = MenuChoice.Exit;
            _menu = new Dictionary<int, MenuChoice>();
        }

        public static void MenuGenerator() //      !!!ДОПИСАТЬ!!!
        {
            //юзает состояние игры в целом
            if (_prevState == GameStates.GameState) return;
            _menu.Clear();

            switch (GameStates.GameState)
            {
                case GameState.MainMenu:
                {
                    _menu.Add(0, MenuChoice.StartGame);
                    _menu.Add(1, MenuChoice.Settings);
                    _menu.Add(2, MenuChoice.Exit);
                    break;
                }
                case GameState.Pause:
                {
                    _menu.Add(0, MenuChoice.Resume);
                    _menu.Add(1, MenuChoice.ExitToMenu);
                    _menu.Add(2, MenuChoice.Exit);
                    break;
                }
                case GameState.GameOver:
                {
                    _menu.Add(0, MenuChoice.StartGame);
                    _menu.Add(1, MenuChoice.ExitToMenu);
                    _menu.Add(2, MenuChoice.Exit);
                    break;
                }
            }
            if (_menu.Count > 0)
            {
                _currentChoice = _menu[0];
                CurrentKey = 0;
            }
            _prevState = GameStates.GameState;
        }

        public static void IncChoice()
        {
            CurrentKey++;
            if (CurrentKey >= _menu.Count) CurrentKey = 0;
            _menu.TryGetValue(CurrentKey, out _currentChoice);
        }

        public static void DecChoice()
        {
            CurrentKey--;
            if (CurrentKey < 0) CurrentKey = _menu.Count - 1;
            _menu.TryGetValue(CurrentKey, out _currentChoice);
        }

        public static MenuChoice CurrentChoice => _currentChoice;

        public static Dictionary<int, MenuChoice> CurrentMenu => _menu;
        public static int CurrentKey { get; private set; }
    }
}