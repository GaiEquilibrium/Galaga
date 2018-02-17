using OpenTK;

namespace Galaga
{
    static class GlobalVariables
    {
        private static Vector2 windowSize;
        private static int objectSize;
        private static Vector2 glObjectSize;
        private static Vector2 centerEnemyPosition;
        private static float centerMove;
        private static Vector2 minMaxCenterX;
        private static Vector2 formationSize;//для генерации
        public static bool isAllMoving;
        private static int subFormation;
        private static int shootFlag;

        static GlobalVariables()
        {
            windowSize.X = 640;
//            windowSize.Y = 640;
            windowSize.Y = 900;

            minMaxCenterX.X = float.MinValue;
            minMaxCenterX.Y = float.MaxValue;

            objectSize = 32;
            formationSize.Y = 6;
            formationSize.X = (windowSize.X / 2 - windowSize.X / 10) / objectSize;

            glObjectSize.X = (objectSize * 2) / windowSize.X;
            glObjectSize.Y = (objectSize * 2) / windowSize.Y;

            ResetCenterEnemyPosition();
            centerMove = 0.05F;
            subFormation = 0;

            shootFlag = 0;
        }
        static public Vector2 GetWindowSize() { return windowSize; }
        static public Vector2 GetGlObjectSize() { return glObjectSize; }
        static public int GetObjectSize() { return objectSize; }
        static public Vector2 GetCenterEnemyPosition() { return centerEnemyPosition; }
        static public void ComputeMinMaxCenterX(float minCenterOffset, float maxCenterOffset)
        {
            maxCenterOffset++;
            minMaxCenterX.X = (-windowSize.X / (objectSize * 2)) - minCenterOffset; //min
            minMaxCenterX.Y = (windowSize.X / (objectSize * 2)) - maxCenterOffset; //max
        }
        static public void MoveCenterEnemyPosition()
        {
            if (centerEnemyPosition.X > minMaxCenterX.Y) centerMove = -0.05F;
            if (centerEnemyPosition.X < minMaxCenterX.X) centerMove = 0.05F;

            centerEnemyPosition.X += centerMove;
        }
        static public int GetSubFormation()
        {
            subFormation++;
            if (subFormation == int.MaxValue) subFormation = 1;
            return subFormation;
        }
        static public void ResetCenterEnemyPosition()
        {
            centerEnemyPosition.X = 0;
            centerEnemyPosition.Y = 3;
        }
        static public int ShootFlag
        {
            get
            {
                return shootFlag;
            }
        }
        static public void ShootFlagInc()
        {
            shootFlag++;
        }
        static public void ShootFlagDec()
        {
            shootFlag--;
        }
        static public void ShootFlagNeg()
        {
            shootFlag = -1;
        }
    }
}
