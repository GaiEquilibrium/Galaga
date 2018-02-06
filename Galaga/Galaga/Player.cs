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
        private int prevMoveDirection;

        public Player(String textureName)//возможно стоит юзать заранее подготовленную текстуру
        {
            position.X = -0.5F;
            position.Y = -5;
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
            if (direction > 0)
            {
                position.X += 0.1F;   //вправо
                prevMoveDirection = 1;
            }
            if (direction < 0)
            {
                position.X -= 0.1F;   //влево
                prevMoveDirection = -1;
            }
        }
        public void Moving() { Moving(prevMoveDirection); }

    }
}
