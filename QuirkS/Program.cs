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

            using (var window = new GameWindow())
            {
                window.Load += window_Load;
                window.Resize += window_Resize;
                window.UpdateFrame += window_UpdateFrame;
                window.RenderFrame += window_RenderFrame;

                V2C4[] vboData = {
                    new V2C4(new Vector2(-1.0f, 1.0f), Color.Red),
                    new V2C4(new Vector2(0.0f, -1.0f), Color.Green),
                    new V2C4(new Vector2(1.0f, 1.0f), Color.Blue),
                };

                int[] iboData = {
                    0, 1, 2
                };

                vBuffer = new VertexBuffer<V2C4>(vboData);
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

        static void window_RenderFrame(object sender, FrameEventArgs e)
        {
            GameWindow g = (GameWindow)sender;

            if (first)
            {
                first = false;

                vertexSh = new VertexShader(File.ReadAllText("Resources/Shaders/Test/test_V2C4.glsl"));
                fragSh = new FragmentShader(File.ReadAllText("Resources/Shaders/null_frag.glsl"));

                shProg = new ShaderProgram();
                shProg.Attach(vertexSh);
                shProg.Attach(fragSh);

                vao = new VertexArrayObject();
                vao.Bind();

                iBuffer.Bind();
                vBuffer.Bind();

                shProg.SetupFormat(typeof(V2C4));
                shProg.Use();

                vao.Unbind();
            }

            ActiveRenderer.Clear(Color.CornflowerBlue);

            vao.Bind();

            Matrix4 persp = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)(16 / 9), 1, 64);
            //shProg.SetUniform("Projection", ref persp);

            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            //shProg.SetUniform("ModelView", ref lookat);

            GL.PointSize(5.0f);

            // Something's wrong! ***************
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawArrays(PrimitiveType.Points, 0, 3);

            vao.Unbind();

            g.SwapBuffers();
        }

        static void window_Load(object sender, EventArgs e)
        {
            GameWindow g = (GameWindow)sender;

            g.VSync = VSyncMode.On;
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

        }
    }
}
