using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics.Shaders
{
    interface IShader
    {
        QuirkShaderType GetType();
        void Destroy();
    }
}
