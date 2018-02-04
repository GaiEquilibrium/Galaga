using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//содержит переменные:
//  текущее положение
//  указание на текстуру
//  количество оставшихся жизней
//  счёт
//и методы:
//  перемещение
//  выстрел

namespace Galaga
{
    class Player : Moved
    {
        private int lifeNum;
        private int score;

        public Player(String textureName)
        {
            position.X = 0;
            position.Y = 0;
            lifeNum = 3;
            score = 0;
            SetTexture(textureName);
        }
        public int GetLifeNum() { return lifeNum; }
        public int GetScore() { return score; }
        public void ReduceLifeNum() { lifeNum--; }
        public void AddToScore(int addedScore) { score += addedScore; }
        public void Moving(int direction)
        {
            if (direction > 0) position.X += 0.1;   //вправо
            if (direction < 0) position.X -= 0.1;   //влево
        }

    }
}
