using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Galaga
{
    class Moved : Object
    {
        protected Vector2d position;
        protected Vector2d velocity;

        public Vector2d GetPos() { return position; }
        public Vector2d GetVel() { return velocity; }
    }
}
