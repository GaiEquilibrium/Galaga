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
            }
            foreach (Enemy enemy in Level.Enemies)
            {
                enemy.Render();
            }
        }
    }
}
