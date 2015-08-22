using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

// OpenTK Math stuff
using OpenTK;
using OpenTK.Graphics;

namespace Quirk.Graphics.VertexFormat
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    struct V2C4 : IVertexFormat
    {
        [ShaderLink("inPosition", typeof(float), 2)]
        public Vector2 Position;

        [ShaderLink("inColor", typeof(float), 4)]
        public Color4 Color;

        public V2C4(Vector2 Position, Color4 Color)
        {
            this.Position = Position;
            this.Color = Color;
        }
    }
}
