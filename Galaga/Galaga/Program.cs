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
using System.Drawing.Text;

namespace Galaga
{
    class Program : GameWindow
    {
        #region variables
        int state;
        //1 - Main menu
        //2 - Level preparation
        //3 - Playing
        //4 - Game Over
        //5 - Pause
        //6 - Exit
        //7 - Settings
        int menuChoice;
        //1 - Start Game    Resume          Start new game
        //2 - Settings      Exit to menu    Exit to menu
        //3 - Exit          Exit            Exit
        const int maxMenuChoice = 3;
        TextRenderer generalText;
        Texture menuFrame;

        Player player;
        Texture backgroundTexture;
        List<Enemy> enemyList = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        Vector2 tmpEnemyOffset;

        List<Enemy> subFormations = new List<Enemy>();
        List<Blast> blastList = new List<Blast>();
        List<Star> starList = new List<Star>();
        Random randomizer;
        int isKeyPressed = 0;

        TextRenderer scoreLabel;
        TextRenderer score;
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
            if (state == 1)
            {
                if (Key.Down == E.Key)
                {
                    menuChoice++;
                    if (menuChoice > maxMenuChoice) { menuChoice = 1; }
                }
                if (Key.Up == E.Key)
                {
                    menuChoice--;
                    if (menuChoice <= 0) { menuChoice = maxMenuChoice; }
                }
                if (Key.Enter == E.Key)
                {
                    if (menuChoice == 1)
                    {
                        menuChoice = 1;
                        state = 2;
                    }
                    if (menuChoice == 2)
                    {
                        menuChoice = 1;
                        state = 7;
                    }
                    if (menuChoice == 3)
                    {
                        menuChoice = 1;
                        state = 6;
                    }
                }
            }
            if (state == 3)
            {
                if (Key.Left == E.Key)
                {
                    isKeyPressed++;
                    player.Moving(-1);
                }
                if (Key.Right == E.Key)
                {
                    isKeyPressed++;
                    player.Moving(1);
                }
                if (Key.Space == E.Key)
                {
                    if (player.CanShoot) bullets.Add(player.Shoot());
                }
                if (Key.Escape == E.Key)
                {
                    state = 5;
                }
            }
            if (state == 5)
            {
                if (Key.Down == E.Key)
                {
                    menuChoice++;
                    if (menuChoice > maxMenuChoice) { menuChoice = 1; }
                }
                if (Key.Up == E.Key)
                {
                    menuChoice--;
                    if (menuChoice <= 0) { menuChoice = maxMenuChoice; }
                }
                if (Key.Enter == E.Key)
                {
                    if (menuChoice == 1)
                    {
                        menuChoice = 1;
                        state = 3;
                    }
                    if (menuChoice == 2)
                    {
                        menuChoice = 1;
                        state = 1;
                    }
                    if (menuChoice == 3)
                    {
                        menuChoice = 1;
                        state = 6;
                    }
                }
            }
            if (state == 4)
            {
                if (Key.Down == E.Key)
                {
                    menuChoice++;
                    if (menuChoice > maxMenuChoice) { menuChoice = 1; }
                }
                if (Key.Up == E.Key)
                {
                    menuChoice--;
                    if (menuChoice <= 0) { menuChoice = maxMenuChoice; }
                }
                if (Key.Enter == E.Key)
                {
                    if (menuChoice == 1)
                    {
                        menuChoice = 1;
                        state = 2;
                    }
                    if (menuChoice == 2)
                    {
                        menuChoice = 1;
                        state = 1;
                    }
                    if (menuChoice == 3)
                    {
                        menuChoice = 1;
                        state = 6;
                    }
                }
            }
            if (state == 7)
            {
                if (Key.Escape == E.Key)
                { state = 1; }
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
            state = 1;
            menuChoice = 1;
            generalText = new TextRenderer();

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            backgroundTexture = new Texture(new Bitmap("Texture/background.png"));
            menuFrame = new Texture(new Bitmap("Texture/menu_frame.png"));
            #region forDebug
            // debug part / отладочная часть 
            //            GL.Viewport(0, 0, (int)GlobalVariables.GetWindowSize().X, (int)GlobalVariables.GetWindowSize().Y);
            //            GL.MatrixMode(MatrixMode.Projection);
            //            GL.LoadIdentity();
            //            GL.Frustum(-1, 1, -1, 1, 1.5, 20);
            //            GL.MatrixMode(MatrixMode.Modelview);
            //            
            //            GL.Translate(0, 0, -3);
            #endregion

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
            if (state == 2)
            {
                LevelLoad();
                state = 3;
                return;
            }
            if (state == 3)
            {
                GlobalVariables.isAllMoving = true;
                if (isKeyPressed != 0) player.Moving();
                //на самом деле этот метод вероятно не очень правильный
                //ибо в таком варианте оно позволяет перемещать игрока в противоположном направленному направлении

                GlobalVariables.MoveCenterEnemyPosition();

                foreach (Bullet bullet in bullets) { bullet.Moving(); }

                //немного хитрая проверка попадания, надо будет проверить, не ли варианта лучше
                bool tmpIsHit = false;
                bool tmpIsAll = false;
                while (!tmpIsAll)
                {
                    foreach (Bullet bullet in bullets)
                    {
                        foreach (Enemy enemy in enemyList)
                        {
                            if (!enemy.GetIsMoving() && IsHit(bullet.GetPos(), (GlobalVariables.GetCenterEnemyPosition() + enemy.GetCenterOffset()), false, bullet.IsPlayerOwner()))
                            {
                                player.AddToScore(enemy.GetCost());
                                enemyList.Remove(enemy);
                                bullets.Remove(bullet);
                                blastList.Add(new Blast(GlobalVariables.GetCenterEnemyPosition() + enemy.GetCenterOffset(), false));
                                tmpIsHit = true;
                                break;
                            }
                            if (enemy.GetIsMoving() && IsHit(bullet.GetPos(), enemy.GetPos(), false, bullet.IsPlayerOwner()))
                            {
                                player.AddToScore(enemy.GetCost());
                                enemyList.Remove(enemy);
                                bullets.Remove(bullet);
                                blastList.Add(new Blast(enemy.GetPos(), false));
                                tmpIsHit = true;
                                break;
                            }
                        }
                        if (IsHit(bullet.GetPos(), player.GetPos(), true, bullet.IsPlayerOwner()))
                        {
                            blastList.Add(new Blast(player.GetPos(), true));//не менять положение
                            player.Reset();
                             if (player.GetLifeNum() <= 0) state = 4;
                            bullets.Remove(bullet);
                            tmpIsHit = true;
                        }
                        if (tmpIsHit) break;
                    }
                    if (tmpIsHit) tmpIsHit = false;
                    else tmpIsAll = true;
                }

                tmpIsAll = false;
                player.CanShoot = true;
                while (!tmpIsAll)
                {
                    foreach (Bullet bullet in bullets)
                    {
                        if (bullet.GetPos().Y > 11 || bullet.GetPos().Y < -11)
                        {
                            bullets.Remove(bullet);
                            break;
                        }
                        if (bullet.GetVel().Y > 0) player.CanShoot = false;
                    }
                    tmpIsAll = true;
                }

                //      !не перемещать ниже !
                foreach (Enemy enemy in enemyList)
                {
                    bool isEqualSubFormation = false;
                    if (!enemy.GetIsMoving()) GlobalVariables.isAllMoving = false;//      !!!

                    if (enemy.GetIsMoving() && enemy.GetSubFormation() != 0)
                    {
                        foreach (Enemy enemyBuf in subFormations)
                        {
                            if (enemyBuf.GetSubFormation() == enemy.GetSubFormation())
                            {
                                isEqualSubFormation = true;
                                if (enemy.GetCost() > enemyBuf.GetCost())
                                {
                                    subFormations.Remove(enemyBuf);
                                    subFormations.Add(enemy);
                                    break;
                                }
                            }
                        }
                        if (!isEqualSubFormation)
                        {
                            subFormations.Add(enemy);
                            isEqualSubFormation = false;
                        }
                    }
                }
                foreach (Enemy enemy in enemyList)
                {
                    if (enemy.GetSubFormation() != 0)
                    {
                        foreach (Enemy enemyBuf in subFormations)
                        {
                            if (enemyBuf.GetSubFormation() == enemy.GetSubFormation())
                                enemy.SetVel(enemyBuf.GetVel());
                        }
                    }
                }

                float minCentralOffset = float.MaxValue;
                float maxCentralOffset = float.MinValue;
                foreach (Enemy enemy in enemyList)
                {
                    if (enemy.GetCenterOffset().X > maxCentralOffset) maxCentralOffset = enemy.GetCenterOffset().X;
                    if (enemy.GetCenterOffset().X < minCentralOffset) minCentralOffset = enemy.GetCenterOffset().X;

                    if (enemy.GetIsMoving()) enemy.Moving(player.GetPos());

                    int startChance;
                    if (enemyList.Count > 8) startChance = 996;
                    else startChance = 993;
                    if (randomizer.Next(0, 1000) > startChance && !enemy.GetIsMoving() && CanMove(enemy)) enemy.StartMove();//(player.GetPos());
                    if (enemy.GetIsMoving() && randomizer.Next(0, 1000) > 990) bullets.Add(enemy.Shoot());

                    if (IsCollide(enemy.GetPos(), player.GetPos()))
                    {
                        player.AddToScore(enemy.GetCost());
                        blastList.Add(new Blast(player.GetPos(), true));//не перемещать
                        player.Reset();
                        if (player.GetLifeNum() <= 0) state = 4;
                        enemyList.Remove(enemy);
                        blastList.Add(new Blast(enemy.GetPos(), false));
                        break;
                    }
                }
                GlobalVariables.ComputeMinMaxCenterX(minCentralOffset, maxCentralOffset);

                foreach (Star star in starList)
                {
                    star.Moving();
                }
                score.PrepareToRender(player.GetScore().ToString());
            }
            if (state == 6) Exit();
        }
        protected override void OnRenderFrame(FrameEventArgs E)
        {
            base.OnRenderFrame(E);

            //            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

            if (state == 1)
            {
                GL.PushMatrix();

                GL.Translate(0, 0.5, 0);
                generalText.PrepareToRender("GALAGA");
                generalText.Render();
                GL.Translate(0, -0.2, 0);
                generalText.PrepareToRender("Start game");
                generalText.Render();
                if (menuChoice == 1)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }
                GL.Translate(0, -0.1, 0);
                generalText.PrepareToRender("Settings");
                generalText.Render();
                if (menuChoice == 2)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }
                GL.Translate(0, -0.1, 0);
                generalText.PrepareToRender("Exit");
                generalText.Render();
                if (menuChoice == 3)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }

