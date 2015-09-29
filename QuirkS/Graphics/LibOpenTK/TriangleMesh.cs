using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.LibOpenTK
{
    public class TriangleMesh<T> : IMesh<T> where T : struct
    {
        private VertexBuffer<T> VXBuffer;
        private IndexBuffer<int> IXBuffer;
        private int IndexCount;

        private bool Initialised = false;

        public TriangleMesh()
        {
            Initialised = false;
        }

        public TriangleMesh(T[] Vertices, int[] Indices)
        {
            VXBuffer = new VertexBuffer<T>(Vertices);
            IXBuffer = new IndexBuffer<int>(Indices);

            IndexCount = Indices.Length;
            Initialised = true;
        }

        public void Bind()
        {
            if (!Initialised)
                throw new InvalidOperationException("Mesh has not been initialised.");

            VXBuffer.Bind();
            IXBuffer.Bind();
        }

        public void Unbind()
        {
            VXBuffer.Unbind();
            IXBuffer.Unbind();
        }

        public void Destroy()
        {
            VXBuffer.Destroy();
            IXBuffer.Destroy();
        }

        public void SetVertices(T[] Vertices, int[] Indices)
        {
            if (Initialised)
                throw new InvalidOperationException("Mesh has already been initialised.");

            VXBuffer = new VertexBuffer<T>(Vertices);
            IXBuffer = new IndexBuffer<int>(Indices);

            IndexCount = Indices.Length;
        }

        public void Draw()
        {
            if (!Initialised)
                throw new InvalidOperationException("Mesh has not been initialised.");
            
            //GL.DrawArrays(PrimitiveType.Lines, 0, IndexCount);
            //GL.DrawElements(BeginMode.Triangles, IndexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}
