using System.Collections.Generic;

namespace Galaga
{
    static class LevelRenderer
    {
        public static void Render()
        {
            foreach (KeyValuePair<int, Player> player in Level.Players)
            {
                player.Value.Render();
                player.Value.RenderLifes();
            }
            foreach (Enemy enemy in Level.Enemies)
            {
                enemy.Render();
            }
            foreach (Bullet bullet in Level.Bullets)
            {
                bullet.Render();
            }
        }
    }
}
