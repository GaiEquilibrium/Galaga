using System.Collections.Generic;
using OpenTK;

namespace Galaga
{
    public static class Level
    {
        public static readonly Dictionary<int,Player> Players;
        public static readonly List<Enemy> Enemies;
        public static readonly List<Bullet> Bullets;

        private static int _playersCount = 1;//временно
        private static int _playerId;

        static Level()
        {
            Players = new Dictionary<int, Player>();
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();
            _playerId = -1;
        }

        private static void Load()
        {
            Enemies.Clear();
            Players.Clear();
            _playerId = -1;

            for (int i = 0; i < _playersCount; i++) //хреновая система
            {
                Player tmpPlayer = new Player();
                Players.Add(tmpPlayer.PlayerId, tmpPlayer);
                KeyboardInput.SetPlayerId(tmpPlayer.PlayerId);
            }

            for (int i=0;i<10;i++)
                Enemies.Add(new Enemy(0,new Vector2(i-5,3)));
        }

        public static void Update()   //переписать вообще целиком
        {
            switch (GameStates.GameState)
            {
                case GameState.LevelLoad:
                {
                    Load();
                    GameStates.LevelStateChanger();
                    break;
                }
                case GameState.Game:
                {
                    LevelUpdater.Update();
                    break;
                }
                case GameState.Exit:
                {
                    break;
                }
            }
        }

        public static void Render() { LevelRenderer.Render(); }


        public static int PlayerId 
        {
            get
            {
                _playerId++;
                return _playerId;
            }
        }
    }
}