                GL.PopMatrix();
            }
            if (state == 3 || state == 5 || state == 4)
            {
                RenderBackground();
                RenderBackground();

                GL.PushMatrix();
                GL.Translate(-1 + scoreLabel.Size.X / GlobalVariables.GetWindowSize().X, 1 - scoreLabel.Size.Y / GlobalVariables.GetWindowSize().Y, 0);
                scoreLabel.Render();
                GL.Translate(scoreLabel.Size.X / GlobalVariables.GetWindowSize().X + score.Size.X / GlobalVariables.GetWindowSize().X, 0, 0);
                score.Render();
                GL.PopMatrix();

                player.RenderObject();

                foreach (Enemy enemy in enemyList) { enemy.RenderObject(); }
                foreach (Bullet bullet in bullets) { bullet.RenderObject(); }
                bool tmpIsAll = false;
                while (!tmpIsAll)
                {
                    foreach (Blast blast in blastList)
                    {
                        if (blast.RenderBlast()) blastList.Remove(blast);
                        break;
                    }
                    tmpIsAll = true;
                }
                player.RenderLifes();
            }
            if (state == 4)
            {
                GL.PushMatrix();

                isKeyPressed = 0;

                GL.Translate(0, 0.5, 0);
                generalText.PrepareToRender("GAME OVER");
                generalText.Render();
                GL.Translate(0, -0.2, 0);
                generalText.PrepareToRender("Start new game");
                generalText.Render();
                if (menuChoice == 1)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }
                GL.Translate(0, -0.1, 0);
                generalText.PrepareToRender("Exit to menu");
                generalText.Render();
                if (menuChoice == 2)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }
                GL.Translate(0, -0.1, 0);
                generalText.PrepareToRender("Exit");
                generalText.Render();
                if (menuChoice == 3)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }

                GL.PopMatrix();
            }
            if (state == 5)
            {
                GL.PushMatrix();

                isKeyPressed = 0;

                GL.Translate(0, 0.5, 0);
                generalText.PrepareToRender("PAUSE");
                generalText.Render();
                GL.Translate(0, -0.2, 0);
                generalText.PrepareToRender("Resume");
                generalText.Render();
                if (menuChoice == 1)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }
                GL.Translate(0, -0.1, 0);
                generalText.PrepareToRender("Exit to menu");
                generalText.Render();
                if (menuChoice == 2)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }
                GL.Translate(0, -0.1, 0);
                generalText.PrepareToRender("Exit");
                generalText.Render();
                if (menuChoice == 3)
                {
                    MenuFrameRender(generalText.Size.X / GlobalVariables.GetWindowSize().X, generalText.Size.Y / GlobalVariables.GetWindowSize().Y);
                }

                GL.PopMatrix();
            }
            if (state == 7)
            {
                GL.PushMatrix();

                isKeyPressed = 0;

                GL.Translate(0, 0.5, 0);
                generalText.PrepareToRender("Settings");
                generalText.Render();
                GL.Translate(0, -0.3, 0);
                generalText.PrepareToRender("Work in progress.");
                generalText.Render();
                GL.Translate(0, -0.1, 0);
                generalText.PrepareToRender("Please press Escape to back to menu.");
                generalText.Render();
                GL.PopMatrix();
            }

            SwapBuffers();
        }
        private void RenderBackground()
        {
            backgroundTexture.Bind();
            GL.Color4(Color4.White);
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
            foreach (Star star in starList)
            {
                star.RenderStar();
            }
        }
        private bool IsHit(Vector2 bulletPos, Vector2 shipPos, bool isPlayer, bool bulletOwner)
        {
            if (isPlayer == bulletOwner) return false;
            if (((bulletPos.X + 0.55F) > shipPos.X) && ((bulletPos.X + 0.45F) < (shipPos.X + 1)) &&
                ((bulletPos.Y + 0.7F) > shipPos.Y) && ((bulletPos.Y + 0.3F) < (shipPos.Y + 1)))
                return true;

            return false;
        }
        private bool IsCollide(Vector2 playerPos, Vector2 enemyPos)
        {
            if (((playerPos.X + 0.95F) > enemyPos.X + 0.05) && ((playerPos.X + 0.05F) < (enemyPos.X + 0.95)) &&
                ((playerPos.Y + 0.95F) > enemyPos.Y + 0.05) && ((playerPos.Y + 0.05F) < (enemyPos.Y + 0.95)))
                return true;
            return false;
        }
        private bool CanMove(Enemy enemy)//поправить с учётом различной стоимости противников
        {
            if (enemy.GetCost() < (enemy.GetCostPeLvl() * 3))
            {
                if (enemy.GetCenterOffset().X >= 0)
                {
                    foreach (Enemy enemyBuf in enemyList)
                    {
                        if (enemyBuf.GetCenterOffset().X > enemy.GetCenterOffset().X && !enemyBuf.GetIsMoving() && enemyBuf.GetCost() < (enemy.GetCostPeLvl() * 3)) return false;
                        if (enemyBuf.GetCenterOffset().Y > enemy.GetCenterOffset().Y && enemyBuf.GetCenterOffset().X == enemy.GetCenterOffset().X && !enemyBuf.GetIsMoving() && enemyBuf.GetCost() < (enemy.GetCostPeLvl() * 3)) return false;
                    }
                    return true;
                }
                else
                {
                    foreach (Enemy enemyBuf in enemyList)
                    {
                        if (enemyBuf.GetCenterOffset().X < enemy.GetCenterOffset().X && !enemyBuf.GetIsMoving() && enemyBuf.GetCost() < (enemy.GetCostPeLvl() * 3)) return false;
                        if (enemyBuf.GetCenterOffset().Y > enemy.GetCenterOffset().Y && enemyBuf.GetCenterOffset().X == enemy.GetCenterOffset().X && !enemyBuf.GetIsMoving() && enemyBuf.GetCost() < (enemy.GetCostPeLvl() * 3)) return false;
                    }
                    return true;
                }
            }
            else if (enemy.GetCost() == (enemy.GetCostPeLvl() * 3))
            {
                //проврить, если сверху и по диагоналям 4-го уровня
                foreach (Enemy enemyBuf in enemyList)
                {
                    if (!enemyBuf.GetIsMoving() && enemyBuf.GetCenterOffset().Y > enemy.GetCenterOffset().Y && 
                        enemyBuf.GetCenterOffset().X >= (enemy.GetCenterOffset().X - 1) && 
                        enemyBuf.GetCenterOffset().X <= (enemy.GetCenterOffset().X + 1))
                        return false;
                    if (enemy.GetCenterOffset().X >= 0)
                    { if (enemyBuf.GetCenterOffset().X > enemy.GetCenterOffset().X && !enemyBuf.GetIsMoving() && enemyBuf.GetCenterOffset().Y == enemy.GetCenterOffset().Y) return false; }
                    else if (enemyBuf.GetCenterOffset().X < enemy.GetCenterOffset().X && !enemyBuf.GetIsMoving() && enemyBuf.GetCenterOffset().Y == enemy.GetCenterOffset().Y) return false;
                }
                return true;
            }
            else if (enemy.GetCost() == (enemy.GetCostPeLvl() * 4))
            {
                //проверить, возможно ли потянуть за собой 2-х 3-го уровня
                int num = 0;
                int tmpSubFormation = GlobalVariables.GetSubFormation();
                foreach (Enemy enemyBuf in enemyList)
                {
                    if (enemy.GetCenterOffset().X >= 0)
                    { if (enemyBuf.GetCenterOffset().X > enemy.GetCenterOffset().X && !enemyBuf.GetIsMoving() && enemyBuf.GetCenterOffset().Y == enemy.GetCenterOffset().Y) return false; }
                    else if (enemyBuf.GetCenterOffset().X < enemy.GetCenterOffset().X && !enemyBuf.GetIsMoving() && enemyBuf.GetCenterOffset().Y == enemy.GetCenterOffset().Y) return false;
                }

                foreach (Enemy enemyBuf in enemyList)
                {
                    if (!enemyBuf.GetIsMoving() && enemyBuf.GetCenterOffset().Y == enemy.GetCenterOffset().Y - 1 &&
                        enemyBuf.GetCenterOffset().X >= (enemy.GetCenterOffset().X - 1) &&
                        enemyBuf.GetCenterOffset().X <= (enemy.GetCenterOffset().X + 1))
                    {
                        if (enemy.GetCenterOffset().X < 0) enemyBuf.StartMove(-tmpSubFormation);
                        else enemyBuf.StartMove(tmpSubFormation);
                        num++;
                    }
                    if (num == 2) break;
                }
                //если дошло до этого момента, то значит предыдущие остальные проверки успешно пройдены
                if (enemy.GetCenterOffset().X < 0) enemy.StartMove(-tmpSubFormation);
                else enemy.StartMove(tmpSubFormation);
                return false;
            }
            else return true;//true;
        }
        private void LevelLoad()
        {
            enemyList.Clear();
            bullets.Clear();
            subFormations.Clear();
            blastList.Clear();
            starList.Clear();
            isKeyPressed = 0;
            GlobalVariables.ResetCenterEnemyPosition();

            randomizer = new Random();

            scoreLabel = new TextRenderer();
            score = new TextRenderer();

            for (int i = 0; i < 30; i++)
            {
                tmpEnemyOffset.X = (float)(randomizer.NextDouble() - 0.5) * 25;
                tmpEnemyOffset.Y = (float)(randomizer.NextDouble() - 0.5) * 11;
                Star tmpStar = new Star((float)randomizer.NextDouble() / 3, tmpEnemyOffset, randomizer.Next(0, 99));
                starList.Add(tmpStar);
            }

            player = new Player();

            tmpEnemyOffset.X = -4;
            tmpEnemyOffset.Y = 2;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    enemyList.Add(new Enemy(1, tmpEnemyOffset, (i % Enemy.maxStage) * Enemy.frameToStage));
                    tmpEnemyOffset.Y++;
                }
                tmpEnemyOffset.X++;
                tmpEnemyOffset.Y = 2;
            }
            tmpEnemyOffset.X = -4;
            tmpEnemyOffset.Y = 5;
            for (int i = 0; i < 8; i++)
            {
                enemyList.Add(new Enemy(2, tmpEnemyOffset, (i % Enemy.maxStage) * Enemy.frameToStage));
                tmpEnemyOffset.X++;
            }
            tmpEnemyOffset.X = -3;
            tmpEnemyOffset.Y = 6;
            for (int i = 0; i < 6; i++)
            {
                enemyList.Add(new Enemy(3, tmpEnemyOffset, (i % Enemy.maxStage) * Enemy.frameToStage));
                tmpEnemyOffset.X++;
            }
            tmpEnemyOffset.X = -2;
            tmpEnemyOffset.Y = 7;
            for (int i = 0; i < 2; i++)
            {
                enemyList.Add(new Enemy(4, tmpEnemyOffset, (i % Enemy.maxStage) * Enemy.frameToStage));
                tmpEnemyOffset.X += 3;
            }

            scoreLabel.PrepareToRender("Score");
            score.PrepareToRender("0");
        }
        private void MenuFrameRender(float x, float y)
        {
            menuFrame.Bind();
            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-x, -y);
            GL.TexCoord2(1, 0);
            GL.Vertex2(x, -y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(x, y);
            GL.TexCoord2(0, 1);
            GL.Vertex2(-x, y);
            GL.End();
        }
    }
}
