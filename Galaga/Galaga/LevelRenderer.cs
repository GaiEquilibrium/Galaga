namespace Galaga
{
    static class LevelRenderer
    {
        public static void Render()
        {
            foreach (Player player in Level.Players)
            {
                player.Render();
            }
            foreach (Enemy enemy in Level.Enemies)
            {
                enemy.Render();
            }
        }
    }
}
