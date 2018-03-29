using OpenTK;

namespace Galaga
{
    //отвечает за выстрел, когда он виден и активен
    //по сути, только методы на проверки столкновений (и возможно собственно методы уничтожения)
    public class Bullet : Moved
    {
        public bool IsComplete { get; private set; }

        public Bullet(Vector2 startPosition, Belonging newOwner)
        {
            position = startPosition;
            Belonging = newOwner;
            if (Belonging == Belonging.Player) velocity.Y = 0.8F;
            else if (Belonging == Belonging.Enemy) velocity.Y = -0.8F;
            else
            {
                velocity.Y = 0;
                position.Y = 20;
            }
        }

        public new void Moving()
        {
            base.Moving();
            if (position.Y > 10 || position.Y < -10) IsComplete = true;
        }
    }
}
