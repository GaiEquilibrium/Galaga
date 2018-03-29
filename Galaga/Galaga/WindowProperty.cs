using OpenTK;

namespace Galaga
{
    static class WindowProperty
    {
        private static Vector2 _windowSize;
        private static int _objectSize;
        private static Vector2 _glObjectSize;

        static WindowProperty()
        {
            _windowSize.X = 640;
            _windowSize.Y = 900;

            _objectSize = 64;

            _glObjectSize.X = _objectSize / _windowSize.X;
            _glObjectSize.Y = _objectSize / _windowSize.Y;
        }
        public static Vector2 WindowSize => _windowSize;
        public static Vector2 GlObjectSize => _glObjectSize;
        public static int ObjectSize => _objectSize;

        public static Vector2 PixelToGlSize(Vector2 size)
        {
            Vector2 glSize;
            glSize.X = size.X / _windowSize.X;
            glSize.Y = size.Y / _windowSize.Y;
            return glSize;
        }
    }
}
