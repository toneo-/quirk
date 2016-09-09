using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public interface ILibraryContext
    {
        /// <summary>
        /// Returns the maximum number of uniform bindings available.
        /// </summary>
        int GetMaximumUniformBindings();

        /// <summary>
        /// Creates a new vertex buffer of the given size.
        /// </summary>
        /// <typeparam name="T">The vertex type stored by this buffer (i.e. V3N3T2). Must be value type (i.e. struct).</typeparam>
        /// <param name="Size">The size of the created buffer.</param>
        /// <param name="Usage">How the buffer will be used - for example, StaticDraw for vertices that will never change.</param>
        /// <returns>The created vertex buffer.</returns>
        IVertexBuffer<T> CreateVertexBuffer<T>(int Size, BufferUsage Usage = BufferUsage.StaticDraw) where T : struct;

        /// <summary>
        /// Creates a new vertex buffer and stores the given set of vertices.
        /// </summary>
        /// <typeparam name="T">The vertex type stored by this buffer (i.e. V3N3T2). Must be value type (i.e. struct).</typeparam>
        /// <param name="Vertices">The vertices to store.</param>
        /// <param name="Usage">How the buffer will be used - for example, StaticDraw for vertices that will never change.</param>
        /// <returns>The created vertex buffer.</returns>
        IVertexBuffer<T> CreateVertexBuffer<T>(T[] Vertices, BufferUsage Usage = BufferUsage.StaticDraw) where T : struct;

        /// <summary>
        /// Creates a new index buffer and stores the given set of indices.
        /// </summary>
        /// <typeparam name="T">The index type stored by this buffer (i.e. int). Must be a primitive type.</typeparam>
        /// <param name="Indices">The indices to store.</param>
        /// <returns>The created index buffer.</returns>
        IIndexBuffer<T> CreateIndexBuffer<T>(T[] Indices) where T : struct;

        // Todo: Document
        IUniformBuffer<T> CreateUniformBuffer<T>(T Data) where T : struct;
        IUniformBuffer<T> CreateUniformBuffer<T>(T[] Data) where T : struct;
        IUniformBuffer<T> CreateUniformBuffer<T>(int Size) where T : struct;

        // Also todo: document
        IVertexArrayObject CreateVertexArrayObject();

        /// <summary>
        /// Draws triangles using the currently bound vertex and index buffers.
        /// <b>Indices are assumed to be unsigned integers.</b>
        /// </summary>
        /// <param name="Offset"></param>
        /// <param name="Count"></param>
        void DrawTriangles(int Offset, int Count);
    }
}
