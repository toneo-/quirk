using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    /// <summary>
    /// Enum describing how a buffer object is likely to be used.
    /// For example, StaticDraw suggests that the contents of the buffer will never be modified and that the buffer will be used for drawing only.
    /// </summary>
    public enum BufferUsage : int
    {
        StreamDraw = 0x88E0,
        StreamRead = 0x88E1,
        StreamCopy = 0x88E2,

        StaticDraw = 0x88E4,
        StaticRead = 0x88E5,
        StaticCopy = 0x88E6,

        DynamicDraw = 0x88E8,
        DynamicRead = 0x88E9,
        DynamicCopy = 0x88EA
    }
}
