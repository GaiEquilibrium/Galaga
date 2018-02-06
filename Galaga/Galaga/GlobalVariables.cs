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

        static GlobalVariables()
        {
            windowSize.X = 640;
            windowSize.Y = 640;

            objectSize = 32;

            glObjectSize.X = (objectSize * 2) / windowSize.X;
            glObjectSize.Y = (objectSize * 2) / windowSize.Y;

            centerEnemyPosition.X = 0;
            centerEnemyPosition.Y = 3;
            centerMove = 0.1F;
        }
        static public Vector2 GetWindowSize() { return windowSize; }
        static public Vector2 GetGlObjectSize() { return glObjectSize; }
        static public int GetObjectSize() { return objectSize; }
        static public Vector2 GetCenterEnemyPosition() { return centerEnemyPosition; }
        static public void MoveCenterEnemyPosition()
        {
            if (centerEnemyPosition.X > 5) centerMove = -0.1F;
            if (centerEnemyPosition.X < -5) centerMove = 0.1F;

            centerEnemyPosition.X += centerMove;
        }
    }
}
