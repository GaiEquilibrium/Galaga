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

            GL.PushMatrix();
            GL.Translate((position.X+0.5) * GlobalVariables.GetGlObjectSize().X, (position.Y + 0.5) * GlobalVariables.GetGlObjectSize().Y, 0);//хммм

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(1, 1);
            GL.Vertex2(-GlobalVariables.GetGlObjectSize().X / 2, -GlobalVariables.GetGlObjectSize().Y / 2);

            GL.TexCoord2(0, 1);
            GL.Vertex2(GlobalVariables.GetGlObjectSize().X / 2, -GlobalVariables.GetGlObjectSize().Y / 2);

            GL.TexCoord2(0, 0);
            GL.Vertex2(GlobalVariables.GetGlObjectSize().X / 2, GlobalVariables.GetGlObjectSize().Y / 2);

            GL.TexCoord2(1, 0);
            GL.Vertex2(-GlobalVariables.GetGlObjectSize().X / 2, GlobalVariables.GetGlObjectSize().Y / 2);

            GL.End();
            GL.PopMatrix();

            stage++;
            return false;
        }
    }
}
