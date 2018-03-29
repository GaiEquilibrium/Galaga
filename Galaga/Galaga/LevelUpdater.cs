using System.Collections.Generic;

namespace Galaga
{
    static class LevelUpdater
    {
        public static void Update()
        {
            //нафиг отсюда обработку входящих действий игрока, тут только обновление его состояний

//            foreach (Enemy enemy in _enemies)
//            {
//                enemy.Moving();
//            }
            foreach (KeyValuePair<int, Player> player in Level.Players)
            {
                player.Value.Update();
            }

            //                WindowProperty.isAllMoving = true;


            //переделать (и вообще избавиться от GlobalVariables)
            //                GlobalVariables.MoveCenterEnemyPosition();

            //                foreach (Bullet bullet in _bullets) { bullet.Moving(); }

            //немного хитрая проверка попадания, надо будет проверить, не ли варианта лучше
            //                bool tmpIsHit = false;
            //                bool tmpIsAll = false;
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
    }
}
