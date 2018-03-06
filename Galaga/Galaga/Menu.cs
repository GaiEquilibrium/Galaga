using System.Collections.Generic;

namespace Galaga
{
    internal static class Menu
    {
        private static Dictionary<int, menuChoice> _menu; //какого лешего оно хочет тут readonly O.o
        private static gameState _prevState;
        private static menuChoice _currentChoice;

        static Menu()
        {
            _prevState = gameState.Exit;
            CurrentKey = 0;
            _currentChoice = menuChoice.Exit;
            _menu = new Dictionary<int, menuChoice>();
        }

        public static void MenuGenerator() //      !!!ДОПИСАТЬ!!!
        {
            //юзает состояние игры в целом
            if (_prevState == GameStates.GameState) return;
            _menu.Clear();

            switch (GameStates.GameState)
            {
                case gameState.MainMenu:
                {
                    _menu.Add(0, menuChoice.StartGame);
                    _menu.Add(1, menuChoice.Settings);
                    _menu.Add(2, menuChoice.Exit);
                    break;
                }
                case gameState.Pause:
                {
                    _menu.Add(0, menuChoice.Resume);
                    _menu.Add(1, menuChoice.ExitToMenu);
                    _menu.Add(2, menuChoice.Exit);
                    break;
                }
                case gameState.GameOver:
                {
                    _menu.Add(0, menuChoice.StartGame);
                    _menu.Add(1, menuChoice.ExitToMenu);
                    _menu.Add(2, menuChoice.Exit);
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

        public static menuChoice CurrentChoice => _currentChoice;

        public static Dictionary<int, menuChoice> CurrentMenu => _menu;
        public static int CurrentKey { get; private set; }
    }
}