using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    interface IVertexArrayObject
    {
        void Bind();
        void Unbind();

        int GetReference();
        void Destroy();
    }
}
