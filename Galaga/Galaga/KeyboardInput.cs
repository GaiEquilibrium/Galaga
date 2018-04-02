using OpenTK.Input;

namespace Galaga
{
    static class KeyboardInput
    {
        private static bool _isMovingRight;              
        private static bool _isMovingLeft;
        private static int _playerId;

        static KeyboardInput ()
        {
            _isMovingLeft = false;
            _isMovingRight = false;
            _playerId = -1;
        }

        public static void SetPlayerId(int playerId)
        {
            _playerId = playerId;
        }

        public static void KeyDown(object sender, KeyboardKeyEventArgs e)
        {

            if (GameStates.IsGame && _playerId >= 0)
            {
                switch (e.Key)
                {
                    case Key.Left:
                    {
                        _isMovingLeft = true;
                        Level.Players[_playerId].Action(PlayerAction.MoveLeft);
                        break;
                    }
                    case Key.Right:
                    {
                        _isMovingRight = true;
                        Level.Players[_playerId].Action(PlayerAction.MoveRight);
                        break;
                    }
                    case Key.Space:
                    {
                        Level.Players[_playerId].Action(PlayerAction.Shoot);
                        break;
                    }
                    case Key.Escape:
                    {
                        GameStates.KeyboardStateChanger();
                        break;
                    }
                }
            }
            else if (GameStates.IsMenu)
            {
                switch (e.Key)
                {
                    case Key.Down:
                    {
                        Menu.IncChoice();   //нулевой пункт меню вверху
                        break;
                    }
                    case Key.Up:
                    {
                        Menu.DecChoice();
                        break;
                    }
                    case Key.Enter:
                    {
                        GameStates.KeyboardStateChanger();
                        break;
                    }
                }
            }
            else
            {
                if (e.Key == Key.Escape || e.Key == Key.Enter)
                    GameStates.KeyboardStateChanger();
            }
        }
        public static void KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (GameStates.IsGame && _playerId >= 0)
            {
                switch (e.Key)
                {
                    case Key.Left:
                    {
                        _isMovingLeft = false;
                        if (!_isMovingRight) Level.Players[_playerId].Action(PlayerAction.Stop);
                        break;
                    }
                    case Key.Right:
                    {
                        _isMovingRight = false;
                        if (!_isMovingLeft) Level.Players[_playerId].Action(PlayerAction.Stop);
                        break;
                    }
                }
            }
        }
    }
}