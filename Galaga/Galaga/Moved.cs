using OpenTK;

namespace Galaga
{
    //по сути родитель для всех перемещающихся объектов и содержит самые простые общие методы
    class Moved
    {
        protected Vector2 position;
        protected Vector2 velocity;
//        protected Texture texture;// = new Texture();

        public Vector2 GetPos() { return position; }
        public Vector2 GetVel() { return velocity; }
        public void SetVel(Vector2 vel) { velocity = vel; } //  !!!проверить, есть ли способ обойтись без этого!!!
        public void Moving() { position += velocity; }
//        public void RenderObject(int obj) { texture.RenderObject(position,obj,velocity,0); }
    }
}
