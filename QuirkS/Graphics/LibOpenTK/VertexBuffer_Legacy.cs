using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.OpenTK
{
    public class VertexBufferLegacy : IVertexBuffer
    {
        private int Reference = -1;
        private int BufferSize = -1;
        private BufferUsage Usage;

        /// <summary>
        /// Creates a VertexBuffer from the given array of vertices.
        /// </summary>
        /// <param name="Vertices">The vertices</param>
        /// <param name="Usage">How the buffer will be used - StaticDraw by default.</param>
        public VertexBuffer(Array Vertices, BufferUsage Usage = BufferUsage.StaticDraw)
        {
            unsafe
            {
                byte[] rawBytes = VerticesToByteArray(Vertices);
                
                fixed (byte* pRawBytes = rawBytes)
                {
                    IntPtr ipRawBytes = (IntPtr)pRawBytes;
                    this.InitialiseBuffer(ipRawBytes, rawBytes.Length, Usage);
                }
            }
        }

        public VertexBuffer(int BufferSize, BufferUsage Usage = BufferUsage.StaticDraw)
        {
            this.InitialiseBuffer(IntPtr.Zero, BufferSize, Usage);
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
            GL.BindBuffer(BufferTarget.ArrayBuffer, Reference);
        }

        /// <summary>
        /// Writes data from memory into the buffer.
        /// </summary>
        /// <param name="Length">The size of the data in bytes.</param>
        /// <param name="Data">The data to write.</param>
        public void WriteData(IntPtr Data, int Length)
        {
            if (Length > BufferSize) throw new QuirkGraphicsException("Data size exceeds buffer size!");

            unsafe
            {
                byte* dataPtr = (byte*)Data.ToPointer();
                this.WriteData(dataPtr, Length);
            }
        }

        public void WriteData(Array DataArray)
        {
            // Work out array size
            Type t = DataArray.GetValue(0).GetType();
            int dataSize = DataArray.Length * Marshal.SizeOf(t);

            if (dataSize > this.BufferSize)
                throw new QuirkGraphicsException("Data size exceeds buffer size!");

            // Copy data to a byte[] array so it's usable
            unsafe
            {
                byte[] rawBytes = VerticesToByteArray(DataArray);
                
                fixed (byte* rawPtr = rawBytes)
                    WriteData(rawPtr, dataSize);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="BufferSize"></param>
        /// <param name="Usage"></param>
        private void InitialiseBuffer(IntPtr Data, int BufferSize, BufferUsage Usage)
        {
            Reference = GL.GenBuffer();

            if (GL.GetError() != ErrorCode.NoError)
            {
                throw new QuirkGraphicsException("Failed to generate VBO!");
            }

            this.BufferSize = BufferSize;
            this.Usage = Usage;

            GL.BindBuffer(BufferTarget.ArrayBuffer, Reference);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)BufferSize, Data, (BufferUsageHint)Usage);
        }

        /// <summary>
        /// Creates a copy of an array of vertices as an array of bytes.
        /// </summary>
        /// <param name="Vertices">The vertices to copy. All vertices must be of the same type and must not contain references - this will cause memory leaks.</param>
        /// <returns>An array of bytes.</returns>
        private unsafe byte[] VerticesToByteArray(Array Vertices)
        {
            if (Vertices.Length == 0)
                throw new ArgumentException("Given array has no elements!");

            Type vertexType = Vertices.GetValue(0).GetType();

            // Work out required buffer size
            int vertexSize = Marshal.SizeOf(vertexType);
            int bufferSize = Vertices.Length * vertexSize;
            byte[] rawBytes = new byte[bufferSize];

            fixed (byte* pRawBytes = rawBytes)
            {
                IntPtr ipRawBytes = (IntPtr)pRawBytes;

                // Copy vertex data into the byte array
                for (int i = 0; i < Vertices.Length; i++)
                {
                    object Vertex = Vertices.GetValue(0);
                    Marshal.StructureToPtr(Vertex, ipRawBytes + (i * vertexSize), false);
                }
            }

            return rawBytes;
        }

        private unsafe void WriteData(byte* DataPtr, int Length)
        {
            if (Length > BufferSize)
                throw new ArgumentOutOfRangeException("Can't write more data than the buffer can hold!");

            this.Bind(); // Otherwise we'll write to the wrong buffer!

            // Get video memory pointer, copy from sysmem to vidmem
            IntPtr VideoMemPtr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);
            byte* VideoPtr = (byte*)VideoMemPtr.ToPointer();

            for (uint i = 0; i < Length; i++)
                VideoPtr[i] = DataPtr[i];

            GL.UnmapBuffer(BufferTarget.ArrayBuffer);
        }
    }
}
