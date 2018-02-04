using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Galaga
{
    class Program : GameWindow
    {
        #region variables
        Player player;
        Texture backgroundTexture;

        private float ProjectionWidth;
        private float ProjectionHeight;
        private const int NominalWidth = 700;
        private const int NominalHeight = 500;
        #endregion

        static void Main()
        {
            using (var Game = new Program())
            {
                Game.Run(30);
            }
        }

        public Program() : base(700, 500, GraphicsMode.Default, "Galaga")
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            player = new Player("Texture/FTLship.png");
            backgroundTexture = new Texture(new Bitmap("Texture/background.png"));
        }

        protected override void OnResize(EventArgs E)
        {
            base.OnResize(E);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            ProjectionWidth = NominalWidth;
            ProjectionHeight = (float)ClientRectangle.Height / (float)ClientRectangle.Width * ProjectionWidth;
            if (ProjectionHeight < NominalHeight)
            {
                ProjectionHeight = NominalHeight;
                ProjectionWidth = (float)ClientRectangle.Width / (float)ClientRectangle.Height * ProjectionHeight;
            }
            if (ClientSize.Width < NominalWidth)
            {
                ClientSize = new Size(NominalWidth, ClientSize.Height);
            }
            if (ClientSize.Height < NominalHeight)
            {
                ClientSize = new Size(ClientSize.Width, NominalHeight);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs E)
        {
            base.OnUpdateFrame(E);
        }

        protected override void OnRenderFrame(FrameEventArgs E)
        {
            base.OnRenderFrame(E);

//            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(BeginMode.Quads);

            RenderBackground();
            RenderBackground();
            player.RenderObject(player.GetPos(), 35);

            SwapBuffers();
        }

        private void RenderBackground()
        {
            backgroundTexture.Bind();
            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Quads);

//            GL.TexCoord2(0, 0);
//            GL.Vertex2(0, 0);
//
//            GL.TexCoord2((float)ClientRectangle.Width / backgroundTexture.Width, 0);
//            GL.Vertex2(ProjectionWidth, 0);
//
//            GL.TexCoord2((float)ClientRectangle.Width / backgroundTexture.Width, (float)ClientRectangle.Height / backgroundTexture.Height);
//            GL.Vertex2(ProjectionWidth, ProjectionHeight);
//
//            GL.TexCoord2(0, (float)ClientRectangle.Height / backgroundTexture.Height);
//            GL.Vertex2(0, ProjectionHeight);
//

            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, -1);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, -1);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, 1);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, 1);

            GL.End();
        }
    }
}
