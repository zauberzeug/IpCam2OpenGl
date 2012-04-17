using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace IpCam2OpenGl
{
    public class Quad
    {
        Vector2 position = new Vector2 (0, 0);


        public Quad ()
        {
            Alpha = 1f;
            DrawWireframe = false;
            Color = new Vector4(1f, 1f, 1f, Alpha);
        }

        public Vector4 Color { get; set;}

        public float Alpha { get; set; }

        public bool DrawWireframe { get; set; }

        public Vector2 Position {
            set {
                position = value;
            }
            get {
                return position;
            }
        }

        public float X {
            set {
                position = new Vector2 (value, position.Y);
            }
            get {
                return position.X;
            }
        }

        public float Y {
            set {
                position = new Vector2 (position.X, value);
            }
            get {
                return position.Y;
            }
        }

        public void Draw ()
        {
            if (DrawWireframe) {
                GL.PolygonMode (MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.LineWidth (3f);
                GL.Enable (EnableCap. LineSmooth);
            }
            GL.PushMatrix ();

            GL.Translate (
                position.X, position.Y, 0);

            GL.Begin (BeginMode.Quads);

            if (DrawWireframe) {
                GL.Color3 (1 - Alpha, Alpha, 0.0f);
            } else
                GL.Color4 (Color);

            GL.TexCoord2 (1.0f, 1.0f);
            GL.Vertex3 (-1.0f, -1.0f, 0f);

            GL.TexCoord2 (0.0f, 1.0f);
            GL.Vertex3 (1.0f, -1.0f, 0f);

            GL.TexCoord2 (0.0f, 0.0f);
            GL.Vertex3 (1.0f, 1.0f, 0.0f);

            GL.TexCoord2 (1.0f, 0.0f);
            GL.Vertex3 (-1.0f, 1.0f, 0f);

            GL.End ();

            GL.PopMatrix ();

            if (DrawWireframe)
                GL.PolygonMode (MaterialFace.FrontAndBack, PolygonMode.Fill);

        }
    }
}

