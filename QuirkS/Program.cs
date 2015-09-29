﻿using System;
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
using Quirk.Graphics.Loaders;

namespace Quirk
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    struct terribleLight
    {
        public Vector3 Direction;
        float _Padding1;
        public Vector3 Position;
        float _Padding2;
        public Vector3 Color;
        public float Range;
    };

    //struct DirectionalLight
    //{
    //    vec3 Direction;
    //    vec3 Position;
    //    vec3 Color;
    //    float Range;
    //};

    class Program
    {
        // Todo: Separate the window stuff out (it's OpenTK specific)
        
        static IRenderer ActiveRenderer = new RendererTK();
        static IGenericBuffer vBuffer, iBuffer;
        static IMesh<V3N3> Mesh;

        static void Main(string[] args)
        {
            using (var window = new GameWindow(800, 600))
            {
                window.Load += window_Load;
                window.Resize += window_Resize;
                window.UpdateFrame += window_UpdateFrame;
                window.RenderFrame += window_RenderFrame;
                window.VSync = VSyncMode.Off;

                V3N3T2[] rawVertices;
                V3N3[] vboData;
                int[] iboData;

                //OBJLoader.LoadFromStream(File.OpenRead(@"C:\Users\Toneo\Projects\C#\QuirkS\suzanne.obj"), out rawVertices, out iboData);
                //OBJLoader.LoadV3N3Stream(File.OpenRead(@"E:\suzanne.obj"), out vboData, out iboData, true);

                OBJLoader loader = new OBJLoader(true);
                loader.LoadFromStream(File.OpenRead(@"E:\suzanne.obj"), out rawVertices, out iboData);

                // Convert to V3N3
                vboData = new V3N3[rawVertices.Length];

                for (int i = 0; i < rawVertices.Length; i++)
                    vboData[i] = new V3N3(rawVertices[i].Position, rawVertices[i].Normal);

                // Generate Mesh
                Mesh = new TriangleMesh<V3N3>(vboData, iboData);

                window.Closing += window_Closing;

                window.Run(200, 60);
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

        static IGenericBuffer ubo;

        static Camera cam = new Camera();

        static terribleLight[] lights = new terribleLight[10];

        static void window_RenderFrame(object sender, FrameEventArgs e)
        {
            GameWindow g = (GameWindow)sender;

            double fps = g.RenderFrequency;
            Console.WriteLine("FPS: " + fps);

            if (first)
            {
                first = false;

                vertexSh = new VertexShader(File.ReadAllText("Resources/Shaders/Test/test_vert_V3N3.glsl"));
                fragSh = new FragmentShader(File.ReadAllText("Resources/Shaders/Test/test_frag_V3N3.glsl"));

                shProg = new ShaderProgram();
                shProg.Attach(vertexSh);
                shProg.Attach(fragSh);

                lights[0].Direction = new Vector3(0, 0.0f, 1.0f);
                lights[0].Position = new Vector3(0, 0.0f, -10.0f);
                lights[0].Color = new Vector3(1.0f, 0.5f, 0.0f);
                lights[0].Range = 1000.0f;

                lights[1].Direction = new Vector3(0.0f, 0.0f, -5.0f);
                lights[1].Position = new Vector3(0, 0.0f, 5.0f);
                lights[1].Color = new Vector3(0.0f, 0.5f, 1.0f);
                lights[1].Range = 15.0f;

                ubo = new UniformBuffer<terribleLight>(lights);

             //Vector3 Direction = new Vector3(0, 0.0f, 1.0f);
            //Vector3 Position = new Vector3(0, 0.0f, -10.0f);
            //Vector3 Clor = new Vector3(1.0f, 0.5f, 0.0f);
            //shProg.SetUniform("Lights[0].Direction", ref Direction);
            ////shProg.SetUniform("Lights[0].Position", ref Position);
            //shProg.SetUniform("Lights[0].Color", ref Clor);
            //shProg.SetUniform("Lights[0].Range", Range);

                vao = new VertexArrayObject();
                vao.Bind();

                GL.Enable(EnableCap.DepthTest);

                Mesh.Bind();
                ubo.Bind();

                shProg.SetupFormat(typeof(V3N3));
                shProg.Use();
                shProg.BindUniformBlock("BlockyBlock", 0);

                vao.Unbind();

                cam.Position = new Vector3(0, -5.0f, 0);
                cam.Angles = new Angle(0, 0, 0);
            }

            ActiveRenderer.Clear(Color.CornflowerBlue);

            vao.Bind();


            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, (float)(16 / 9), 1, 1024);
            Matrix4 view = cam.PerspectiveMatrix;// Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 0, 1);
            Matrix4 model = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
            Matrix4 MVP = model * view * projection;

            shProg.SetUniform("ModelViewProjection", ref MVP);
            shProg.SetUniform("Model", ref model);

            // "DO YOU HAVE EPILEPSY?" mode
            //Random r = new Random();
            //lights[0].Direction.X = (float)(r.NextDouble() * 1.0f);
            //lights[0].Direction.Y = (float)(r.NextDouble() * 1.0f);
            //lights[0].Direction.Z = (float)(r.NextDouble() * 1.0f);
            //lights[0].Direction.Normalize();

            //lights[0].Position.X = (float)(r.NextDouble() * 1.0f);
            //lights[0].Position.Y = (float)(r.NextDouble() * 1.0f);
            //lights[0].Position.Z = (float)(r.NextDouble() * 1.0f);
            //lights[0].Position = (lights[0].Position.Normalized() * 20.0f);
            //((UniformBuffer<terribleLight>)(ubo)).WriteData(lights);

            shProg.SetUniform("LightCount", 5);

            GL.PointSize(15.0f);

            Mesh.Draw();

            vao.Unbind();
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

            if (g.Keyboard[Key.W])
                cam.Position = Vector3.Add(cam.Position, cam.Angles.Forward() * 0.5f);

            if (g.Keyboard[Key.S])
                cam.Position = Vector3.Add(cam.Position, -cam.Angles.Forward() * 0.5f);

            if (g.Keyboard[Key.A])
                cam.Angles = new Angle(cam.Angles.Pitch, cam.Angles.Yaw - 0.3f, cam.Angles.Roll);

            if (g.Keyboard[Key.D])
                cam.Angles = new Angle(cam.Angles.Pitch, cam.Angles.Yaw + 0.3f, cam.Angles.Roll);

            if (g.Keyboard[Key.Left])
                cam.Position = Vector3.Add(cam.Position, -cam.Angles.Right());

            if (g.Keyboard[Key.Right])
                cam.Position = Vector3.Add(cam.Position, cam.Angles.Right());


            if (g.Keyboard[Key.Q])
                cam.Angles = new Angle(cam.Angles.Pitch, cam.Angles.Yaw, cam.Angles.Roll - 0.1f);

            if (g.Keyboard[Key.E])
                cam.Angles = new Angle(cam.Angles.Pitch, cam.Angles.Yaw, cam.Angles.Roll + 0.1f);
        }
    }
}
