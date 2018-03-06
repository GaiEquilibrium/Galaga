//Used OpenTK example, as a base
//https://github.com/mono/opentk/blob/master/Source/Examples/OpenGL/1.x/TextRendering.cs
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Galaga
{
    //овечает за всю работу с текстом
    class TextString
    {
        //TODO
        //проверить, всё ли тут нормально с памятью
        //надо попробовать переписать для того что бы сделать всё проще и понятнее
        private Bitmap _bmp;
        private Graphics _gfx;
        private int _texture;
        private Rectangle _rectGfx;
        private bool _disposed;
        private Font _font = new Font(FontFamily.GenericSansSerif, 24);
        private int _width, _height;
        private int _needWidth = 300, _needHeight = 100;
        private string _text;

        public TextString()
        {
            _height = _needHeight;
            _width = _needWidth;

            if (_width <= 0)
                throw new ArgumentOutOfRangeException($"width");
            if (_height <= 0)
                throw new ArgumentOutOfRangeException($"height");
            if (GraphicsContext.CurrentContext == null)
                throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");

            _bmp = new Bitmap(_width, _height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            _gfx = Graphics.FromImage(_bmp);
            _gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            _texture = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        }
        public void SetNewSize(string newText) { _text = newText; SetNewSize(); }
        public void SetNewSize()
        {
            if (string.IsNullOrEmpty(_text)) return;
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    var measures = graphics.MeasureString(_text, _font);
                    _needWidth = (int)Math.Ceiling(measures.Width);
                    _needHeight = (int)Math.Ceiling(measures.Height);
                }
            }
        }
        public void DrawString(Brush brush, PointF point)
        {
            if (_text == "" || _text == null) return;

            _gfx.DrawString(_text, _font, brush, point);
        }
        public void Clear(Color color)
        {
            _gfx.Clear(color);
            _rectGfx = new Rectangle(0, 0, _bmp.Width, _bmp.Height);
        }
        public int Texture
        {
            get
            {
                UploadBitmap();
                return _texture;
            }
        }
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(_width, _height);
            }
        }
        void UploadBitmap()
        {
            if (_rectGfx != RectangleF.Empty)
            {
                System.Drawing.Imaging.BitmapData data = _bmp.LockBits(_rectGfx, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.BindTexture(TextureTarget.Texture2D, _texture);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, _rectGfx.X, _rectGfx.Y, _rectGfx.Width, _rectGfx.Height, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                _bmp.UnlockBits(data);
                _rectGfx = Rectangle.Empty;
            }
        }
        void Dispose(bool manual)
        {
            if (!_disposed)
            {
                if (manual)
                {
                    _bmp.Dispose();
                    _gfx.Dispose();
                    if (GraphicsContext.CurrentContext != null) GL.DeleteTexture(_texture);
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void CheckReset()
        {
            SetNewSize();
            if (_bmp.Width != _needWidth || _bmp.Height != _needHeight)
            {
                _disposed = false;
                Dispose(true);

                //repiat constructor
                _height = _needHeight;
                _width = _needWidth;

                if (_width <= 0)
                    throw new ArgumentOutOfRangeException($"width");
                if (_height <= 0)
                    throw new ArgumentOutOfRangeException($"height");
                if (GraphicsContext.CurrentContext == null)
                    throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");

                _bmp = new Bitmap(_width, _height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                _gfx = Graphics.FromImage(_bmp);
                _gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                _texture = GL.GenTexture();

                GL.BindTexture(TextureTarget.Texture2D, _texture);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            }
        }
        public void PrepareToRender(string newText)
        {
            PointF position = PointF.Empty;

            _text = newText;
            CheckReset();
            Clear(Color.Black);
            DrawString(Brushes.White, position);
        }
        public void Render()
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.PushMatrix();
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-(Size.X / GlobalVariables.GetWindowSize().X), -(Size.Y / GlobalVariables.GetWindowSize().Y));
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2((Size.X / GlobalVariables.GetWindowSize().X), -(Size.Y / GlobalVariables.GetWindowSize().Y));
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2((Size.X / GlobalVariables.GetWindowSize().X), (Size.Y / GlobalVariables.GetWindowSize().Y));
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-(Size.X / GlobalVariables.GetWindowSize().X), (Size.Y / GlobalVariables.GetWindowSize().Y));
            GL.End();
            GL.PopMatrix();
        }
        ~TextString()
        {
            Console.WriteLine("[Warning] Resource leaked: {0}.", typeof(TextString));
        }
    }
}
