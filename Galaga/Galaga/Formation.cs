using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Galaga
{
    public static class Formation
    {
        //надо переписать под статический класс
        //и в итоге перенести ноль формации в центр внизу
        //formation start in bottom center
        public static float OffsetCoefficient { get; private set; }

        private static float _offsetChanger;
        private static Vector2 _maxOffset;   //x = left; y = right
        private static float _size;
        private static readonly Vector2 StartVelocity = new Vector2(-0.1F,0);
        private static Vector2 _position;
        private static Vector2 _velocity;

        static Formation()
        {
            _position = new Vector2(0, 5);
            _velocity = StartVelocity;
            _maxOffset = Vector2.Zero;
            _size = 0;
            OffsetCoefficient = 1;
            _offsetChanger = 0.05F;
        }

        private static void Moving()
        {
            if (GameStates.GameState == GameState.LevelLoad)
            {
                if (_position.X + _maxOffset.X < -9.5 && _velocity.X < 0) _velocity.X *= -1;
                if (_position.X + _maxOffset.Y > 10.5 && _velocity.X > 0) _velocity.X *= -1;

                _position += _velocity;
            }
            else if (GameStates.GameState == GameState.Game)
            {
                if (OffsetCoefficient <= 1) _offsetChanger *= -1;
                if (OffsetCoefficient * _maxOffset.X < -9.5 || OffsetCoefficient * _maxOffset.Y > 10.5)
                    _offsetChanger *= 1;

                OffsetCoefficient += _offsetChanger;
            }
        }

        private static void SizeCalculation()
        {
            _maxOffset.X = float.MaxValue;
            _maxOffset.Y = float.MinValue;

            foreach (var enemy in Level.Enemies.Values)
            {
                if (enemy.GetInGameState != OnLevelStates.InMainFormation ||
                    enemy.GetInGameState != OnLevelStates.Moving || 
                    enemy.GetInGameState != OnLevelStates.MovingEnd ||
                    enemy.GetInGameState != OnLevelStates.MovingInFormation)
                    continue;
                if (enemy.Position.X > _maxOffset.Y) _maxOffset.Y = enemy.Position.X;
                if (enemy.Position.X < _maxOffset.X) _maxOffset.Y = enemy.Position.X;
            }
            _size = _maxOffset.Y - _maxOffset.X;
        }

        public static void Update()
        {
            SizeCalculation();
            Moving();
        }

        public static Vector2 Position => _position;
    }
}