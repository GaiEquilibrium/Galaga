using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Galaga
{
    //только методы на проверки столкновений (и возможно собственно методы уничтожения)
    class Bullet : Moved
    {
        private bool owner; //true - player, false - enemy
        public Bullet(Vector2 startPosition, bool newOwner)
        {
            position = startPosition;
            owner = newOwner;
            if (newOwner) velocity.Y = 0.8F;
            else velocity.Y = -0.8F;
            SetTexture("Texture/Bullet.png");
        }
        public bool IsPlayerOwner() { return owner; }
    }
}
