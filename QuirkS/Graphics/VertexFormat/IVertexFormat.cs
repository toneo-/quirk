using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics.VertexFormat
{
    interface IVertexFormat
    {
        int GetStride();

        public int GetColorComponents();
        public int GetVertexComponents();

        public Type GetColorType();
        public Type GetVertexType();
    }
}
