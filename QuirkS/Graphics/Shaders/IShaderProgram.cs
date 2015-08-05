using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quirk.Graphics.VertexFormat;

namespace Quirk.Graphics.Shaders
{
    interface IShaderProgram
    {
        void Link();
        void Unlink();

        void Attach(IShader Shader);
        void Detach(IShader Shader);

        void SetupFormat(IVertexFormat VertexFormat);
        int GetReference();

        void Destroy();
    }
}
