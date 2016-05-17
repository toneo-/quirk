using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.LibOpenTK
{
    public class Texture2D : ITexture
    {
        private int Reference = -1;

        public Texture2D(Bitmap bitmap)
        {
            Reference = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Reference);

            BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                bitmap.Width, bitmap.Height,
                0,
                (BitConverter.IsLittleEndian) ? OpenTK.Graphics.OpenGL.PixelFormat.Bgra :  OpenTK.Graphics.OpenGL.PixelFormat.Rgba,
                PixelType.UnsignedByte,
                bmpData.Scan0
            );

            bitmap.UnlockBits(bmpData);
        }

        public void Bind()
        {
            // todo: change texunit
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, Reference);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
