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
using Quirk.Graphics.Loaders;

using Quirk.Utility;

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

    partial class Program
    {
        // Todo: Separate the window stuff out (it's OpenTK specific)

        static void Main(string[] args)
        {
            using (var window = new GameWindow(800, 600, GraphicsMode.Default, "Quirk#", GameWindowFlags.FixedWindow, DisplayDevice.Default))
            {
                window.Load += window_Load;
                window.Resize += window_Resize;
                window.UpdateFrame += window_UpdateFrame;
                window.RenderFrame += window_RenderFrame;
                window.VSync = VSyncMode.Off;

                window.Closing += window_Closing;

                // Initial setup (pre-rendering)
                SetupTest();

                cam.Position = new Vector3(0, -5.0f, 0);
                cam.Angles = new Angle(0, 0, 0);

                window.Run(60, 60);
            }
        }

        static void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        static Camera cam = new Camera();

        static void window_RenderFrame(object sender, FrameEventArgs e)
        {
            GameWindow g = (GameWindow)sender;

            double fps = g.RenderFrequency;
            Console.WriteLine("FPS: " + fps);

            RenderTest();
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


            if (g.Keyboard[Key.Space])
                cam.Position = Vector3.Add(cam.Position, cam.UpVector * 0.5f);

            if (g.Keyboard[Key.ShiftLeft])
                cam.Position = Vector3.Add(cam.Position, cam.UpVector * -0.5f);


            if (g.Keyboard[Key.Q])
                cam.Angles = new Angle(cam.Angles.Pitch, cam.Angles.Yaw, cam.Angles.Roll - 0.1f);

            if (g.Keyboard[Key.E])
                cam.Angles = new Angle(cam.Angles.Pitch, cam.Angles.Yaw, cam.Angles.Roll + 0.1f);
        }
    }
}
