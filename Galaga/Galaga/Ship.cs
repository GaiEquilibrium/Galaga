
using OpenTK;

namespace Galaga
{
    public class Ship : Moved
    {
        public bool IsCollide(Vector2 enemyPos)
        {
            if (((position.X + 0.95F) > enemyPos.X + 0.05) && ((position.X + 0.05F) < (enemyPos.X + 0.95)) &&
                ((position.Y + 0.95F) > enemyPos.Y + 0.05) && ((position.Y + 0.05F) < (enemyPos.Y + 0.95)))
                return true;
            return false;
        }

        public bool IsHit(Vector2 bulletPos, Vector2 shipPos, bool isPlayer, bool bulletOwner)
        {
            if (isPlayer == bulletOwner) return false;
            if (((bulletPos.X + 0.55F) > shipPos.X) && ((bulletPos.X + 0.45F) < (shipPos.X + 1)) &&
                ((bulletPos.Y + 0.7F) > shipPos.Y) && ((bulletPos.Y + 0.3F) < (shipPos.Y + 1)))
                return true;

            return false;
        }
    }
}
