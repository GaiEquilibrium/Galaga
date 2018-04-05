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
            foreach (var enemy in Level.Enemies)
            {
                enemy.Value.Render();
            }
            foreach (Bullet bullet in Level.Bullets)
            {
                bullet.Render();
            }
        }
    }
}
