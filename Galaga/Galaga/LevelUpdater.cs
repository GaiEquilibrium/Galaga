using System.Collections.Generic;

//bug - при попадании по противнику, строй на короткое время прерывает движение

namespace Galaga
{
    static class LevelUpdater
    {
        private static bool _completeCheck = false; //проверка завершённостей циклов проверки
        private static bool _hitCheck = false;
        public static void Update()
        {
            //нафиг отсюда обработку входящих действий игрока, тут только обновление его состояний

            foreach (var enemy in Level.Enemies)
            {
                enemy.Value.Update();
            }
            foreach (KeyValuePair<int, Player> player in Level.Players) { player.Value.Update(); }

            while (!_completeCheck)
            {
                _completeCheck = true;
                foreach (Bullet bullet in Level.Bullets)
                {
                    if (bullet.IsComplete)
                    {
                        Level.Bullets.Remove(bullet);
                        _completeCheck = false;
                        break;
                    }
                    _completeCheck = true;
                }
            }
            foreach (Bullet bullet in Level.Bullets) { bullet.Update(); }
//            Level.MainFormation.Update();


            //стрельба игрока
            foreach (KeyValuePair<int, Player> player in Level.Players)
            {
                Bullet shoot = null;
                if (player.Value.WantShoot) shoot = player.Value.Shoot();
                if (shoot != null) Level.Bullets.Add(shoot);
            }

            //проверка попаданий
            _completeCheck = false;
            while (!_completeCheck)
            {
                _completeCheck = false;
                foreach (Bullet bullet in Level.Bullets)
                {
                    foreach (var enemy in Level.Enemies)
                    {
                        if (enemy.Value.IsHit(bullet))
                        {
                            Level.Players[bullet.PlayerId].AddToScore(enemy.Value.Cost);
                            Level.MainFormation.Remove(enemy.Value);
                            Level.Enemies.Remove(enemy.Key);
                            bullet.IsComplete = true;
                            Level.Bullets.Remove(bullet);
                            //                            blastList.Add(new Blast(GlobalVariables.GetCenterEnemyPosition() + enemy.GetCenterOffset(), false));
                            _hitCheck = true;
                            break;
                        }
                    }
                    if (_hitCheck) break;
                    foreach (KeyValuePair<int, Player> player in Level.Players)
                    {
                        if (player.Value.IsHit(bullet))
                        {
//                            blastList.Add(new Blast(player.GetPos(), true)); //не менять положение
                            player.Value.Reset();
                            if (player.Value.LifeNum <= 0)
                            {
                                GameStates.LevelStateChanger();//для одиночного варианта так
                                Level.Players.Remove(player.Key);
                            }
                            bullet.IsComplete = true;
                            Level.Bullets.Remove(bullet);
                            _hitCheck = true;
                        }
                    }
                    if (_hitCheck) break;
                }
                if (_hitCheck) _hitCheck = false;
                else _completeCheck = true;
            }


            //                WindowProperty.isAllMoving = true;


            //переделать (и вообще избавиться от GlobalVariables)
            //                GlobalVariables.MoveCenterEnemyPosition();


            //немного хитрая проверка попадания, надо будет проверить, не ли варианта лучше
            //                

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
    }
}
