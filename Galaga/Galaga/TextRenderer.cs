//Used OpenTK example, as a base
//https://github.com/mono/opentk/blob/master/Source/Examples/OpenGL/1.x/TextRendering.cs
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Galaga
{
    class TextRenderer
    {
        //TODO
        //проверить, всё ли тут нормально с памятью
        private Bitmap bmp;
        private Graphics gfx;
        private int texture;
        private Rectangle rectGFX;
        private bool disposed;
        private Font font = new Font(FontFamily.GenericSansSerif, 24);
        private int width, height;
        private int needWidth = 300, needHeight = 100;
        private string text;

        public TextRenderer()
        {
//            mainContext.MakeCurrent(mainWindow);

            height = needHeight;
            width = needWidth;

            if (width <= 0)
                throw new ArgumentOutOfRangeException("width");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height ");
            if (GraphicsContext.CurrentContext == null)
                throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");

            bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = Graphics.FromImage(bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            texture = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        }
        public void SetNewSize(string newText) { text = newText; SetNewSize(); }
        public void SetNewSize()
        {
            if (text == "" || text == null) return;
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics Graphics = Graphics.FromImage(bitmap))
                {
                    var Measures = Graphics.MeasureString(text, font);
                    needWidth = (int)Math.Ceiling(Measures.Width);
                    needHeight = (int)Math.Ceiling(Measures.Height);
                }
            }
        }
        public void DrawString(Brush brush, PointF point)
        {
            if (text == "" || text == null) return;

            gfx.DrawString(text, font, brush, point);
        }
        public void Clear(Color color)
        {
            gfx.Clear(color);
            rectGFX = new Rectangle(0, 0, bmp.Width, bmp.Height);
        }
        public int Texture
        {
            get
            {
                UploadBitmap();
                return texture;
            }
        }
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(width, height);
            }
        }
        void UploadBitmap()
        {
            if (rectGFX != RectangleF.Empty)
            {
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(rectGFX, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, rectGFX.X, rectGFX.Y, rectGFX.Width, rectGFX.Height, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);
                rectGFX = Rectangle.Empty;
            }
        }
        void Dispose(bool manual)
        {
            if (!disposed)
            {
                if (manual)
                {
                    bmp.Dispose();
                    gfx.Dispose();
                    if (GraphicsContext.CurrentContext != null) GL.DeleteTexture(texture);
                }
                disposed = true;
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
            if (bmp.Width != needWidth || bmp.Height != needHeight)
            {
                disposed = false;
                Dispose(true);

                //repiat constructor
                height = needHeight;
                width = needWidth;

                if (width <= 0)
                    throw new ArgumentOutOfRangeException("width");
                if (height <= 0)
                    throw new ArgumentOutOfRangeException("height ");
                if (GraphicsContext.CurrentContext == null)
                    throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");

                bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                gfx = Graphics.FromImage(bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                texture = GL.GenTexture();

                GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            }
        }
        public void PrepareToRender(string newText)
        {
            PointF position = PointF.Empty;

            text = newText;
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
        ~TextRenderer()
        {
            Console.WriteLine("[Warning] Resource leaked: {0}.", typeof(TextRenderer));
        }
    }
}
