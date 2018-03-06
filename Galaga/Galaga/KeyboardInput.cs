using OpenTK.Input;

namespace Galaga
{
    static class KeyboardInput
    {
        private static bool _isMovingRight;              
        private static bool _isMovingLeft;

        static KeyboardInput ()
        {
            _isMovingLeft = false;
            _isMovingRight = false;
        }
        public static void KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (GameStates.IsGame)
            {
                switch (e.Key)
                {
                    case Key.Left:
                    {
                        _isMovingLeft = true;
                        Level.PlayerAction(PlayerAction.MoveLeft);
                        break;
                    }
                    case Key.Right:
                    {
                        _isMovingRight = true;
                        Level.PlayerAction(PlayerAction.MoveRight);
                        break;
                    }
                    case Key.Space:
                    {
                        Level.PlayerAction(PlayerAction.Shoot);
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
        }
        public static void KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (GameStates.IsGame)
            {
                switch (e.Key)
                {
                    case Key.Left:
                    {
                        _isMovingLeft = false;
                        if (!_isMovingRight) Level.PlayerAction(PlayerAction.Stop);
                        break;
                    }
                    case Key.Right:
                    {
                        _isMovingRight = false;
                        if (!_isMovingLeft) Level.PlayerAction(PlayerAction.Stop);
                        break;
                    }
                }
            }
        }
    }
}