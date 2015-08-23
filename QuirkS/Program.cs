using System;
using System.Drawing;

using System.Runtime.InteropServices;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using Quirk.Graphics;
using Quirk.Graphics.LibOpenTK;
using Quirk.Graphics.Shaders;
using Quirk.Graphics.Shaders.LibOpenTK;
using Quirk.Graphics.VertexFormat;

namespace Quirk
{
    class Program
    {
        // Todo: Separate the window stuff out (it's OpenTK specific)
        
        static IRenderer ActiveRenderer = new RendererTK();
        static IGenericBuffer vBuffer, iBuffer;

        static void Main(string[] args)
        {
            using (var window = new GameWindow(800, 600))
            {
                window.Load += window_Load;
                window.Resize += window_Resize;
                window.UpdateFrame += window_UpdateFrame;
                window.RenderFrame += window_RenderFrame;

                V3C4[] vboData = {
                    new V3C4(new Vector3(-0.8f, 0.0f,-0.8f), Color.Red),
                    new V3C4(new Vector3(0.8f, 0.0f, -0.8f), Color.Green),
                    new V3C4(new Vector3(0.0f, 0.0f, 0.8f), Color.Blue),
                };

                int[] iboData = {
                    0, 1, 2
                };

                vBuffer = new VertexBuffer<V3C4>(vboData);
                iBuffer = new IndexBuffer<int>(iboData);

                window.Closing += window_Closing;

                window.Run(60);
            }
        }

        static void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vBuffer.Destroy();
            iBuffer.Destroy();
        }

        static bool first = true;
        static IVertexArrayObject vao;
        static IShader fragSh, vertexSh;
        static IShaderProgram shProg;
        static float shift1 = 0.0f;
        static float shift2 = -5.0f;

        static void window_RenderFrame(object sender, FrameEventArgs e)
        {
            GameWindow g = (GameWindow)sender;

            if (first)
            {
                first = false;

                vertexSh = new VertexShader(File.ReadAllText("Resources/Shaders/Test/test_V3C4.glsl"));
                fragSh = new FragmentShader(File.ReadAllText("Resources/Shaders/null_frag.glsl"));

                shProg = new ShaderProgram();
                shProg.Attach(vertexSh);
                shProg.Attach(fragSh);

                vao = new VertexArrayObject();
                vao.Bind();

                iBuffer.Bind();
                vBuffer.Bind();

                shProg.SetupFormat(typeof(V3C4));
                shProg.Use();

                vao.Unbind();
            }

            ActiveRenderer.Clear(Color.CornflowerBlue);

            vao.Bind();

            Camera cam = new Camera();
            cam.Position = new Vector3(shift1, shift2, 0);
            cam.Angles = new Angle(0, 0, 0);

            Matrix4 persp = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, (float)(16 / 9), 1, 64);
            Matrix4 lookat = cam.PerspectiveMatrix;// Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 0, 1);
            Matrix4 PMV = lookat * persp;

            shProg.SetUniform("ProjectionModelView", ref PMV);

            GL.PointSize(15.0f);

            // Something's wrong! ***************
            GL.DrawArrays(PrimitiveType.Points, 0, 3);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            vao.Unbind();

            GL.Flush();
            g.SwapBuffers();
        }

        static void window_Load(object sender, EventArgs e)
        {
            GameWindow g = (GameWindow)sender;
        }

        static void window_Resize(object sender, EventArgs e)
        {
            GameWindow g = (GameWindow)sender;

            GL.Viewport(0, 0, g.Width, g.Height);
        }

        static void window_UpdateFrame(object sender, FrameEventArgs e)
        {
            GameWindow g = (GameWindow)sender;

            if (g.Keyboard[Key.Enter])
                g.Exit();

            if (g.Keyboard[Key.Left])
                shift1 -= 0.1f;

            if (g.Keyboard[Key.Right])
                shift1 += 0.1f;

            if (g.Keyboard[Key.Up])
                shift2 += 0.1f;

            if (g.Keyboard[Key.Down])
                shift2 -= 0.1f;
        }
    }
}
