using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public class TriangleMesh<T> : IMesh where T : struct
    {
        private ILibraryContext Context;

        private IVertexBuffer<T> VXBuffer;
        private IIndexBuffer<int> IXBuffer;
        private int IndexCount;

        private bool Initialised = false;

        public TriangleMesh(ILibraryContext Context)
        {
            this.Context = Context;
            Initialised = false;
        }

        public TriangleMesh(ILibraryContext Context, T[] Vertices, int[] Indices)
        {
            this.Context = Context;
            this.SetVertices(Vertices, Indices);

            Initialised = true;
        }

        ~TriangleMesh()
        {
            this.Destroy();
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

            VXBuffer = Context.CreateVertexBuffer<T>(Vertices);
            IXBuffer = Context.CreateIndexBuffer<int>(Indices);

            IndexCount = Indices.Length;
        }

        public void Draw()
        {
            if (!Initialised)
                throw new InvalidOperationException("Mesh has not been initialised.");
            
            Context.DrawTriangles(0, IndexCount);
        }
    }
}
