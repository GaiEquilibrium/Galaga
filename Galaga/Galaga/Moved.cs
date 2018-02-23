using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Galaga
{
    class Moved
    {
        protected Vector2 position;
        protected Vector2 velocity;
        protected Textures textures = new Textures();

        public Vector2 GetPos() { return position; }
        public Vector2 GetVel() { return velocity; }
        public void SetVel(Vector2 vel) { velocity = vel; } //  !!!проверить, есть ли способ обойтись без этого!!!
        public void Moving() { position += velocity; }
        public void RenderObject(int obj) { textures.RenderObject(position,obj,velocity,0); }
    }
}
