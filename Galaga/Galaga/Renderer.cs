using OpenTK.Graphics.OpenGL;

namespace Galaga
{
    class Renderer
    {
        public Renderer()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

//            Background.RenderBackground();
//            Background.RenderBackground();
            if (GameStates.IsGame)
            {
                Background.RenderBackground();
                Level.Render();
            }
            else if (GameStates.IsMenu)
            {
                MenuRenderer.Render();
            }
        }
    }
}
