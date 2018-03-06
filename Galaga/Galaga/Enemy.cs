using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Galaga
{
    //отвечает за противника
    //содержит в себе основные его характеристики, и отвечает за его возможные действия
    //является родителем для противника-босса
    class Enemy : Ship
    {
        protected Vector2 offset;//formation start in bottom left corner
        protected int cost;
        protected bool isMoving;
        protected bool movingEnd;
        protected int formation;
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
        }

        public Enemy(int newCost, Vector2 startPosition)
        {
            cost = newCost;
//            offset = newOffset;

            velocity.X = 0;
            velocity.Y = 0;

            isMoving = false;
        }
        public void ChangeFormation(int newFormation, Vector2 newOffset)
        {
            formation = newFormation;
            offset = newOffset;
        }
        public void RenderObject()
        {
/*            stage++;
            if (stage >= frameToStage * maxStage) stage = 0;
            if (movingEnd)
            {
                Vector2 tmpVelocity = velocity;
                tmpVelocity.X = 0;
                textures.RenderObject(position, (cost / costPerLvl) - 1, tmpVelocity, stage/frameToStage);
            }
            else if (velocity.X == 0 && velocity.Y == 0) { textures.RenderObject(GlobalVariables.GetCenterEnemyPosition() + centerOffset, (cost / costPerLvl) - 1,velocity, stage / frameToStage); }
            else textures.RenderObject(position, (cost / costPerLvl) - 1,velocity, stage / frameToStage);*/
        }
        public void StartMove(Vector2 formationPosition)
        {
//            position = GlobalVariables.GetCenterEnemyPosition() + centerOffset;
//заменить на считывание позиции у текущего строя
            position = offset+ formationPosition;
            velocity.Y = 0.3F;
            if (offset.X > 0) velocity.X = 0.4F;
            if (offset.X < 0) velocity.X = -0.4F;
            isMoving = true;
            movingEnd = false;
        }
        public void StartMove(int newSubFormation)  //хммм
        {
//            position = GlobalVariables.GetCenterEnemyPosition() + centerOffset;
            velocity.Y = 0.3F;
            if (newSubFormation > 0) velocity.X = 0.4F;
            if (newSubFormation < 0) velocity.X = -0.4F;
//            subFormation = newSubFormation;
            isMoving = true;
            movingEnd = false;
        }
        public void Moving(Vector2 PlayerPosition)  //вообще переписать 
        { 
            position += velocity;
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
            if (GlobalVariables.isAllMoving) movingEnd = false;//проверить, возможно ли прикрутить сюда, 
            //что бы в конце игры удалялись дорогие противники (можно если возвращать, к примеру bool)
            
//            if (movingEnd ) position.X = GlobalVariables.GetCenterEnemyPosition().X + centerOffset.X;
//            if (movingEnd && position.Y <= GlobalVariables.GetCenterEnemyPosition().Y + centerOffset.Y)
            {
                velocity.X = 0;
                velocity.Y = 0;
                isMoving = false;
                movingEnd = false;
            }
        }
        public Bullet Shoot()
        {
            GlobalVariables.ShootFlagInc();
            Vector2 tmpPos = position;
            tmpPos.Y--;
            return new Bullet(tmpPos, false);
        }

        public Vector2 Offset
        {
            get { return offset; }
        }
        public int Cost
        {
            get { return cost; }
        }
        public bool IsMoving
        {
            get { return isMoving; }
        }
        public int Formation
        {
            get { return formation; }
        }
//        public int GetCostPeLvl() { return costPerLvl; }
    }
}
