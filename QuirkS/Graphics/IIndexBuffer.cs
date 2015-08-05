using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    interface IIndexBuffer
    {
        void Bind();
        void Unbind();
        void Destroy();
        void WriteData(IntPtr Data, int Length);
        //void WriteData(T[] Indices);
    }
}
