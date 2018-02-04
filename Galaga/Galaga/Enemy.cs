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
        private Vector2d CenterOffset;
        private int cost;

        public Enemy(int cost, String textureName)
        {
            this.cost = cost;
            SetTexture(textureName);
        }
        public Vector2d GetYCenterOffset() { return CenterOffset; }
        public int GetCost() { return cost; }
    }
}
