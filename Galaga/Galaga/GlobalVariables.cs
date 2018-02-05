using OpenTK;

namespace Galaga
{
    static class GlobalVariables
    {
        private static Vector2 windowSize;
        private static int objectSize;
        private static Vector2 glObjectSize;

        static GlobalVariables()
        {
            windowSize.X = 700;
            windowSize.Y = 700;

            objectSize = 35;

            glObjectSize.X = (objectSize * 2) / windowSize.X;
            glObjectSize.Y = (objectSize * 2) / windowSize.Y;
        }
        static public Vector2 GetWindowSize() { return windowSize; }
        static public Vector2 GetGlObjectSize() { return glObjectSize; }
        static public int GetObjectSize() { return objectSize; }
    }
}
