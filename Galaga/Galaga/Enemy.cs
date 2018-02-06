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
        
        public Enemy(int cost, String textureName, Vector2 newCentralOffset)
        {
            this.cost = cost;
            SetTexture(textureName);
            centerOffset = newCentralOffset;

            velocity.X = 0;
            velocity.Y = 0;
        }
        public Vector2 GetCenterOffset() { return centerOffset; }
        public int GetCost() { return cost; }
        public void RenderObject()
        {
            if (velocity.X == 0) { RenderObject(GlobalVariables.GetCenterEnemyPosition() + centerOffset); }
        }
    }
}
