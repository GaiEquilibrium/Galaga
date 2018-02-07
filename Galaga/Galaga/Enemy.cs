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

        public Enemy(int cost, String textureName, Vector2 newCentralOffset)
        {
            this.cost = cost;
            SetTexture(textureName);
            centerOffset = newCentralOffset;

            velocity.X = 0;
            velocity.Y = 0;

            isMoving = false;
        }
        public Vector2 GetCenterOffset() { return centerOffset; }
        public int GetCost() { return cost; }
        public void RenderObject()
        {
            if (velocity.X == 0) { RenderObject(GlobalVariables.GetCenterEnemyPosition() + centerOffset); }
            else RenderObject(position);
        }
        public void StartMove(Vector2 PlayerPosition)//поменять на private
        {
            position = GlobalVariables.GetCenterEnemyPosition() + centerOffset;
            velocity.Y = 0.3F;
            if (PlayerPosition.X > position.X) velocity.X = 0.3F;
            if (PlayerPosition.X < position.X) velocity.X = -0.3F;
            isMoving = true;
        }
        public void Moving(Vector2 PlayerPosition)
        {
            position += velocity;
            if (velocity.Y > -0.1) velocity.Y -= 0.02F;
            if (PlayerPosition.X > position.X && velocity.X < 0.3) velocity.X += 0.02F;
            if (PlayerPosition.X < position.X && velocity.X > -0.3) velocity.X -= 0.02F;
            if (position.Y < -5) position.Y = 4;//поменять на высчитываемое значение
        }
        public bool GetIsMoving() { return isMoving; }

    }
}
