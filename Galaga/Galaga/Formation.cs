using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Galaga
{
    public class Formation : Moved
    {
        //formation start in bottom left corner
        public Dictionary<int,Vector2> EnemiesPositions;
        private Vector2 _minimum;
        private Vector2 _maximum;
        private Vector2 _size;
        private bool IsSubFormation { get; }
        private Vector2 startVelocity = new Vector2(-0.1F,0);

        public Formation()
        {
            EnemiesPositions = new Dictionary<int, Vector2>();
            IsSubFormation = false;
            position = new Vector2(0, 0);
            velocity = startVelocity;
            _size = new Vector2(0,0);
            State = 0;
            Counter = 0;
        }
        public Formation(bool isSubFormation)
        {
            EnemiesPositions = new Dictionary<int, Vector2>();
            IsSubFormation = isSubFormation;
            position = new Vector2(0,0);
            velocity = startVelocity;
            _size = new Vector2(0, 0);
            State = 0;
            Counter = 0;
        }
        public Formation(Dictionary<int,Enemy> enemies)
        {
            EnemiesPositions = new Dictionary<int, Vector2>();
            foreach (var enemy in enemies)
            {
                EnemiesPositions.Add(enemy.Key,enemy.Value.MainFormationOffset);
            }
            IsSubFormation = false;
            SizePositionCalculation();
            velocity = startVelocity;
            State = 0;
            Counter = 0;
        }
        public Formation(Dictionary<int, Enemy> enemies, bool isSubFormation)
        {
            EnemiesPositions = new Dictionary<int, Vector2>();
            IsSubFormation = isSubFormation;
            if (isSubFormation)
            {
                foreach (var enemy in enemies)
                {
                    EnemiesPositions.Add(enemy.Key, enemy.Value.SubFormationOffset);
                }
                return;
            }
            foreach (var enemy in enemies)
            {
                EnemiesPositions.Add(enemy.Key, enemy.Value.MainFormationOffset);
            }
            SizePositionCalculation();
            velocity = startVelocity;
            State = 0;
            Counter = 0;
        }

        public void Add(Enemy enemy)
        {
            EnemiesPositions.Add(enemy.EnemyId, IsSubFormation ? enemy.SubFormationOffset : enemy.MainFormationOffset);
            SizePositionCalculation();
        }
        public void Remove(Enemy enemy)
        {
            EnemiesPositions.Remove(enemy.EnemyId);
            SizePositionCalculation();
        }

        public new void Moving()
        {
            if (position.X < -9.5 && Velocity.X<0) velocity.X *= -1;
            if (position.X + _size.X -0.5 > 10 && Velocity.X > 0) velocity.X *= -1;
            base.Moving();
        }

        public new void Update()
        {
            Moving();
        }

        private void SizePositionCalculation()
        {
            _minimum.X = float.MaxValue;
            _minimum.Y = float.MaxValue;
            _maximum.X = float.MinValue;
            _maximum.Y = float.MinValue;

            foreach (var enemy in EnemiesPositions)
            {
                if (enemy.Value.X > _maximum.X) _maximum.X = enemy.Value.X;
                if (enemy.Value.Y > _maximum.Y) _maximum.Y = enemy.Value.Y;
                if (enemy.Value.X < _minimum.X) _minimum.X = enemy.Value.X;
                if (enemy.Value.Y < _minimum.Y) _minimum.Y = enemy.Value.Y;
            }
            _size = _maximum - _minimum + new Vector2(1, 1);

            if (IsSubFormation)
            {
                if (_minimum.X != 0 || _minimum.Y != 0)
                {
                    foreach (var key in EnemiesPositions.Keys.ToList())
                    {
                        Level.Enemies[key].SubFormationOffset -= _minimum;
                        EnemiesPositions[key] -= _minimum;
                    }
                }
            }
            else
            {
                if (_minimum.X != 0 || _minimum.Y != 0)
                {
                    foreach (var key in EnemiesPositions.Keys.ToList())
                    {
                        Level.Enemies[key].MainFormationOffset -= _minimum;
                        EnemiesPositions[key] -= _minimum;
                    }
                }
            }
            foreach (var enemy in EnemiesPositions)
            {
                if (enemy.Value.X == 0)
                {
                    position = Level.Enemies[enemy.Key].Position;
                    position.Y -= enemy.Value.Y;
                    break;
                }
            }
        }
    }
}