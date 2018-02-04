using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Galaga
{
    class Object
    {
        private Texture texture;

        public void RenderObject(Vector2d Coords, int objectSize)   //хммм
        {
            //тест
            objectSize = 1;

            texture.Bind();
//            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(1, 1);
            GL.Vertex2(Coords.X * objectSize, Coords.Y * objectSize);

            GL.TexCoord2(0, 1);
            GL.Vertex2((Coords.X + 1) * objectSize, Coords.Y * objectSize);

            GL.TexCoord2(0, 0);
            GL.Vertex2((Coords.X + 1) * objectSize, (Coords.Y + 1) * objectSize);

            GL.TexCoord2(1, 0);
            GL.Vertex2(Coords.X * objectSize, (Coords.Y + 1) * objectSize);

            GL.End();
        }

        protected void SetTexture(String path)
        {
            texture = new Texture(new Bitmap(path));
        }
    }
}
