using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public interface IUniformBuffer<T> : IGenericBuffer where T : struct
    {
        int GetBindingIndex();

        /// <summary>
        /// Writes a single piece of data to the uniform buffer.
        /// </summary>
        /// <param name="Data">The data to write.</param>
        void WriteData(T Data);

        /// <summary>
        /// Writes a single piece of data to the uniform buffer at position (Size of Data) * Offset,
        /// much like writing to an array position.
        /// </summary>
        /// <param name="Data">The data to write.</param>
        /// <param name="Offset">
        ///     The offset to write at - as a multiple of data's size.
        ///     Ex: If data is an int, the position will be sizeof(int) * Offset.</param>
        void WriteData(T Data, int Offset);

        /// <summary>
        /// Writes an array of data to the uniform buffer.
        /// </summary>
        /// <param name="Data">The array of data to write.</param>
        void WriteData(T[] Data);
    }
}
