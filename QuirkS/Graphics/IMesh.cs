using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public interface IMesh<T> where T : struct
    {
        void Bind();
        void Unbind();
        void Destroy();

        void SetVertices(T[] Vertices, int[] Indices);

        void Draw();
    }
}
