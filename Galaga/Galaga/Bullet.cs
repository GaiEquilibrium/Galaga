using OpenTK;

namespace Galaga
{
    //отвечает за выстрел, когда он виден и активен
    //по сути, только методы на проверки столкновений (и возможно собственно методы уничтожения)
    public class Bullet : Moved
    {
        private bool _isComplete;
        public bool IsComplete {
            get => _isComplete;
            set
            {               
                if (_isComplete == false && PlayerId>=0)
                {
                    Level.Players[PlayerId].Shoots++;
                }
                _isComplete = value;
            }
        }
        public int PlayerId { get; }

        public Bullet(Vector2 startPosition, Belonging newOwner, int newPlayerId)
        {
            position = startPosition;
            Belonging = newOwner;
            _isComplete = false;
            if (Belonging == Belonging.Player)
            {
                PlayerId = newPlayerId;
                velocity.Y = 0.8F;
            }
            else if (Belonging == Belonging.Enemy)
            {
                PlayerId = -1;
                velocity.Y = -0.8F;
            }
            else
            {
                PlayerId = -1;
                velocity.Y = 0;
                position.Y = 20;
                _isComplete = true;
            }
            GameObject = GameObject.Bullet;
            State = 0;
        }

        public new void Update()
        {
            Moving();
        }

        public new void Moving()
        {
            base.Moving();
            if (position.Y > 10 || position.Y < -10)
            {
                IsComplete = true;
            }
        }
    }
}
