using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Quirk.Graphics
{
    /// <summary>
    /// Renderer interface
    /// </summary>
    interface IRenderer
    {
        /// <summary>
        /// Clears the depth buffer.
        /// </summary>
        void ClearDepthBuffer();

        /// <summary>
        /// Clears the colour buffer with the given colour, then clears the depth buffer.
        /// </summary>
        void Clear(Color c);
    }
}
