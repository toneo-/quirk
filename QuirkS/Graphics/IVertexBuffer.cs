using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    interface IVertexBuffer
    {
        void Bind();
        void Destroy();
        void WriteData(IntPtr Data, int Size);
        //void WriteData(T[] DataArray);
    }
}
