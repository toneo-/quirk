using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.OpenTK
{
    class IndexBufferLegacy : IIndexBuffer
    {
        private int Reference = -1;
        private int BufferSize = -1;
        private BufferUsage Usage;

        public IndexBuffer(int[] Indices, BufferUsage Usage = BufferUsage.StaticDraw)
        {
            // Convert the array to bytes and fill the vertex buffer
            int bufferSize = Indices.Length * sizeof(int);
            byte[] rawBytes = new byte[bufferSize];

            // Far easier than vertices - we only use ints!
            Buffer.BlockCopy(Indices, 0, rawBytes, 0, bufferSize);

            unsafe
            {
                fixed (byte* rawPtr = rawBytes)
                    this.InitialiseBuffer((IntPtr)rawPtr, bufferSize, Usage);
            }
        }
        
        public IndexBuffer(int BufferSize, BufferUsage Usage = BufferUsage.StaticDraw)
        {
            this.InitialiseBuffer(IntPtr.Zero, BufferSize, Usage);
        }

        private void InitialiseBuffer(IntPtr Data, int BufferSize, BufferUsage Usage)
        {
            Reference = GL.GenBuffer();

            if (GL.GetError() != ErrorCode.NoError)
            {
                throw new QuirkGraphicsException("Failed to generate IBO!");
            }

            this.BufferSize = BufferSize;
            this.Usage = Usage;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Reference);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)BufferSize, Data, (BufferUsageHint)Usage);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Reference);
        }

        public void Destroy()
        {
            GL.DeleteBuffer(Reference);
        }

        /// <summary>
        /// Writes data from memory into the buffer.
        /// </summary>
        /// <param name="Length">The size of the data in bytes.</param>
        /// <param name="Data">The data to write.</param>
        public void WriteData(IntPtr Data, int Length)
        {
            if (Length > BufferSize)
                throw new QuirkGraphicsException("Length is greater than the size of the buffer!");

            this.Bind(); // Otherwise we'll write to the wrong buffer!

            IntPtr VideoMemPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);

            unsafe
            {
                byte* DataPtr = (byte*)Data.ToPointer();
                byte* VideoPtr = (byte*)VideoMemPtr.ToPointer();

                for (uint i = 0; i < Length; i++)
                    VideoPtr[i] = DataPtr[i];
            }

            GL.UnmapBuffer(BufferTarget.ElementArrayBuffer);
        }

        public void WriteData(int[] Indices)
        {
            // Work out array size
            Type t = Indices[0].GetType();
            int dataSize = Indices.Length * Marshal.SizeOf(t);

            if (dataSize > this.BufferSize)
                throw new QuirkGraphicsException("Indices array size exceeds buffer size!");

            // Copy data to a byte[] array so it's usable
            byte[] rawBytes = new byte[dataSize];
            Buffer.BlockCopy(Indices, 0, rawBytes, 0, dataSize);

            unsafe
            {
                fixed (byte* rawPtr = rawBytes)
                    WriteData(rawPtr, dataSize);
            }
        }

        private unsafe void WriteData(byte* DataPtr, int Length)
        {
            // Get video memory pointer, copy from sysmem to vidmem
            IntPtr VideoMemPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);
            byte* VideoPtr = (byte*)VideoMemPtr.ToPointer();

            for (uint i = 0; i < Length; i++)
                VideoPtr[i] = DataPtr[i];

            GL.UnmapBuffer(BufferTarget.ArrayBuffer);
        }
    }
}
