using OpenTK;

namespace Galaga
{
    public enum GameObject
    {
        Player,
        Enemy,
        Boss,
        Star,
        Blast,
        Bullet
    }

    public enum Belonging
    {
        Player,
        Enemy,
        None
    }

    public abstract class Moved
    {
        protected Vector2 position;
        protected Vector2 velocity;
        protected int frameToStage;

        public int Counter;
        public GameObject GameObject { get; protected set; }
        public Belonging Belonging { get; protected set; }
        public Vector2 Position => position;
        public Vector2 Velocity => velocity;
        public int State { get; protected set; }    //тут можно ещё и хранить здоровье боссов параллельно

        public void Moving() { position += Velocity; }

        public void Render() { Textures.Render(this); }

        public void Update()
        {
            Moving();
        }
    }
}
