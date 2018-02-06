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

        public void RenderObject(Vector2 Coords)   //хммм
        {
            texture.Bind();
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

        protected void SetTexture(String path)
        {
            texture = new Texture(new Bitmap(path));
        }
    }
}
