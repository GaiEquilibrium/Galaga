using System;

using OpenTK;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Galaga
{
    public class Texture : IDisposable
    {
        public int GlHandle { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        #region NPOT
        private static bool? _calculatedSupportForNpot;
        public static bool NpotIsSupported
        {
            get
            {
                if (!_calculatedSupportForNpot.HasValue)
                {
                    _calculatedSupportForNpot = false;
                    int extensionsCount;
                    GL.GetInteger(GetPName.NumExtensions, out extensionsCount);
                    for (var i = 0; i < extensionsCount; i++)
                    {
                        if ("GL_ARB_texture_non_power_of_two" == GL.GetString(StringName.Extensions, i))
                        {
                            _calculatedSupportForNpot = true;
                            break;
                        }
                    }
                }
                return _calculatedSupportForNpot.Value;
            }
        }
        public int PotWidth
        {
            get
            {
                return NpotIsSupported ? Width : (int)Math.Pow(2, Math.Ceiling(Math.Log(Width, 2)));
            }
        }
        public int PotHeight
        {
            get
            {
                return NpotIsSupported ? Height : (int)Math.Pow(2, Math.Ceiling(Math.Log(Height, 2)));
            }
        }
        #endregion

        public Texture(Bitmap bitmap)
        {
            GlHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, GlHandle);

            Width = bitmap.Width;
            Height = bitmap.Height;

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, PotWidth, PotHeight, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bitmapData.Width, bitmapData.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }
        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, GlHandle);
        }

        #region Disposable
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    GL.DeleteTexture(GlHandle);
                }
                _disposed = true;
            }
        }
        ~Texture()
        {
            Dispose(false);
        }
        #endregion
    }
}