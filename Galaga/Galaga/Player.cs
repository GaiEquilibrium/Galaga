using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

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
        public bool CanShoot;

        public Player()//возможно стоит юзать заранее подготовленную текстуру
        {
            position.X = -0.5F;
            position.Y = -7;
            lifeNum = 3;
            score = 0;
        }
        public int GetLifeNum() { return lifeNum; }
        public int GetScore() { return score; }
        public void ReduceLifeNum() { lifeNum--; }
        public void AddToScore(int addedScore) { score += addedScore; }
        public void Moving(int direction)
        {
            if (direction > 0)
            {
                position.X += 0.15F;   //вправо
                prevMoveDirection = 1;
            }
            if (direction < 0)
            {
                position.X -= 0.15F;   //влево
                prevMoveDirection = -1;
            }
        }
        public new void Moving() { Moving(prevMoveDirection); }
        public Bullet Shoot()
        {
            Vector2 tmpPos = position;
            tmpPos.Y++;
            return new Bullet(tmpPos,true);
        }
        public void Reset()
        {
            lifeNum--;
            position.X = -0.5F;
            if (lifeNum > 0) { position.Y = -7; }
            else { position.Y = 20; }
        }
        public void RenderObject() { RenderObject(textures.GetEnemyTypeNum()); }
        public void RenderLifes()//подумать над более удобным вариантом
        {
            Vector2 tmpPos;
            tmpPos.X = -10;
            tmpPos.Y = -14;
            for (int i = 1; i < lifeNum; i++)
            {
                textures.RenderObject(tmpPos, textures.GetEnemyTypeNum(),velocity);
                tmpPos.X++;
            }
        }
    }
}
