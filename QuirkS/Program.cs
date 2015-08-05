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
        
        static void window_RenderFrame(object sender, FrameEventArgs e)
        {
            GameWindow g = (GameWindow)sender;

            if (first)
            {
                first = false;

                vao = new VertexArrayObject();
                vao.Bind();

                GL.EnableClientState(ArrayCap.VertexArray);
                GL.EnableClientState(ArrayCap.ColorArray);

                iBuffer.Bind();
                vBuffer.Bind();

                GL.VertexPointer(2, VertexPointerType.Float, 2 * sizeof(float) + 4 * sizeof(float), IntPtr.Zero);
                GL.ColorPointer(4, ColorPointerType.Float, 2 * sizeof(float) + 4 * sizeof(float), (IntPtr)(2 * sizeof(float)));
                
                vao.Unbind();

                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.ColorArray);

                vertexSh = new FragmentShader(File.ReadAllText("Resources/Shaders/null_vertex.glsl"));
                fragSh = new FragmentShader(File.ReadAllText("Resources/Shaders/null_frag.glsl"));
            }

            ActiveRenderer.Clear(Color.CornflowerBlue);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            //GL.Translate(new Vector3(0.0f, 0.0f, -1.0f));
            //GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
            //GL.Frustum(-1.0, 1.0, -1.0, 1.0, 0.0, 400.0);
            Matrix4 persp = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)(16/9), 1, 64);
                //OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.LoadMatrix(ref persp);

            vao.Bind();

            GL.MatrixMode(MatrixMode.Modelview);
            
            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.LoadMatrix(ref lookat);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
