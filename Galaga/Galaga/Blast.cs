using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Galaga
{
    class Blast
    {
        private Vector2 position;
        private int stage;
        bool isPlayer;

        static int frameToStage = 3;
        private static int enemyStage = 5;
        private static int playerStage = 4;
        private static Texture[] enemyTexture = new Texture[enemyStage];
        private static Texture[] playerTexture = new Texture[playerStage];

        static Blast()
        {
            for (int i = 0; i < enemyStage; i++)
            {
                enemyTexture[i] = new Texture(new Bitmap("Texture/BlastE1_" + i + ".png"));
            }
            for (int i = 0; i < playerStage; i++)
            {
                playerTexture[i] = new Texture(new Bitmap("Texture/BlastP1_" + i + ".png"));
            }
        }
        public Blast(Vector2 newPositiion, bool newIsPlayer)
        {
            isPlayer = newIsPlayer;
            position = newPositiion;
            stage = 0;
        }
        public bool RenderBlast()//true - blast complete
        {
            if (isPlayer && stage < playerStage*frameToStage) playerTexture[stage/ frameToStage].Bind();
            else if (!isPlayer && stage < enemyStage* frameToStage) enemyTexture[stage/ frameToStage].Bind();
            else return true;

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
            return false;
        }
    }
}
