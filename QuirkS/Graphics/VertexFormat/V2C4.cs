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
        public Vector2 Position;
        public Color4 Color;

        public V2C4(Vector2 Position, Color4 Color)
        {
            this.Position = Position;
            this.Color = Color;
        }

        public int GetStride()
        {
            // 2-float position, 4-float color
            return sizeof(float) * (2 + 4);
        }

        public int GetColorComponents()
        {
            return sizeof(float) * 4;
        }

        public int GetVertexComponents()
        {
            return sizeof(float) * 2;
        }

        public Type GetColorType()
        {
            return typeof(float);
        }

        public Type GetVectorType()
        {
            return typeof(float);
        }
    }

    /*
     * 
    [Serializable]
    unsafe struct SomeVertexFormat
    {
        public Vector2 position;
        public Color4 colour;
    };
     * */
}
