using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.Shaders.LibOpenTK
{
    class VertexShader : IShader
    {
        protected int Reference = -1;
        
        public VertexShader(string ShaderSource)
        {
            this.GenerateShader();
            this.CompileAndLoad(ShaderSource);
        }

        ~VertexShader()
        {
            this.Destroy();
        }

        public QuirkShaderType GetShaderType()
        {
            return QuirkShaderType.VertexShader;
        }

        public int GetReference()
        {
            return Reference;
        }

        public void Destroy()
        {
            GL.DeleteShader(Reference);
            Reference = -1;
        }

        protected void GenerateShader()
        {
            Reference = GL.CreateShader(ShaderType.VertexShader);

            if (GL.GetError() != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to generate shader! " + GL.GetError().ToString());
        }

        protected void CompileAndLoad(string ShaderSource)
        {
            GL.ShaderSource(Reference, ShaderSource);
            GL.CompileShader(Reference);

            int statusCode;
            GL.GetShader(Reference, ShaderParameter.CompileStatus, out statusCode);

            if (statusCode != 1) // If compilation failed, error
            {
                string InfoLog = GL.GetShaderInfoLog(Reference);
                throw new QuirkShaderCompilationException("Failed to compile shader.", InfoLog);
            }
        }
    }
}
