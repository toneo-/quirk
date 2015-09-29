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
    public class UniformBuffer<T> : IGenericBuffer where T : struct
    {
        private int Reference = -1;
        private int BufferSize = -1;

        /// <summary>
        /// Creates a UniformBuffer from the given data.
        /// </summary>
        /// <param name="Data">The data</param>
        public UniformBuffer(T Data)
        {
            this.GenerateBuffer();

            // Work out size of buffer needed
            this.BufferSize = Marshal.SizeOf(typeof(T));

            GL.BindBuffer(BufferTarget.UniformBuffer, Reference);
            GL.BufferData<T>(BufferTarget.UniformBuffer, new IntPtr(BufferSize), ref Data, BufferUsageHint.DynamicDraw);
        }

        /// <summary>
        /// Creates a UniformBuffer from the given array of data.
        /// </summary>
        /// <param name="Data">The data</param>
        public UniformBuffer(T[] Data)
        {
            this.GenerateBuffer();

            // Work out size of buffer needed
            this.BufferSize = Marshal.SizeOf(typeof(T)) * Data.Length;

            GL.BindBuffer(BufferTarget.UniformBuffer, Reference);
            GL.BufferData<T>(BufferTarget.UniformBuffer, new IntPtr(BufferSize), Data, BufferUsageHint.DynamicDraw);
        }

        /// <summary>
        /// Creates a blank UniformBuffer of a given size.
        /// </summary>
        /// <param name="BufferSize">The size of the buffer in bytes</param>
        public UniformBuffer(int BufferSize)
        {
            this.GenerateBuffer();

            GL.BindBuffer(BufferTarget.UniformBuffer, Reference);
            GL.BufferData(BufferTarget.UniformBuffer, new IntPtr(BufferSize), IntPtr.Zero, BufferUsageHint.DynamicDraw);
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
            // TEMPORARY !!!
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, Reference);
            GL.BindBuffer(BufferTarget.UniformBuffer, Reference);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        public void WriteData(IntPtr Data, int Size)
        {
            // Sanity checks
            if (Size < 0)
                throw new QuirkGraphicsException("Data size must be >= 0!");
            else if (Size > this.BufferSize)
                throw new QuirkGraphicsException("Data size exceeds buffer size!");

            // Load the data into the buffer
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, new IntPtr(Size), Data);
        }

        public void WriteData(T[] Data)
        {
            int dataSize = Marshal.SizeOf(typeof(T)) * Data.Length;

            // Sanity check
            if (dataSize > this.BufferSize)
                throw new QuirkGraphicsException("Data size exceeds buffer size!");

            // Load the data into the buffer
            GL.BufferSubData<T>(BufferTarget.UniformBuffer, IntPtr.Zero, new IntPtr(dataSize), Data);
        }

        private void GenerateBuffer()
        {
            Reference = GL.GenBuffer();

            if (GL.GetError() != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to generate UBO!");
        }
    }
}
