using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

//содержит переменные:
//  стоимость
//  имя текстуры
//  текущее положение
//  текущую скорость
//  смещение относительно центра группы
//и методы:
//  изменение скорости (для возможности перемещения)
//  выстрел
//  деструктор должен так же отрисовать "взрыв"

namespace Galaga
{
    class Enemy : Moved
    {
        private Vector2 centerOffset;
        private int cost;
        private bool isMoving;
        private bool movingEnd;
        private static int costPerLvl = 40;
        private int subFormation;

        public Enemy(int enemyLvl, Vector2 newCentralOffset)
        {
            cost = enemyLvl* costPerLvl;
            centerOffset = newCentralOffset;

            velocity.X = 0;
            velocity.Y = 0;

            isMoving = false;
        }
        public Vector2 GetCenterOffset() { return centerOffset; }
        public int GetCost() { return cost; }
        public void RenderObject()
        {
            if (movingEnd)
            {
                Vector2 tmpVelocity = velocity;
                tmpVelocity.X = 0;
                textures.RenderObject(position, (cost / costPerLvl) - 1, tmpVelocity);
            }
            else if (velocity.X == 0 && velocity.Y == 0) { textures.RenderObject(GlobalVariables.GetCenterEnemyPosition() + centerOffset, (cost / costPerLvl) - 1,velocity); }
            else textures.RenderObject(position, (cost / costPerLvl) - 1,velocity);
        }
        public void StartMove()
        {
            position = GlobalVariables.GetCenterEnemyPosition() + centerOffset;
            velocity.Y = 0.3F;
            if (centerOffset.X > 0) velocity.X = 0.4F;
            if (centerOffset.X < 0) velocity.X = -0.4F;
            isMoving = true;
            movingEnd = false;
        }
        public void StartMove(int newSubFormation)
        {
            position = GlobalVariables.GetCenterEnemyPosition() + centerOffset;
            velocity.Y = 0.3F;
            if (newSubFormation > 0) velocity.X = 0.4F;
            if (newSubFormation < 0) velocity.X = -0.4F;
            subFormation = newSubFormation;
            isMoving = true;
            movingEnd = false;
        }
        public void Moving(Vector2 PlayerPosition)
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
                subFormation = 0;
                position.Y = 11;
//                velocity.X = 0;
            }
            if (GlobalVariables.isAllMoving) movingEnd = false;//проверить, возможно ли прикрутить сюда, 
            //что бы в конце игры удалялись дорогие противники (можно если возвращать, к примеру bool)
            
            if (movingEnd ) position.X = GlobalVariables.GetCenterEnemyPosition().X + centerOffset.X;
            if (movingEnd && position.Y <= GlobalVariables.GetCenterEnemyPosition().Y + centerOffset.Y)
            {
                velocity.X = 0;
                velocity.Y = 0;
                isMoving = false;
                movingEnd = false;
            }
        }
        public bool GetIsMoving() { return isMoving; }
        public int GetCostPeLvl() { return costPerLvl; }
        public Bullet Shoot()
        {
            Vector2 tmpPos = position;
            tmpPos.Y--;
            return new Bullet(tmpPos, false);
        }
        public int GetSubFormation() { return subFormation; }
    }
}
