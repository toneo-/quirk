using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.LibOpenTK
{
    public class IndexBuffer<T> : IIndexBuffer<T> where T : struct
    {
        private int Reference = -1;
        private int BufferSize = -1;
        private BufferUsage Usage;

        /// <summary>
        /// Creates a IndexBuffer from the given array of data.
        /// </summary>
        /// <param name="Data">The data</param>
        /// <param name="Usage">How the buffer will be used - StaticDraw by default.</param>
        public IndexBuffer(T[] Data, BufferUsage Usage = BufferUsage.StaticDraw)
        {
            this.GenerateBuffer();

            // Work out size of buffer needed
            this.BufferSize = Marshal.SizeOf(typeof(T)) * Data.Length;
            this.Usage = Usage;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Reference);
            GL.BufferData<T>(BufferTarget.ElementArrayBuffer, new IntPtr(BufferSize), Data, (BufferUsageHint)Usage);
        }

        /// <summary>
        /// Creates a blank IndexBuffer of a given size.
        /// </summary>
        /// <param name="BufferSize">The size of the buffer in bytes</param>
        /// <param name="Usage">How the buffer will be used - StaticDraw by default.</param>
        public IndexBuffer(int BufferSize, BufferUsage Usage = BufferUsage.StaticDraw)
        {
            this.GenerateBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Reference);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(BufferSize), IntPtr.Zero, (BufferUsageHint)Usage);
        }

        ~IndexBuffer()
        {
            this.Destroy();
        }

        /// <summary>
        /// Immediately deletes and invalidates this vertex buffer.
        /// </summary>
        public void Destroy()
        {
            GL.DeleteBuffer(Reference);

            Reference = -1;
            BufferSize = -1;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Reference);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void WriteData(IntPtr Data, int Size)
        {
            // Sanity checks
            if (Size < 0)
                throw new QuirkGraphicsException("Data size must be >= 0!");
            else if (Size > this.BufferSize)
                throw new QuirkGraphicsException("Data size exceeds buffer size!");

            // Load the data into the buffer
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, new IntPtr(Size), Data);
        }

        public void WriteData(T[] Data)
        {
            int dataSize = Marshal.SizeOf(typeof(T)) * Data.Length;

            // Sanity check
            if (dataSize > this.BufferSize)
                throw new QuirkGraphicsException("Data size exceeds buffer size!");

            // Load the data into the buffer
            GL.BufferSubData<T>(BufferTarget.ElementArrayBuffer, IntPtr.Zero, new IntPtr(dataSize), Data);
        }

        private void GenerateBuffer()
        {
            Reference = GL.GenBuffer();

            if (GL.GetError() != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to generate VBO!");
        }
    }
}
