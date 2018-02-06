using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Player player;
        Texture backgroundTexture;
        List<Enemy> enemyList = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        Vector2 tmpEnemyOffset;
        

        int isKeyPressed = 0;
        #endregion

        static void Main()
        {
            using (var Game = new Program())
            {
                Game.Run(30);
            }
        }
        public Program() : base((int)GlobalVariables.GetWindowSize().X, (int)GlobalVariables.GetWindowSize().Y, GraphicsMode.Default, "Galaga")
        {
            VSync = VSyncMode.On;
            Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(OnKeyDown);
            Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(OnKeyUp);
        }
        protected void OnKeyDown(object Sender, KeyboardKeyEventArgs E)
        {
            if (Key.Left == E.Key)
            {
                isKeyPressed ++;
                player.Moving(-1);
            }
            if (Key.Right == E.Key)
            {
                isKeyPressed ++;
                player.Moving(1);
            }
            if (Key.Space == E.Key)
            {
                bullets.Add(player.Shoot());
            }
        }
        protected void OnKeyUp(object Sender, KeyboardKeyEventArgs E)
        {
            if (Key.Left == E.Key || Key.Right == E.Key)
            {
                isKeyPressed --;
            }
        }
        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            
            player = new Player("Texture/PlayerShip.png");
            tmpEnemyOffset.X = -3;
            tmpEnemyOffset.Y = 2;

            for (int i = 0; i<6; i++)
            {
                enemyList.Add(new Enemy(0, "Texture/Enemy1_1.png", tmpEnemyOffset));
                tmpEnemyOffset.X ++;
            }

            backgroundTexture = new Texture(new Bitmap("Texture/background.png"));
        }
        protected override void OnResize(EventArgs E)
        {
            base.OnResize(E);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

//как минимум пока что, нафиг вообще какую либо смену размеров
//            ProjectionWidth = NominalWidth;
//            ProjectionHeight = (float)ClientRectangle.Height / (float)ClientRectangle.Width * ProjectionWidth;
//            if (ProjectionHeight < NominalHeight)
//            {
//                ProjectionHeight = NominalHeight;
//                ProjectionWidth = (float)ClientRectangle.Width / (float)ClientRectangle.Height * ProjectionHeight;
//            }
            if (ClientSize.Width < (int)GlobalVariables.GetWindowSize().X)
            {
                ClientSize = new Size((int)GlobalVariables.GetWindowSize().X, ClientSize.Height);
            }
            if (ClientSize.Height < (int)GlobalVariables.GetWindowSize().Y)
            {
                ClientSize = new Size(ClientSize.Width, (int)GlobalVariables.GetWindowSize().Y);
            }
            if (ClientSize.Width > (int)GlobalVariables.GetWindowSize().X)
            {
                ClientSize = new Size((int)GlobalVariables.GetWindowSize().X, ClientSize.Height);
            }
            if (ClientSize.Height > (int)GlobalVariables.GetWindowSize().Y)
            {
                ClientSize = new Size(ClientSize.Width, (int)GlobalVariables.GetWindowSize().Y);
            }

        }
        protected override void OnUpdateFrame(FrameEventArgs E)
        {
            base.OnUpdateFrame(E);
            if (isKeyPressed !=0) player.Moving();
            //на самом деле этот метод вероятно не очень правильный
            //ибо в таком варианте оно позволяет перемещать игрока в противоположном направленному направлении

            GlobalVariables.MoveCenterEnemyPosition();
            foreach (Bullet bullet in bullets) { bullet.Moving(); }

            //немного хитра проверка попадания, надо будет проверить, не ли варианта лучше
            bool tmpIsHit = false;
            bool tmpIsAll = false;
            while (!tmpIsAll)
            {
                foreach (Enemy enemy in enemyList)
                {
                    foreach (Bullet bullet in bullets)
                    {
                        if (IsHit(bullet.GetPos(), (GlobalVariables.GetCenterEnemyPosition() + enemy.GetCenterOffset())))
                        {
                            enemyList.Remove(enemy);
                            bullets.Remove(bullet);
                            tmpIsHit = true;
                            break;
                        }
                    }
                    if (tmpIsHit) break;
                }
                if (tmpIsHit) tmpIsHit = false;
                else tmpIsAll = true;
            }
        }
        protected override void OnRenderFrame(FrameEventArgs E)
        {
            base.OnRenderFrame(E);

//            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(BeginMode.Quads);

            RenderBackground();
            RenderBackground();
            player.RenderObject(player.GetPos());

            //for (int i = 0; i < 6; i++) enemys[i].RenderObject();
            foreach (Enemy enemy in enemyList) { enemy.RenderObject(); }
            foreach (Bullet bullet in bullets) { bullet.RenderObject(bullet.GetPos()); }

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
        private bool IsHit(Vector2 bulletPos, Vector2 shipPos)
        {
            if (((bulletPos.X + 0.6F) > shipPos.X) && ((bulletPos.X + 0.4F) < (shipPos.X + 1)) &&
                ((bulletPos.Y + 0.95F) > shipPos.Y) && ((bulletPos.Y + 0.05F) < (shipPos.Y + 1)))
                return true;

            return false;
        }
    }
}
