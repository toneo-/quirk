using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

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
    }
}
