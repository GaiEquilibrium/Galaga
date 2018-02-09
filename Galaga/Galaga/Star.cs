using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Galaga
{
    class Star
    {
        private Vector2 position;
        private Vector2 velocity;
        private int stage;
        private static int frameToStage = 50;
        private static int starStage = 4;
        private static Texture[] starTexture = new Texture[starStage];

        static Star()
        {
            for (int i = 0; i < starStage; i++)
            {
                starTexture[i] = new Texture(new Bitmap("Texture/Star" + i + ".png"));
            }
        }
        public Star(float newVelocity, Vector2 newPosition, int newStage)
        {
            velocity.Y = -newVelocity;
            velocity.X = 0;
            position = newPosition;
            stage = newStage;
        }
        public void Moving()
        {
            position += velocity;
            if (position.Y < -10) position.Y = 10;
        }
        public void RenderStar()
        {
            starTexture[stage / frameToStage].Bind();
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(1, 1);
            GL.Vertex2(position.X * GlobalVariables.GetGlObjectSize().X, position.Y * GlobalVariables.GetGlObjectSize().Y);
            GL.TexCoord2(0, 1);
            GL.Vertex2((position.X + 1) * GlobalVariables.GetGlObjectSize().X, position.Y * GlobalVariables.GetGlObjectSize().Y);
            GL.TexCoord2(0, 0);
            GL.Vertex2((position.X + 1) * GlobalVariables.GetGlObjectSize().X, (position.Y + 1) * GlobalVariables.GetGlObjectSize().Y);
            GL.TexCoord2(1, 0);
            GL.Vertex2(position.X * GlobalVariables.GetGlObjectSize().X, (position.Y + 1) * GlobalVariables.GetGlObjectSize().Y);
            GL.End();
            stage++;
            if (stage > 99) stage = 0;
        }
    }
}
