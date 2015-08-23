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
    struct V3C4 : IVertexFormat
    {
        [ShaderLink("inPosition", typeof(float), 3)]
        public Vector3 Position;

        [ShaderLink("inColor", typeof(float), 4)]
        public Color4 Color;

        public V3C4(Vector3 Position, Color4 Color)
        {
            this.Position = Position;
            this.Color = Color;
        }
    }
}
