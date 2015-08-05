using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.LibOpenTK
{
    /// <summary>
    /// OpenTK renderer
    /// </summary>
    public class RendererTK : IRenderer
    {
        public void ClearDepthBuffer()
        {
            //GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.ClearDepth(0);
        }

        public void Clear(Color c)
        {
            GL.ClearColor(c);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
