using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;
using Quirk.Graphics.VertexFormat;

namespace Quirk.Graphics.Shaders.LibOpenTK
{
    class ShaderProgram : IShaderProgram
    {
        protected int Reference = -1;
        protected List<IShader> LinkedShaders = new List<IShader>();

        public ShaderProgram()
        {
            Reference = GL.CreateProgram();

            if (GL.GetError() != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to generate shader program! " + GL.GetError().ToString());
        }

        public void Link()
        {
            GL.LinkProgram(Reference);

            if (GL.GetError() != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to link shader program! " + GL.GetError().ToString());
        }

        public void Unlink()
        {
            GL.LinkProgram(0);
        }

        public int GetReference()
        {
            return Reference;
        }

        public void Destroy()
        {
            foreach (IShader shader in LinkedShaders)
            {
                this.Detach(shader);
                shader.Destroy();
            }

            GL.DeleteProgram(Reference);

            if (GL.GetError() != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to destroy shader program! " + GL.GetError().ToString());
            Reference = -1;
        }

        public void Attach(IShader Shader)
        {
            GL.AttachShader(Reference, Shader.GetReference());
        }

        public void Detach(IShader Shader)
        {
            GL.DetachShader(Reference, Shader.GetReference());
        }

        void SetupFormat(IVertexFormat VertexFormat)
        {
            int colorComponents = VertexFormat.GetColorComponents();
            int vectorComponents = VertexFormat.GetVertexComponents();

            // Todo: redo this

            //GL.VertexPointer(2, VertexPointerType.Float, 2 * sizeof(float) + 4 * sizeof(float), IntPtr.Zero);
            //GL.ColorPointer(4, ColorPointerType.Float, 2 * sizeof(float) + 4 * sizeof(float), (IntPtr)(2 * sizeof(float)));

            // If there are color components...
            if (colorComponents > 0)
            {
                int addrColorIn = GL.GetAttribLocation(Reference, "inColor");
                GL.VertexAttribPointer(addrColorIn, colorComponents, VertexAttribPointerType.Float, false, 0, 0);
            }

            // If there are vector components (basically guaranteed)
            if (vectorComponents > 0)
            {
                int addrPositionIn = GL.GetAttribLocation(Reference, "inPosition");
                GL.VertexAttribPointer(addrPositionIn, vectorComponents, VertexAttribPointerType.Float, false, 0, 0);
            }
        }
    }
}
