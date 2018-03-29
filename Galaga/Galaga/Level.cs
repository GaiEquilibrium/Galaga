using System.Collections.Generic;
using OpenTK;

namespace Galaga
{
    //хранит все объекты игры и обрабатывает их взаимодействие
    public static class Level
    {
        private static Dictionary<int,Player> _players;
        private static List<Enemy> _enemies;
        private static int _playersCount = 1;//временно
        private static int _playerId;

        static Level()
        {
            _players = new Dictionary<int, Player>();
            _enemies = new List<Enemy>();
            _playerId = -1;
        }

        private static void Load()
        {
            _enemies.Clear();
            _players.Clear();

            for (int i = 0; i < _playersCount; i++) //хреновая система
            {
                Player tmpPlayer = new Player();
                _players.Add(tmpPlayer.PlayerId, tmpPlayer);
                KeyboardInput.SetPlayerId(tmpPlayer.PlayerId);
            }

            for (int i=0;i<10;i++)
                _enemies.Add(new Enemy(0,new Vector2(i-5,3)));
        }

        public static void Update()   //переписать вообще целиком
        {
//            KeyboardInput.MenuChanger();
            if (GameStates.GameState == GameState.LevelLoad)
            {
                Load();
                //KeyboardInput.CurrentGameState = gameState.Game;
                GameStates.LevelStateChanger();
                return;
            }
            if (GameStates.GameState == GameState.Game)
            {
                LevelUpdater.Update();


            }
            if (GameStates.GameState == GameState.Exit)
            {
//                WindowProperty.ShootFlagNeg();
            }
        }
        //лучше вообще перенесу в отдельный класс, и вероятно написать как и рендерер

        public static void Render() { LevelRenderer.Render(); }

        public static List<Enemy> Enemies => _enemies;

        public static Dictionary<int,Player> Players => _players;

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
