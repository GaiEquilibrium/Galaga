using System;
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

        //calculation direction in radians
        public float DirectionCalc(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 difference = endPoint - startPoint;
            if (difference.X != 0)
            {
                return (float)Math.Atan2(difference.Y, difference.X);
            }
            else
            {
                if (difference.Y > 0)
                {
                    return (float)Math.PI / 2;
                }
                else
                {
                    return (float)-Math.PI / 2;
                }
            }
        }
        //same, but use velocity to calculations
        public float DirectionCalc()
        {
            if (Velocity.X != 0)
            {
                return (float)Math.Atan2(Velocity.Y, Velocity.X);
            }
            else
            {
                if (Velocity.Y > 0)
                {
                    return (float)Math.PI / 2;
                }
                else
                {
                    return (float)-Math.PI / 2;
                }
            }
        }
    }
}
