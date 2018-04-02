using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Galaga
{

    //только для игровых объектов
    static class Textures
    {
        private static Dictionary<int, Texture> _enemies = new Dictionary<int, Texture>();
        private static Texture[] _boss = new Texture[4];
        private static Texture _player;
        private static Texture[] _star = new Texture[4];
        private static Texture[] _enemyBlast = new Texture[5];
        private static Texture[] _playerBlast = new Texture[4];
        private static Texture _bullet;

        static Textures()
        {//тут выбирается вариант по умолчанию
            _player = new Texture(new Bitmap("Texture/2/Player.png"));
            for (int i = 0; i < 4; i++)
            {
                _boss[i] = new Texture(new Bitmap("Texture/2/boss" + i+".png"));
            }

            //вроде бы так
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    _enemies.Add(i*2 + j, new Texture(new Bitmap("Texture/2/Enemy" + i + "" + j + ".png")));
                }
            }
            for (int i = 0; i < 5; i++)
            {
                _enemyBlast[i] = new Texture(new Bitmap("Texture/BlastE" + i + ".png"));
            }
            for (int i = 0; i < 4; i++)
            {
                _playerBlast[i] = new Texture(new Bitmap("Texture/BlastP" + i + ".png"));
            }

            for (int i = 0; i < 4; i++)
            {
                _star[i] = new Texture(new Bitmap("Texture/Star" + i + ".png"));
            }
            _bullet = new Texture(new Bitmap("Texture/Bullet.png"));
        }

        static void SetTextures(int textureSet)
        {
            _player = new Texture(new Bitmap("Texture/" + textureSet + "/Player.png"));
            for (int i = 0; i < 4; i++)
            {
                _boss[i] = new Texture(new Bitmap("Texture/" + textureSet + "/boss" + i + ".png"));
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    _enemies.Add(i*2 + j, new Texture(new Bitmap("Texture/" + textureSet + "/Enemy" + i + "" + j + ".png")));
                }
            }
            for (int i = 0; i < 5; i++)
            {
                _enemyBlast[i] = new Texture(new Bitmap("Texture/BlastE" + i + ".png"));
            }
            for (int i = 0; i < 4; i++)
            {
                _playerBlast[i] = new Texture(new Bitmap("Texture/BlastP" + i + ".png"));
            }

            for (int i = 0; i < 4; i++)
            {
                _star[i] = new Texture(new Bitmap("Texture/Star" + i + ".png"));
            }
            _bullet = new Texture(new Bitmap("Texture/Bullet.png"));
        }

        public static void Render(Moved renderedObject)
        {
            GL.Color4(Color4.White);
            GL.Translate(renderedObject.Position.X * WindowProperty.ObjectSize / WindowProperty.WindowSize.X, renderedObject.Position.Y * WindowProperty.ObjectSize / WindowProperty.WindowSize.Y, 0);
            switch (renderedObject.GameObject)
            {
                case GameObject.Player:
                {
                        _player.Bind();
                        break;
                }
                case GameObject.Blast:
                {
                    if (renderedObject.Belonging == Belonging.Enemy)
                    {
                            _enemyBlast[renderedObject.State].Bind();
                    }
                    else if (renderedObject.Belonging == Belonging.Player)
                    {
                            _enemyBlast[renderedObject.State].Bind();
                    }
                    break;
                }
                case GameObject.Boss:
                {
                    _boss[renderedObject.State].Bind();
                    break;
                }
                case GameObject.Bullet:
                {
                    _bullet.Bind();
                    break;
                }
                case GameObject.Enemy:
                {
                    _enemies[renderedObject.State].Bind();
                    break;
                }
                case GameObject.Star:
                {
                    _star[renderedObject.State].Bind();
                    break;
                }
            }
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(1, 1);
            GL.Vertex2(-WindowProperty.GlObjectSize.X / 2, -WindowProperty.GlObjectSize.Y / 2);
            GL.TexCoord2(0, 1);
            GL.Vertex2(WindowProperty.GlObjectSize.X / 2, -WindowProperty.GlObjectSize.Y / 2);
            GL.TexCoord2(0, 0);
            GL.Vertex2(WindowProperty.GlObjectSize.X / 2, WindowProperty.GlObjectSize.Y / 2);
            GL.TexCoord2(1, 0);
            GL.Vertex2(-WindowProperty.GlObjectSize.X / 2, WindowProperty.GlObjectSize.Y / 2);
            GL.End();

            GL.Translate(-renderedObject.Position.X * WindowProperty.ObjectSize / WindowProperty.WindowSize.X, -renderedObject.Position.Y * WindowProperty.ObjectSize / WindowProperty.WindowSize.Y, 0);

            renderedObject.Counter++;
        }
        public static void Render(Vector2 position, int state, Belonging belonging, GameObject gameObject)
        {
            GL.Color4(Color4.White);
            GL.Translate(position.X * WindowProperty.ObjectSize / WindowProperty.WindowSize.X, position.Y * WindowProperty.ObjectSize / WindowProperty.WindowSize.Y, 0);
            switch (gameObject)
            {
                case GameObject.Player:
                    {
                        _player.Bind();
                        break;
                    }
                case GameObject.Blast:
                    {
                        if (belonging == Belonging.Enemy)
                        {
                            _enemyBlast[state].Bind();
                        }
                        else if (belonging == Belonging.Player)
                        {
                            _enemyBlast[state].Bind();
                        }
                        break;
                    }
                case GameObject.Boss:
                    {
                        _boss[state].Bind();
                        break;
                    }
                case GameObject.Bullet:
                    {
                        _bullet.Bind();
                        break;
                    }
                case GameObject.Enemy:
                    {
                        _enemies[state].Bind();
                        break;
                    }
                case GameObject.Star:
                    {
                        _star[state].Bind();
                        break;
                    }
            }
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(1, 1);
            GL.Vertex2(-WindowProperty.GlObjectSize.X / 2, -WindowProperty.GlObjectSize.Y / 2);
            GL.TexCoord2(0, 1);
            GL.Vertex2(WindowProperty.GlObjectSize.X / 2, -WindowProperty.GlObjectSize.Y / 2);
            GL.TexCoord2(0, 0);
            GL.Vertex2(WindowProperty.GlObjectSize.X / 2, WindowProperty.GlObjectSize.Y / 2);
            GL.TexCoord2(1, 0);
            GL.Vertex2(-WindowProperty.GlObjectSize.X / 2, WindowProperty.GlObjectSize.Y / 2);
            GL.End();

            GL.Translate(-position.X * WindowProperty.ObjectSize / WindowProperty.WindowSize.X, -position.Y * WindowProperty.ObjectSize / WindowProperty.WindowSize.Y, 0);
        }
    }
}
