using System;
using System.Threading;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace Galaga
{
    class Program : GameWindow
    {
        #region variables

        private Renderer _renderer;
        SoundMaster _soundMaster;

        #endregion

        static void Main()
        {
            using (var game = new Program())
            {
                game.Run(30);
            }
        }
        public Program() : base((int)WindowProperty.WindowSize.X, (int)WindowProperty.WindowSize.Y, GraphicsMode.Default, "Galaga")
        {
            VSync = VSyncMode.On;
            Keyboard.KeyDown += KeyboardInput.KeyDown;//хотя тут можно сразу на статическую ф-ю ссылаться тогда =) (короче, проверить, нормально ли работает в таком виде)
            Keyboard.KeyUp += KeyboardInput.KeyUp;// не уверен что без серого заработает, так что надо проверить
        }
        protected void OnKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            KeyboardInput.KeyUp(sender, e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _renderer = new Renderer();

            //надо переписать
//            _soundMaster = new SoundMaster();
//            Thread soundMasterThread = new Thread(_soundMaster.SoundPlay);
//            soundMasterThread.Start();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //тоже вынести в какой то отдельный класс, отвечающий чисто за окна
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            if (ClientSize.Width < (int)WindowProperty.WindowSize.X)
            {
                ClientSize = new Size((int)WindowProperty.WindowSize.X, ClientSize.Height);
            }
            if (ClientSize.Height < (int)WindowProperty.WindowSize.Y)
            {
                ClientSize = new Size(ClientSize.Width, (int)WindowProperty.WindowSize.Y);
            }
            if (ClientSize.Width > (int)WindowProperty.WindowSize.X)
            {
                ClientSize = new Size((int)WindowProperty.WindowSize.X, ClientSize.Height);
            }
            if (ClientSize.Height > (int)WindowProperty.WindowSize.Y)
            {
                ClientSize = new Size(ClientSize.Width, (int)WindowProperty.WindowSize.Y);
            }

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            Level.Update();

            if (GameStates.GameState == GameState.Exit) Exit();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _renderer.Render();

            SwapBuffers();
        }
    }
}
