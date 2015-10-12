using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.LibOpenTK
{
    /// <summary>
    /// OpenTK-specific implementation of ILibraryContext.
    /// </summary>
    public class OpenTKLibraryContext : ILibraryContext
    {
        public int GetMaximumUniformBindings()
        {
            throw new NotImplementedException();
        }

        public IVertexBuffer<T> CreateVertexBuffer<T>(int Size, BufferUsage Usage = BufferUsage.StaticDraw) where T : struct
        {
            return new LibOpenTK.VertexBuffer<T>(Size, Usage);
        }

        public IVertexBuffer<T> CreateVertexBuffer<T>(T[] Vertices, BufferUsage Usage = BufferUsage.StaticDraw) where T : struct
        {
            return new LibOpenTK.VertexBuffer<T>(Vertices, Usage);
        }

        public IIndexBuffer<T> CreateIndexBuffer<T>(T[] Indices) where T : struct
        {
            return new LibOpenTK.IndexBuffer<T>(Indices);
        }

        public void DrawTriangles(int Offset, int Count)
        {
            GL.DrawElements(PrimitiveType.Triangles, Count, DrawElementsType.UnsignedInt, new IntPtr(Offset));
        }
    }
}