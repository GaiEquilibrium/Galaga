using OpenTK;

namespace Galaga
{
    public enum PlayerAction { MoveLeft, MoveRight, Shoot, Stop }

    public class Player : Ship
    {
        public int LifeNum { get; private set; }
        public int Score { get; private set; }
        public int Shoots;
        public bool WantShoot { get; private set; }
        public int PlayerId { get; }

        public Player()
        {
            PlayerId = Level.PlayerId;
            position.X = -0.5F;
            position.Y = -7;
            LifeNum = 3;
            Score = 0;
            GameObject = GameObject.Player;
            Belonging = Belonging.Player;
            Shoots = 2;
            WantShoot = false;
        }
        public void ReduceLifeNum() { LifeNum--; }
        public void AddToScore(int addedScore) { Score += addedScore; }
        public void Action(PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.MoveLeft:
                {
                    velocity.X = -0.15F;
                    break;
                }
                case PlayerAction.MoveRight:
                {
                    velocity.X = 0.15F;
                    break;
                }
                case PlayerAction.Shoot:
                {
                    WantShoot = true;
                    break;
                }
                case PlayerAction.Stop:
                {
                    velocity.X = 0;
                    break;
                }
            }
        }

        public Bullet Shoot()
        {
//            SoundMaster.Shoot();
            WantShoot = false;
            if (Shoots <= 0) return null;
            Shoots--;
            Vector2 tmpPos = Position;
            tmpPos.Y++;
            return new Bullet(tmpPos,Belonging.Player,PlayerId);
        }
        public void Reset()
        {
            LifeNum--;
            position.X = 0F;
            if (LifeNum > 0) { position.Y = -7; }
            else { position.Y = 20; }
        }

        public void RenderLifes()//подумать над более удобным вариантом
        {
            Vector2 tmpPos;
            tmpPos.X = -9.5F;
            tmpPos.Y = -13.5F + PlayerId;
            for (int i = 1; i < LifeNum; i++)
            {
                Textures.Render(tmpPos,0,Belonging.Player,GameObject.Player);
                tmpPos.X++;
            }
        }
    }
}
