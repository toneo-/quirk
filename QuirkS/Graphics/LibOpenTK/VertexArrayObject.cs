﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Quirk.Graphics.LibOpenTK
{
    public class VertexArrayObject : IVertexArrayObject
    {
        private int Reference = -1;

        public VertexArrayObject()
        {
            Reference = GL.GenVertexArray();
        }

        ~VertexArrayObject()
        {
            this.Destroy();
        }

        public void Bind()
        {
            GL.BindVertexArray(Reference);

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                throw new QuirkGraphicsException("Unable to bind VertexArrayObject: " + error.ToString());
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public int GetReference()
        {
            return Reference;
        }

        /// <summary>
        /// Immediately deletes and invalidates this VAO.
        /// </summary>
        public void Destroy()
        {
            GL.DeleteVertexArray(Reference);
            Reference = -1;
        }
    }
}
