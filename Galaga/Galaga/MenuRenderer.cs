﻿using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using  System.Collections.Generic;

namespace Galaga
{
    static class MenuRenderer
    {
        private static TextString _textString;
        private static Texture _menuFrame;

        static MenuRenderer()
        {
            _textString = new TextString();
            _menuFrame = new Texture(new Bitmap("Texture/menu_frame.png"));
        }

        public static void Render()
        {
            if (Menu.CurrentMenu.Count == 0) return;
            GL.PushMatrix();

            GL.Translate(0, 0.5, 0);
            switch (GameStates.GameState)
            {
                case GameState.MainMenu:
                    {
                        _textString.PrepareToRender("GALAGA");
                        break;
                    }
                case GameState.Pause:
                    {
                        _textString.PrepareToRender("PAUSE");
                        break;
                    }
                case GameState.GameOver:
                    {
                        _textString.PrepareToRender("GAME OVER");
                        break;
                    }
            }
            _textString.Render();

            GL.Translate(0, -0.2, 0);

            foreach (KeyValuePair<int, MenuChoice> pair in Menu.CurrentMenu)
            {
                switch (pair.Value)
                {
                    case MenuChoice.Exit:
                        {
                            _textString.PrepareToRender("Exit");
                            break;
                        }
                    case MenuChoice.ExitToMenu:
                        {
                            _textString.PrepareToRender("Exit to menu");
                            break;
                        }
                    case MenuChoice.StartGame:
                        {
                            _textString.PrepareToRender("Start new game");
                            break;
                        }
                    case MenuChoice.Resume:
                        {
                            _textString.PrepareToRender("Resume");
                            break;
                        }
                    case MenuChoice.Settings:
                        {
                            _textString.PrepareToRender("Settings");
                            break;
                        }
                }
                _textString.Render();

                if (pair.Key == Menu.CurrentKey)
                {
                    FrameRender(_textString.Size.X / WindowProperty.WindowSize.X, _textString.Size.Y / WindowProperty.WindowSize.Y);
                }
                GL.Translate(0, -0.1, 0);
            }
            GL.PopMatrix();
        }

        private static void FrameRender(float x, float y)
        {
            _menuFrame.Bind();
            GL.Color4(Color4.White);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-x, -y);
            GL.TexCoord2(1, 0);
            GL.Vertex2(x, -y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(x, y);
            GL.TexCoord2(0, 1);
            GL.Vertex2(-x, y);
            GL.End();
        }
    }
}
