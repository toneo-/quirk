using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK; // Vector2
using Quirk.Graphics.VertexFormat;

namespace Quirk.Graphics.Shaders
{
    interface IShaderProgram
    {
        void Link();
        void Unlink();
        void Use();

        void Attach(IShader Shader);
        void Detach(IShader Shader);

        void SetupFormat(Type VertexFormat);

        void BindUniformBlock(string Name, int BindingIndex);

        void SetUniform(string Name, int Value);
        void SetUniform(string Name, float Value);
        void SetUniform(string Name, ref Vector2 Value);
        void SetUniform(string Name, ref Vector3 Value);
        void SetUniform(string Name, ref Matrix4 Value);

        // NB: Add extra SetUniforms
        //     And maybe GetUniforms

        int GetReference();
        void Destroy();
    }
}
