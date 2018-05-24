using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;

namespace Galaga
{
    public enum OnLevelStates
    {
        LevelStart,
        Moving,
        MovingEnd,
        MovingInFormation,
        InMainFormation,
        Waiting, //вероятно только так можно нормально написать что бы противники выходили отдельными волнами
        Destroed //что бы воткнуть наконец нормальную проверку необходимости убирать объект с поля
    }

    public class Enemy : Ship
    {
        #region variables
        protected int cost;
        protected OnLevelStates InGameState;
        protected static float MaxSpeed = 0.3F;
        public Vector2 MainFormationOffset; //formation start in bottom left corner
        public Vector2 SubFormationOffset;
        public int EnemyId { get; }
        private Dictionary<Vector2, bool> waypoints; //is moving aroud, and point
        protected static String SettingsFile = @"Parameters/enemies.txt";
        protected Dictionary<Vector2, bool>.Enumerator Enumerator;
        protected KeyValuePair<Vector2, bool> CurrentWaypoint;
        protected bool WaypointsNotEnd;

        private float radius;
        //написать более толковую систему анимации
        //        private int stage;
        //        public const int maxStage = 4;
        //        public const int frameToStage = 20;

        #endregion

        #region constructors
        public Enemy()
        {
            cost = 0;

            velocity.X = 0;
            velocity.Y = 0;

            InGameState = OnLevelStates.Waiting;
            Belonging = Belonging.Enemy;

            SubFormationOffset.X = -1;
            SubFormationOffset.Y = -1;

            waypoints = new Dictionary<Vector2, bool>();
        }

        public Enemy(int id, Vector2 startPosition, Vector2 formationOffset, String enemyType, Dictionary<Vector2, bool> startWaypoints)
        {
            velocity.X = 0;
            velocity.Y = 0;

            InGameState = OnLevelStates.Waiting;

            GameObject = GameObject.Enemy;
            Belonging = Belonging.Enemy;

            SubFormationOffset.X = -1;
            SubFormationOffset.Y = -1;

            MainFormationOffset = formationOffset;
            EnemyId = id;

            waypoints = new Dictionary<Vector2, bool>(startWaypoints);
            using (StreamReader sr = File.OpenText(SettingsFile))
            {
                String readedString;
                while ((readedString = sr.ReadLine()) != null)
                {
                    if (enemyType == readedString)
                    {
                        String tmpDataString;
                        if ((tmpDataString = sr.ReadLine()) == null) break;
                        State = int.Parse(tmpDataString.Substring(tmpDataString.IndexOf(' ')));

                        if ((tmpDataString = sr.ReadLine()) == null) break;
                        cost = int.Parse(tmpDataString.Substring(tmpDataString.IndexOf(' ')));

                        break;
                    }
                }
            }
            position = startPosition;
            
            //стандартные вэйпоинты надо будет читать после того, как будет в первый раз переходить в формацию, 
            //или уводить с поля и удалять, если нет позиции в формации
            //а после создания, используются вэйпоинты из файла уровня
            velocity = new Vector2(0, MaxSpeed);
            Enumerator = waypoints.GetEnumerator();
            WaypointsNotEnd = Enumerator.MoveNext();
            CurrentWaypoint = Enumerator.Current;
            WaypointsNotEnd = Enumerator.MoveNext();
            radius = 0;
        }

        private bool ReadParameters()
        {
            //сюда надо перетащить процесс чтения параметров
            //хотя надо ли???
            return true;
        }

        #endregion

        #region Updaters
        public new void Update()
        {
            switch (InGameState)
            {
                case OnLevelStates.Waiting:
                {
                    StateChanger();
                    break;
                }
                case OnLevelStates.LevelStart:
                {
                    Moving();
//                    StateChanger();
                    break;
                }
                case OnLevelStates.InMainFormation:
                {

                    if (GameStates.GameState == GameState.LevelLoad) position = Formation.Position + MainFormationOffset;
                    else if (GameStates.GameState == GameState.Game)
                    {
                        position.X = FormationOffset.X * Formation.OffsetCoefficient + Formation.Position.X;
                        position.Y = FormationOffset.Y + Formation.Position.Y;
                    }
                    //StateChanger();//надо придумать проверку на возможность вылета
                    break;
                }
                case OnLevelStates.Moving:
                {
                    Moving();
                    break;
                }
                case OnLevelStates.MovingEnd:
                {
                    MoveToMainFormation();
                    break;
                }
                case OnLevelStates.MovingInFormation:
                {
                    break;
                }
            }
        }

        private void StateChanger()
        {
            switch (InGameState)
            {
                case OnLevelStates.Waiting:
                {
                    InGameState = OnLevelStates.LevelStart;
                    break;
                }
                case OnLevelStates.LevelStart:
                {
                    InGameState = OnLevelStates.MovingEnd; //временно
                    break;
                }
                case OnLevelStates.InMainFormation:
                {
                    velocity = new Vector2(0, MaxSpeed);
                    Enumerator = waypoints.GetEnumerator();
                    WaypointsNotEnd = Enumerator.MoveNext();
                    CurrentWaypoint = Enumerator.Current;
                    WaypointsNotEnd = Enumerator.MoveNext();
                    InGameState = OnLevelStates.Moving;
                    break;
                }
                case OnLevelStates.Moving:
                {
                    Enumerator.Dispose();
                    InGameState = OnLevelStates.MovingEnd;
                    break;
                }
                case OnLevelStates.MovingEnd:
                {
                    InGameState = OnLevelStates.InMainFormation;    //временно, так как должена быть более разумная смена состояния
                    break;
                }
                case OnLevelStates.MovingInFormation:
                {
                    break;
                }
            }
        }

