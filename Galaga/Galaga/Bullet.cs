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
        public Bullet(Vector2 startPosition)
        {
            position = startPosition;
            velocity.Y = 0.3F;

            SetTexture("Texture/Bullet.png");
        }
    }
}
