using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace Galaga
{
    //вероятнее всего, как сам является фоном, так хранит и работает со всеми фоновыми объектами
    //возможно сам фон стоит вынести в отдельный класс?
    static class Background
    {
        static Texture _backgroundTexture = new Texture(new Bitmap("Texture/background.png"));
        public static void RenderBackground()
        {
            _backgroundTexture.Bind();
            GL.Color4(Color4.Black);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, -1);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, -1);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, 1);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, 1);

            GL.End();
//            foreach (Star star in starList)
//            {
//                star.RenderStar();
//            }
        }
    }
}
