using System.Collections.Generic;
using OpenTK;

namespace Galaga
{
    public static class Level
    {
        public static readonly Dictionary<int,Player> Players;
        public static readonly Dictionary<int,Enemy> Enemies;
        public static readonly List<Bullet> Bullets;
        public static Formation MainFormation;

        private static int _playersCount = 1;//временно
        private static int _playerId;
        private static int _enemyId;

        static Level()
        {
            Players = new Dictionary<int, Player>();
            Enemies = new Dictionary<int, Enemy>();
            Bullets = new List<Bullet>();
            _playerId = -1;
            _enemyId = -1;
        }

        private static void Load()
        {
            Enemies.Clear();
            Players.Clear();
            _playerId = -1;
            _enemyId = -1;

            for (int i = 0; i < _playersCount; i++) //хреновая система
            {
                Player tmpPlayer = new Player();
                Players.Add(tmpPlayer.PlayerId, tmpPlayer);
                KeyboardInput.SetPlayerId(tmpPlayer.PlayerId);
            }

            for (int i = 0; i < 10; i++)
            {
                Enemy tmpEnemy = new Enemy(EnemyId,0, new Vector2(i - 5, 3), new Vector2(i, 0));
                Enemies.Add(tmpEnemy.EnemyId,tmpEnemy);
            }

            MainFormation = new Formation(Enemies);
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
        public static int EnemyId
        {
            get
            {
                _enemyId++;
                return _enemyId;
            }
        }
    }
}
