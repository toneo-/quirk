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
        // Todo: Binding indices may be freed up later on
        private int UniformBindingCount = 0;

        /// <summary>
        /// Gets the next uniform binding index.
        /// </summary>
        private int NextUniformBindingIndex()
        {
            UniformBindingCount++;
            return (UniformBindingCount - 1);
        }

        public int GetMaximumUniformBindings()
        {
            throw new NotImplementedException();
        }

        // ** Vertex Buffers **

        public IVertexBuffer<T> CreateVertexBuffer<T>(int Size, BufferUsage Usage = BufferUsage.StaticDraw) where T : struct
        {
            return new LibOpenTK.VertexBuffer<T>(Size, Usage);
        }

        public IVertexBuffer<T> CreateVertexBuffer<T>(T[] Vertices, BufferUsage Usage = BufferUsage.StaticDraw) where T : struct
        {
            return new LibOpenTK.VertexBuffer<T>(Vertices, Usage);
        }

        // ** Index Buffers **

        public IIndexBuffer<T> CreateIndexBuffer<T>(T[] Indices) where T : struct
        {
            return new LibOpenTK.IndexBuffer<T>(Indices);
        }

        // ** Uniform Buffers **

        public IUniformBuffer<T> CreateUniformBuffer<T>(int Size) where T : struct
        {
            return new LibOpenTK.UniformBuffer<T>(Size, this.NextUniformBindingIndex());
        }

        public IUniformBuffer<T> CreateUniformBuffer<T>(T Data) where T : struct
        {
            return new LibOpenTK.UniformBuffer<T>(Data, this.NextUniformBindingIndex());
        }

        public IUniformBuffer<T> CreateUniformBuffer<T>(T[] Data) where T : struct
        {
            return new LibOpenTK.UniformBuffer<T>(Data, this.NextUniformBindingIndex());
        }

        // ** Drawing **

        public void DrawTriangles(int Offset, int Count)
        {
            GL.DrawElements(PrimitiveType.Triangles, Count, DrawElementsType.UnsignedInt, new IntPtr(Offset));
        }
    }
}