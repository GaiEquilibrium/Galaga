using OpenTK;

namespace Galaga
{
    //отвечает за корректное отображение взрыва
    class Blast:Moved
    {
        private static readonly int EnemyMaxState = 5;
        private static readonly int PlayerMaxState = 4;

        public bool IsComplete { get; private set; }

        public Blast(Vector2 newPositiion, Belonging newBelonging)
        {
            Belonging = newBelonging;
            GameObject = GameObject.Blast;
            position = newPositiion;
            State = 0;
            frameToStage = 3;
            Counter = 0;
            IsComplete = false;
            if (Belonging == Belonging.None)
            {
                IsComplete = true;
            }
        }

        public new void Update()
        {
            //throw new System.NotImplementedException(); //???

            if (Counter >= frameToStage)
            {
                State++;
                Counter = 0;
            }
            if (Belonging == Belonging.Enemy && State == EnemyMaxState || Belonging == Belonging.Player && State == PlayerMaxState)
            {
                IsComplete = true;
            }
        }
    }
}
