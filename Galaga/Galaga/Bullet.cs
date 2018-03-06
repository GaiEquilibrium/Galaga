using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Galaga
{
    //отвечает за выстрел, когда он виден и активен
    //по сути, только методы на проверки столкновений (и возможно собственно методы уничтожения)
    class Bullet : Moved
    {
        private bool owner; //true - player, false - enemy

        public Bullet(Vector2 startPosition, bool newOwner)
        {
            position = startPosition;
            owner = newOwner;
            if (newOwner) velocity.Y = 0.8F;
            else velocity.Y = -0.8F;
        }
        public bool IsPlayerOwner() { return owner; }
        public void RenderObject()
        {
//            RenderObject(-1);
        }
    }
}
