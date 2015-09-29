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
    public struct V3N3T2 : IVertexFormat
    {
        [ShaderLink("inPosition", typeof(float), 3)]
        public Vector3 Position;

        [ShaderLink("inNormal", typeof(float), 3)]
        public Vector3 Normal;

        [ShaderLink("inUV", typeof(float), 2)]
        public Vector2 UV;

        public V3N3T2(Vector3 Position, Vector3 Normal, Vector2 UV)
        {
            this.Position = Position;
            this.Normal = Normal;
            this.UV = UV;
        }

        public override int GetHashCode()
        {
            // Only using 3 of our floats.
            return (Position.X + Normal.X + UV.X).GetHashCode();
        }
    }
}