        public void ChangeFormation(Vector2 newOffset)
        {
            SubFormationOffset = newOffset;
        }
        #endregion

        #region MovingFunctions
        //velocity must be equal to 0.3    
        protected new void Moving()
        {
            if (WaypointsNotEnd)
            {
                if (CurrentWaypoint.Value)
                    MoveAround(CurrentWaypoint.Key);
                else
                    MoveTo(CurrentWaypoint.Key);
                return;
            }
            if (CurrentWaypoint.Value)
                MoveAround(CurrentWaypoint.Key);
            else
                MoveTo(CurrentWaypoint.Key);
        }

        private void MoveTo(Vector2 point)
        {
            Vector2 unitVelocity = (point - position) /
                                   (float)Math.Sqrt((point.X - position.X) * (point.X - position.X) +
                                                     (point.Y - position.Y) * (point.Y - position.Y)) * MaxSpeed;
            velocity = unitVelocity;
            base.Moving();

            if (Math.Abs(Position.X - CurrentWaypoint.Key.X) < 0.2 &&
                Math.Abs(Position.Y - CurrentWaypoint.Key.Y) < 0.2)
            {
                if (WaypointsNotEnd)
                {
                    CurrentWaypoint = Enumerator.Current;
                    WaypointsNotEnd = Enumerator.MoveNext();
                }
                else
                {
                    StateChanger();
                }
            }
        }

        private void MoveAround(Vector2 point)
        {
            if (radius == 0)
            {
                radius = (float) Math.Sqrt((point.X - position.X) * (point.X - position.X) +
                                                 (point.Y - position.Y) * (point.Y - position.Y));
            }
            float angularVelocity = (float) (Math.PI / (radius * Math.PI / MaxSpeed));
            Vector2 unitVector = (point - position) / radius;
            Vector2 acceleration = MaxSpeed * angularVelocity * unitVector;
            velocity += acceleration;

            base.Moving();

            float direction;
            if (Velocity.X != 0)
            {
                direction = (float)Math.Atan2(Velocity.Y, Velocity.X);
            }
            else
            {
                if (Velocity.Y > 0)
                {
                    direction = (float)Math.PI / 2;
                }
                else
                {
                    direction = (float)-Math.PI / 2;
                }
            }

            if (WaypointsNotEnd &&
                Math.Abs(direction - DirectionCalc(position, Enumerator.Current.Key)) < angularVelocity * 0.5 + 0.05 &&
                !Enumerator.Current.Value)
            {
                radius = 0;
                CurrentWaypoint = Enumerator.Current;
                WaypointsNotEnd = Enumerator.MoveNext();
            }
            else if (WaypointsNotEnd && Math.Abs((float) Math.Sqrt(
                         (Enumerator.Current.Key.X - position.X) * (Enumerator.Current.Key.X - position.X) +
                         (Enumerator.Current.Key.Y - position.Y) * (Enumerator.Current.Key.Y - position.Y))) <=
                     radius && Enumerator.Current.Value)
            {
                radius = 0;
                CurrentWaypoint = Enumerator.Current;
                WaypointsNotEnd = Enumerator.MoveNext();
            }
            else if (!WaypointsNotEnd &&
                     Math.Abs(direction - DirectionCalc(position, Formation.Position + MainFormationOffset)) <
                     angularVelocity * 0.5 + 0.05)
            {
                radius = 0;
                StateChanger();
            }
        }

        private void MoveToMainFormation()
        {
            //добавить проверку на то, есть ли вобще место в формации
            Vector2 moveToPoint = Formation.Position + MainFormationOffset;   //переписать так же как уже написал для update
            Vector2 unitVelocity = (moveToPoint - position) /
                                   (float)Math.Sqrt((moveToPoint.X - position.X) * (moveToPoint.X - position.X) +
                                                     (moveToPoint.Y - position.Y) * (moveToPoint.Y - position.Y)) * MaxSpeed;
            velocity = unitVelocity;
            base.Moving();

            if (Math.Abs(Position.X - moveToPoint.X) < 0.2 &&
                Math.Abs(Position.Y - moveToPoint.Y) < 0.2)
            {
                StateChanger();
            }
        }

        private bool ReadstandartWaypoints()
        {
            waypoints.Clear();

            return true;
        }
        #endregion

        public OnLevelStates GetInGameState => InGameState;

        public Bullet Shoot()
        {
//            SoundMaster.Shoot();//надо будет так же несколько переделать работу со звуком
            Vector2 tmpPos = position;
            tmpPos.Y--;
            return new Bullet(tmpPos, Belonging.Enemy, -1);
        }

        private Vector2 FormationOffset => InSubFormation ? SubFormationOffset : MainFormationOffset;

        public bool InSubFormation => SubFormationOffset.X >= 0;

        public int Cost => cost;
    }
}