using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Galaga
{
    public static partial class Level
    {
        public static readonly Dictionary<int,Player> Players;
        public static readonly Dictionary<int,Enemy> Enemies;
        public static readonly List<Bullet> Bullets;

        private static int[] _lastInWave;
        private static int _currentWave;

        private static int levelNumber, bonusLevelNumber;
        private static int _playersCount = 1;//временно
        private static int _playerId;
        private static int _enemyId;
        private static XmlDocument level;
        private static bool _xmlIsLoaded;

        static Level()
        {
            Players = new Dictionary<int, Player>();
            Enemies = new Dictionary<int, Enemy>();
            Bullets = new List<Bullet>();
            _playerId = -1;
            _enemyId = -1;
            levelNumber = 0;
            bonusLevelNumber = 0;
            level = new XmlDocument();
            _currentWave = 0;
            _xmlIsLoaded = false;
        }

        public static void Clear()
        {
            Players.Clear();
            Enemies.Clear();
            Bullets.Clear();
            _playerId = -1;
            _enemyId = -1;
            _currentWave = 0;
            _xmlIsLoaded = false;
        }

        private static bool Load()
        {
//            Enemies.Clear();
//            Players.Clear();
//            _playerId = -1;
//            _enemyId = -1;
//            _currentWave = 0;
            Clear();

            for (int i = 0; i < _playersCount; i++) //хреновая система
            {
                Player tmpPlayer = new Player();
                Players.Add(tmpPlayer.PlayerId, tmpPlayer);
                KeyboardInput.SetPlayerId(tmpPlayer.PlayerId);
            }

            levelNumber++;
            if (levelNumber == 5) //или другие номера, когда должен быть бонусный уровень
            {
                bonusLevelNumber++;
                level.Load(@"Levels/bonus_level" + bonusLevelNumber + ".xml");
            }
            else
            {
                level.Load(@"Levels/level" + (levelNumber - bonusLevelNumber) + ".xml");
            }
            XmlLevelReader();

            return true;
        }

        private static bool XmlLevelReader()
        {
            //хммм
            Enemy tmpEnemy;
            String enemyType = "";
            String vector2InString = "";
            Vector2 startPoint = Vector2.Zero, formationOffset = Vector2.Zero;
            XmlElement xmlRoot = level.DocumentElement;
            Dictionary<Vector2, bool> startWaypoints = new Dictionary<Vector2, bool>();
            _lastInWave = new int[int.Parse(xmlRoot.FirstChild.InnerText)];

            foreach (XmlNode xmlWaves in xmlRoot)
            {
                if (xmlWaves.Name != "quantity")
                {
                    foreach (XmlNode xmlWaveChild in xmlWaves.ChildNodes)
                    {
                        if (xmlWaveChild.Name == "waypoints")
                        {
                            startWaypoints.Clear();
                            foreach (XmlNode waypoint in xmlWaveChild.ChildNodes)
                            {
                                bool moveType = false;
                                foreach (XmlNode waypointProperty in waypoint.ChildNodes)
                                {
                                    if (waypointProperty.Name == "movingType" &&
                                        waypointProperty.InnerText == "arround") moveType = true;
                                    if (waypointProperty.Name == "position")
                                    {
                                        vector2InString = waypointProperty.InnerText;
                                        vector2InString = vector2InString.Replace('.', ',');
                                        try
                                        {
                                            startPoint.X =
                                                float.Parse(vector2InString.Substring(0, vector2InString.IndexOf(' ')));
                                            startPoint.Y =
                                                float.Parse(vector2InString.Substring(vector2InString.IndexOf(' ')));
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e);
                                            return false;
                                            //throw;
                                        }
                                    }
                                }
                                startWaypoints.Add(startPoint, moveType);
                            }
                        }
                        if (xmlWaveChild.Name == "enemies")
                        {
                            foreach (XmlNode enemy in xmlWaveChild.ChildNodes)
                            {

                                foreach (XmlNode enemyProperty in enemy.ChildNodes)
                                {
                                    if (enemyProperty.Name == "type") enemyType = enemyProperty.InnerText;
                                    if (enemyProperty.Name == "startPoint")
                                    {
                                        vector2InString = enemyProperty.InnerText;
                                        vector2InString = vector2InString.Replace('.', ',');
                                        try
                                        {
                                            startPoint.X =
                                                float.Parse(vector2InString.Substring(0, vector2InString.IndexOf(' ')));
                                            startPoint.Y =
                                                float.Parse(vector2InString.Substring(vector2InString.IndexOf(' ')));
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e);
                                            return false;
                                            //throw;
                                        }
                                    }
                                    if (enemyProperty.Name == "formationOffset")
                                    {
                                        vector2InString = enemyProperty.InnerText;
                                        vector2InString = vector2InString.Replace('.', ',');
                                        try
                                        {
                                            formationOffset.X =
                                                float.Parse(vector2InString.Substring(0, vector2InString.IndexOf(' ')));
                                            formationOffset.Y =
                                                float.Parse(vector2InString.Substring(vector2InString.IndexOf(' ')));
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e);
                                            return false;
                                            //throw;
                                        }
                                    }
                                }
                                tmpEnemy = new Enemy(EnemyId, startPoint, formationOffset, enemyType, startWaypoints);
                                Enemies.Add(tmpEnemy.EnemyId, tmpEnemy);
                            }
                            _lastInWave[_currentWave] = Enemies.Last().Key;
                            _currentWave++;
                        }
                    }
                }
            }
            _xmlIsLoaded = true;
            _currentWave = 0;
            return true;
        }


        public static void Update()   //переписать вообще целиком
        {
            switch (GameStates.GameState)
            {
                case GameState.LevelLoad:
                {
                    if (!_xmlIsLoaded)
                    {
                        Load();
                    }
                    else if (Enemies.Last().Value.GetInGameState == OnLevelStates.Waiting)
                    {
                        loadUpdate();
                    }
                    else GameStates.LevelStateChanger();
                    break;
                }
                case GameState.Game:
                {
                    GameUpdate();
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

        private static String GetCurrentLevel()
        {
            levelNumber++;
            if (levelNumber == 5) return "BonusLevel" + 1;
            return "Level" + levelNumber;
        }
    }
}
