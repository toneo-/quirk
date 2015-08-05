using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics.Shaders
{
    interface IShaderProgram
    {
        void Link();
        void Unlink();

        void Attach(IShader Shader);
        void Detach(IShader Shader);

        int GetReference();

        void Destroy();
    }
}
