using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;

namespace Galaga
{
    class Textures
    {
        private static int enemyTypeNum = 4;
        private static Texture playerTexture;
        private static Texture[,] enemyTexture = new Texture[enemyTypeNum, Enemy.maxStage];
        private static Texture bulletTexture;

        public Textures()
        {
            playerTexture = new Texture(new Bitmap("Texture/1/PlayerShip.png"));
            for (int i = 0; i < enemyTypeNum; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    enemyTexture[i, j] = new Texture(new Bitmap("Texture/1/Enemy" + i.ToString() + j.ToString() + ".png"));
                }
            }
            bulletTexture = new Texture(new Bitmap("Texture/Bullet.png"));
        }

        public void RenderObject(Vector2 Coords, int obj, Vector2 velocity, int stage)   //0 -- (enemyTypeNum-1) - enemy, enemyTypeNum - player, -1 bullet
        {
            //заменить if на swich-case
            if (obj == enemyTypeNum) playerTexture.Bind();
            else if (obj >= 0 && obj < enemyTypeNum) enemyTexture[obj, stage].Bind();
            else if (obj == -1) bulletTexture.Bind();
            else return;

            GL.Color4(Color4.White);

            float angle = 0;
            if (velocity.X == 0 && velocity.Y == 0 || obj == enemyTypeNum) { angle = 0; }
            else
            {
                float tmpVelocityY = velocity.Y / (float)(Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y));
                if (tmpVelocityY >= 1) angle = 0;
                else if (tmpVelocityY <= -1) angle = 180;
                else { angle = (float)(Math.Acos(tmpVelocityY)*180/Math.PI); }
                if (velocity.X > 0) angle = -angle;
            }
            GL.PushMatrix();
            GL.Translate((Coords.X + 0.5) * GlobalVariables.GetGlObjectSize().X, (Coords.Y + 0.5) * GlobalVariables.GetGlObjectSize().Y, 0);//хммм
            GL.Rotate(angle, 0, 0, 1);

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
        }
        public int GetEnemyTypeNum() { return enemyTypeNum; }
    }
}
