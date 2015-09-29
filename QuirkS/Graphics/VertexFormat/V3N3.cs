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
    public struct V3N3 : IVertexFormat
    {
        [ShaderLink("inPosition", typeof(float), 3)]
        public Vector3 Position;

        [ShaderLink("inNormal", typeof(float), 3)]
        public Vector3 Normal;

        public V3N3(Vector3 Position, Vector3 Normal)
        {
            this.Position = Position;
            this.Normal = Normal;
        }
    }
}
