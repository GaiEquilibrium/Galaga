﻿using OpenTK;

namespace Galaga
{
    public class Enemy : Ship
    {
        protected int cost;
        protected bool isMoving;
        protected bool movingEnd;
        public Vector2 MainFormationOffset; //formation start in bottom left corner
        public Vector2 SubFormationOffset;
        public int EnemyId { get; }

//написать более толковую систему анимации
//        private int stage;
//        public const int maxStage = 4;
//        public const int frameToStage = 20;

        public Enemy()
        {
            cost = 0;
            
            velocity.X = 0;
            velocity.Y = 0;

            isMoving = false;
            Belonging = Belonging.Enemy;

            SubFormationOffset.X = -1;
            SubFormationOffset.Y = -1;
        }

        public Enemy(int id, int newCost, Vector2 startPosition, Vector2 formationOffset)
        {
            cost = newCost;

            velocity.X = 0;
            velocity.Y = 0;

            isMoving = false;

            position = startPosition; //временная мера
            GameObject = GameObject.Enemy;
            Belonging = Belonging.Enemy;
            State = 0;  //временно

            SubFormationOffset.X = -1;
            SubFormationOffset.Y = -1;

            MainFormationOffset = formationOffset;
            EnemyId = id;
        }

        public void ChangeFormation(Vector2 newOffset)
        {
            SubFormationOffset = newOffset;
        }

        public void StartMove(Vector2 formationPosition)
        {
//            position = GlobalVariables.GetCenterEnemyPosition() + centerOffset;
//заменить на считывание позиции у текущего строя
//            position = offset + formationPosition;
//            velocity.Y = 0.3F;
//            if (offset.X > 0) velocity.X = 0.4F;
//            if (offset.X < 0) velocity.X = -0.4F;
//            isMoving = true;
//            movingEnd = false;
        }

        public void StartMove(int newSubFormation) //хммм
        {
//            position = GlobalVariables.GetCenterEnemyPosition() + centerOffset;
            velocity.Y = 0.3F;
            if (newSubFormation > 0) velocity.X = 0.4F;
            if (newSubFormation < 0) velocity.X = -0.4F;
//            subFormation = newSubFormation;
            isMoving = true;
            movingEnd = false;
        }

        public void Moving(Vector2 PlayerPosition) //вообще переписать 
        {
/*            position += velocity;
            if (velocity.Y > -0.1) velocity.Y -= 0.02F;
            if (!movingEnd)
            {
                if (PlayerPosition.X > position.X && velocity.X < 0.4) velocity.X += 0.02F;
                if (PlayerPosition.X < position.X && velocity.X > -0.4) velocity.X -= 0.02F;
            }
            if (position.Y < -7)
            {
                movingEnd = true;
//                subFormation = 0;
                position.Y = 11;
//                velocity.X = 0;
            }
            if (WindowProperty.isAllMoving) movingEnd = false; //проверить, возможно ли прикрутить сюда, 
            //что бы в конце игры удалялись дорогие противники (можно если возвращать, к примеру bool)

//            if (movingEnd ) position.X = GlobalVariables.GetCenterEnemyPosition().X + centerOffset.X;
//            if (movingEnd && position.Y <= GlobalVariables.GetCenterEnemyPosition().Y + centerOffset.Y)
            {
                velocity.X = 0;
                velocity.Y = 0;
                isMoving = false;
                movingEnd = false;
            }*/
        }

        public new void Update()
        {
            if (!isMoving)
            {
                if (!InSubFormation) position = Level.MainFormation.Position + MainFormationOffset;
//                else //а вот пока что хз как =\
            }
            else Moving();
        }

        public Bullet Shoot()
        {
//            SoundMaster.Shoot();//надо будет так же несколько переделать работу со звуком
            Vector2 tmpPos = position;
            tmpPos.Y--;
            return new Bullet(tmpPos, Belonging.Enemy,-1);
        }

        private Vector2 FormationOffset => InSubFormation ? SubFormationOffset : MainFormationOffset;

        public bool InSubFormation => SubFormationOffset.X >= 0;

        public int Cost => cost;

        public bool IsMoving => isMoving;

    }
}