using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Galaga
{
    class Textures
    {
        private static int enemyTypeNum = 4;
        private static Texture playerTexture;
        private static Texture[] enemyTexture = new Texture[enemyTypeNum];
        private static Texture bulletTexture;

        public Textures()
        {
            playerTexture = new Texture(new Bitmap("Texture/PlayerShip.png"));
            for (int i = 0; i < enemyTypeNum; i++)
            {
                enemyTexture[i] = new Texture(new Bitmap("Texture/Enemy1_" + i + ".png"));
            }
            bulletTexture = new Texture(new Bitmap("Texture/Bullet.png"));
        }

        public void RenderObject(Vector2 Coords, int obj)   //0 -- (enemyTypeNum-1) - enemy, enemyTypeNum - player, -1 bullet
        {
            //заменить if на swich-case
            if (obj == enemyTypeNum) playerTexture.Bind();
            else if (obj >= 0 && obj < enemyTypeNum) enemyTexture[obj].Bind();
            else if (obj == -1) bulletTexture.Bind();
            else return;
            //            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(1, 1);
            GL.Vertex2(Coords.X * GlobalVariables.GetGlObjectSize().X, Coords.Y * GlobalVariables.GetGlObjectSize().Y);

            GL.TexCoord2(0, 1);
            GL.Vertex2((Coords.X + 1) * GlobalVariables.GetGlObjectSize().X, Coords.Y * GlobalVariables.GetGlObjectSize().Y);

            GL.TexCoord2(0, 0);
            GL.Vertex2((Coords.X + 1) * GlobalVariables.GetGlObjectSize().X, (Coords.Y + 1) * GlobalVariables.GetGlObjectSize().Y);

            GL.TexCoord2(1, 0);
            GL.Vertex2(Coords.X * GlobalVariables.GetGlObjectSize().X, (Coords.Y + 1) * GlobalVariables.GetGlObjectSize().Y);

            GL.End();
        }
        public int GetEnemyTypeNum() { return enemyTypeNum; }
    }
}
