using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Galaga
{
    //отвечает за работу звезды
    //(т.к. все звёзды это всё таки пока что просто массив звёзд)
    class Star:Moved
    {
        private static int _frameToStage = 50;
        private static int _starStage = 4;

        public Star(float newVelocity, Vector2 newPosition, int newStage)
        {
            velocity.Y = -newVelocity;
            velocity.X = 0;
            position = newPosition;
            State = newStage;
            Belonging = Belonging.None;
            GameObject = GameObject.Star;
        }
        public new void Moving()
        {
            base.Moving();
            if (position.Y < -10) position.Y = 10;
        }

        public new void Update()
        {
//            throw new System.NotImplementedException();
            Moving();
        }
    }
}
