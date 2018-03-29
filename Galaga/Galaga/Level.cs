using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Galaga
{
    //хранит все объекты игры и обрабатывает их взаимодействие
    public static class Level
    {
        private static List<Player> _players;
        private static List<Enemy> _enemies;
//        private static List<Bullet> _bullets;
//        static Random _randomizer;
        private static int _playersCount = 1;//временно

        static Level()
        {
            _players = new List<Player>();
            _enemies = new List<Enemy>();

//            _background = new Background();
//            _randomizer = new Random();

//            _fomations = //хммм   new Fomations();
//            _bullets = new List<Bullet>();
        }

        public static void PlayerAction(PlayerAction newAction)
        {
            switch (newAction)
            {
                case Galaga.PlayerAction.MoveLeft:
                {
                    break;
                }
                case Galaga.PlayerAction.MoveRight:
                {
                    break;
                }
                case Galaga.PlayerAction.Shoot:
                {
                    break;
                }
                case Galaga.PlayerAction.Stop:
                {
                    break;
                }
            }
        }

        public static void Render() // переработать и вынести отрисовку в отдельный класс
        {
            //throw new NotImplementedException();
/*          
            if (KeyboardInput.CurrentGameState == gameState.Game || KeyboardInput.CurrentGameState == gameState.GameOver || KeyboardInput.CurrentGameState == gameState.Pause)
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

                foreach (Enemy enemy in _enemies) { enemy.RenderObject(); }
                foreach (Bullet bullet in _bullets) { bullet.RenderObject(); }
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
            
            */
        }

        private static void Load()
        {
            _enemies.Clear();
            _players.Clear();

//            _fomations.Clear();
//            _bullets.Clear();

            for (int i = 0; i < _playersCount; i++)
                _players.Add(new Player());

            for (int i=0;i<10;i++)
                _enemies.Add(new Enemy(0,new Vector2(i-5,3)));//ооочень тестовый вариант
        }

        public static void Update()   //переписать вообще целиком
        {
//            KeyboardInput.MenuChanger();
            if (GameStates.GameState == GameState.LevelLoad)
            {
                Load();
                //KeyboardInput.CurrentGameState = gameState.Game;
                GameStates.LevelStateChanger();
                return;
            }
            if (GameStates.GameState == GameState.Game)
            {
//                WindowProperty.isAllMoving = true;

                //нафиг отсюда обработку входящих действий игрока, тут только обновление его состояний
                foreach (Player player in _players)
                {
                    player.Moving();
                }

                foreach (Enemy enemy in _enemies)
                {
//                    enemy.Moving();
                }

                //переделать (и вообще избавиться от GlobalVariables)
//                GlobalVariables.MoveCenterEnemyPosition();

//                foreach (Bullet bullet in _bullets) { bullet.Moving(); }

                //немного хитрая проверка попадания, надо будет проверить, не ли варианта лучше
                bool tmpIsHit = false;
                bool tmpIsAll = false;
/*                while (!tmpIsAll)
                {
                    foreach (Bullet bullet in _bullets)
                    {
                        foreach (Enemy enemy in _enemies)
                        {
                            if (!enemy.GetIsMoving() && IsHit(bullet.GetPos(), (GlobalVariables.GetCenterEnemyPosition() + enemy.GetCenterOffset()), false, bullet.IsPlayerOwner()))
                            {
                                player.AddToScore(enemy.GetCost());
                                _enemies.Remove(enemy);
                                _bullets.Remove(bullet);
                                blastList.Add(new Blast(GlobalVariables.GetCenterEnemyPosition() + enemy.GetCenterOffset(), false));
                                tmpIsHit = true;
                                break;
                            }
                            if (enemy.GetIsMoving() && IsHit(bullet.GetPos(), enemy.GetPos(), false, bullet.IsPlayerOwner()))
                            {
                                player.AddToScore(enemy.GetCost());
                                _enemies.Remove(enemy);
                                _bullets.Remove(bullet);
                                blastList.Add(new Blast(enemy.GetPos(), false));
                                tmpIsHit = true;
                                break;
                            }
                        }
                        if (IsHit(bullet.GetPos(), player.GetPos(), true, bullet.IsPlayerOwner()))
                        {
                            blastList.Add(new Blast(player.GetPos(), true));//не менять положение
                            player.Reset();
                            if (player.GetLifeNum() <= 0) KeyboardInput.CurrentGameState = gameState.GameOver;
                            _bullets.Remove(bullet);
                            tmpIsHit = true;
                        }
                        if (tmpIsHit) break;
                    }
                    if (tmpIsHit) tmpIsHit = false;
                    else tmpIsAll = true;
                }*/

/*                tmpIsAll = false;
                player.CanShoot = true;
                while (!tmpIsAll)
                {
                    foreach (Bullet bullet in _bullets)
                    {
                        if (bullet.GetPos().Y > 11 || bullet.GetPos().Y < -11)
                        {
                            _bullets.Remove(bullet);
                            break;
                        }
                        if (bullet.GetVel().Y > 0) player.CanShoot = false;
                    }
                    tmpIsAll = true;
                }*/

                //      !не перемещать ниже !
/*                foreach (Enemy enemy in _enemies)
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
                }*/
/*                foreach (Enemy enemy in _enemies)
                {
                    if (enemy.GetSubFormation() != 0)
                    {
                        foreach (Enemy enemyBuf in subFormations)
                        {
                            if (enemyBuf.GetSubFormation() == enemy.GetSubFormation())
                                enemy.SetVel(enemyBuf.GetVel());
                        }
                    }
                }*/

/*                float minCentralOffset = float.MaxValue;
                float maxCentralOffset = float.MinValue;
                foreach (Enemy enemy in _enemies)
                {
                    if (enemy.GetCenterOffset().X > maxCentralOffset) maxCentralOffset = enemy.GetCenterOffset().X;
                    if (enemy.GetCenterOffset().X < minCentralOffset) minCentralOffset = enemy.GetCenterOffset().X;

                    if (enemy.GetIsMoving()) enemy.Moving(player.GetPos());

                    int startChance;
                    if (_enemies.Count > 8) startChance = 996;
                    else startChance = 993;
                    if (randomizer.Next(0, 1000) > startChance && !enemy.GetIsMoving() && CanMove(enemy)) enemy.StartMove();//(player.GetPos());
                    if (enemy.GetIsMoving() && randomizer.Next(0, 1000) > 990) _bullets.Add(enemy.Shoot());

                    if (IsCollide(enemy.GetPos(), player.GetPos()))
                    {
                        player.AddToScore(enemy.GetCost());
                        blastList.Add(new Blast(player.GetPos(), true));//не перемещать
                        player.Reset();
                        if (player.GetLifeNum() <= 0) KeyboardInput.CurrentGameState = gameState.GameOver;
                        _enemies.Remove(enemy);
                        blastList.Add(new Blast(enemy.GetPos(), false));
                        break;
                    }
                }
                GlobalVariables.ComputeMinMaxCenterX(minCentralOffset, maxCentralOffset);

                foreach (Star star in starList)
                {
                    star.Moving();
                }*/
//                score.PrepareToRender(player.GetScore().ToString());
            }
            if (GameStates.GameState == GameState.Exit)
            {
//                WindowProperty.ShootFlagNeg();
            }
        }

        public static List<Enemy> Enemies => _enemies;

        public static List<Player> Players => _players;

//        public static List<Bullet> Bullets => _bullets;
    }
}
