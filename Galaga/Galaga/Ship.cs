namespace Galaga
{
    public class Ship : Moved
    {
        public bool IsCollide(Enemy enemy)
        {
            if (((position.X + 0.95F) > enemy.Position.X + 0.05) && ((position.X + 0.05F) < (enemy.Position.X + 0.95)) &&
                ((position.Y + 0.95F) > enemy.Position.Y + 0.05) && ((position.Y + 0.05F) < (enemy.Position.Y + 0.95)))
                return true;
            return false;
        }

        public bool IsHit(Bullet bullet)
        {
            if (bullet.Belonging != Belonging)
            {
                if (((bullet.Position.X + 0.55F) > Position.X) && ((bullet.Position.X + 0.45F) < (Position.X + 1)) &&
                    ((bullet.Position.Y + 0.7F) > Position.Y) && ((bullet.Position.Y + 0.3F) < (Position.Y + 1)))
                return true;
            }

            return false;
        }
    }
}
