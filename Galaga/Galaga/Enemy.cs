using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;

namespace Galaga
{
    public enum OnLevelStates
    {
        LevelStart,
        MovingStart,
        Moving,
        MovingEnd,
        MovingInFormation,
        InMainFormation,
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
        protected static String SettingsFile = @"Settings/enemies.txt";
        protected Dictionary<Vector2, bool>.Enumerator Enumerator;
        protected KeyValuePair<Vector2, bool> currentWaypoint;
        protected bool waypointsNotEnd;

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

            InGameState = OnLevelStates.LevelStart;
            Belonging = Belonging.Enemy;

            SubFormationOffset.X = -1;
            SubFormationOffset.Y = -1;

            waypoints = new Dictionary<Vector2, bool>();
        }

        public Enemy(int id, Vector2 startPosition, Vector2 formationOffset, String enemyType)
        {
            velocity.X = 0;
            velocity.Y = 0;

            InGameState = OnLevelStates.LevelStart;

            GameObject = GameObject.Enemy;
            Belonging = Belonging.Enemy;

            SubFormationOffset.X = -1;
            SubFormationOffset.Y = -1;

            MainFormationOffset = formationOffset;
            EnemyId = id;

            waypoints = new Dictionary<Vector2, bool>();
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

                        //if ((tmpDataString = sr.ReadLine()) == null) break;
                        //position = new Vector2(
                        //    Convert.ToSingle(tmpDataString.Substring(tmpDataString.IndexOf('x') + 1,
                        //        tmpDataString.LastIndexOf(' ') - tmpDataString.IndexOf('x'))),
                        //    Convert.ToSingle(tmpDataString.Substring(tmpDataString.IndexOf('y') + 1)));
                        //позиция будет читаться из другого файла

                        position = startPosition;
                        break;

                    }
                }
            }
            waypoints.Add(new Vector2(2, 5), true);
            waypoints.Add(new Vector2(2, -5), false);

            //надо разобраться с тем, как с этим работать =\
            //https://msdn.microsoft.com/ru-ru/library/k3z2hhax(v=vs.110).aspx
            //var test = waypoints.GetEnumerator();
        }
        #endregion

        #region Updaters
        public new void Update()
        {
            switch (InGameState)
            {
                case OnLevelStates.LevelStart:
                {
                    StateChanger();
                    break;
                }
                case OnLevelStates.MovingStart:
                {
                    break;
                }
                case OnLevelStates.InMainFormation:
                {
                    position = Level.MainFormation.Position + MainFormationOffset;
                    StateChanger();//надо придумать проверку на возможность вылета
                    break;
                }
                case OnLevelStates.Moving:
                {
                    Moving();
                    break;
                }
                case OnLevelStates.MovingEnd:
                {
                    Moving();
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
                case OnLevelStates.LevelStart:
                {
                    InGameState = OnLevelStates.InMainFormation; //временно
                    break;
                }
                case OnLevelStates.MovingStart:
                {
                    break;
                }
                case OnLevelStates.InMainFormation:
                {
                    velocity = new Vector2(0, MaxSpeed);
                    Enumerator = waypoints.GetEnumerator();
                    waypointsNotEnd = Enumerator.MoveNext();
                    currentWaypoint = Enumerator.Current;
                    waypointsNotEnd = Enumerator.MoveNext();
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
            switch (InGameState)
            {
                case OnLevelStates.Moving:
                {
                    if (waypointsNotEnd)
                    {
                        if (currentWaypoint.Value)
                            MoveAround(currentWaypoint.Key);
                        else
                            MoveTo(currentWaypoint.Key);
                        break;
                    }
                    if (currentWaypoint.Value)
                        MoveAround(currentWaypoint.Key);
                    else
                        MoveTo(currentWaypoint.Key);
                    break;
                }
                case OnLevelStates.MovingEnd:
                {
                    MoveToMainFormation();
                    break;
                }
                default:
                    break;
            }
        }

        private void MoveTo(Vector2 point)
        {
            Vector2 unitVelocity = (point - position) /
                                   (float)Math.Sqrt((point.X - position.X) * (point.X - position.X) +
                                                     (point.Y - position.Y) * (point.Y - position.Y)) * MaxSpeed;
            velocity = unitVelocity;
            base.Moving();

            if (Math.Abs(Position.X - currentWaypoint.Key.X) < 0.2 &&
                Math.Abs(Position.Y - currentWaypoint.Key.Y) < 0.2)
            {
                if (waypointsNotEnd)
                {
                    currentWaypoint = Enumerator.Current;
                    waypointsNotEnd = Enumerator.MoveNext();
                }
                else
                {
                    StateChanger();
                }
            }
        }

        private void MoveAround(Vector2 point)
        {
            float radius = (float)Math.Sqrt((point.X - position.X) * (point.X - position.X) +
                                             (point.Y - position.Y) * (point.Y - position.Y));
            float angularVelocity = MaxSpeed / radius;
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

            if (waypointsNotEnd && Math.Abs(direction - DirectionCalc(position, Enumerator.Current.Key)) < 0.15)
            {
                currentWaypoint = Enumerator.Current;
                waypointsNotEnd = Enumerator.MoveNext();
            }
            else if (!waypointsNotEnd && Math.Abs(direction - DirectionCalc(position, Level.MainFormation.Position + MainFormationOffset)) < 0.15)
            {
                StateChanger();
            }
        }

        private void MoveToMainFormation()
        {
            Vector2 moveToPoint = Level.MainFormation.Position + MainFormationOffset;
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

        public bool IsMoving => OnLevelStates.Moving == InGameState;
        #endregion

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