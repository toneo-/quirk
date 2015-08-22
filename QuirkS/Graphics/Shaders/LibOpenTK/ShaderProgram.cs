using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Quirk.Graphics.VertexFormat;

namespace Quirk.Graphics.Shaders.LibOpenTK
{
    class ShaderProgram : IShaderProgram
    {
        protected int Reference = -1;
        protected List<IShader> LinkedShaders = new List<IShader>();

        public ShaderProgram()
        {
            Reference = GL.CreateProgram();

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to generate shader program! " + error.ToString());
        }

        public void Link()
        {
            GL.LinkProgram(Reference);

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to link shader program! " + error.ToString());
        }

        public void Unlink()
        {
            GL.LinkProgram(0);
        }

        public void Use()
        {
            GL.UseProgram(Reference);

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                throw new QuirkGraphicsException("Unable to use shader program! " + error.ToString());
        }

        public int GetReference()
        {
            return Reference;
        }

        public void Destroy()
        {
            foreach (IShader shader in LinkedShaders)
            {
                this.Detach(shader);
                shader.Destroy();
            }

            GL.DeleteProgram(Reference);

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                throw new QuirkGraphicsException("Failed to destroy shader program! " + error.ToString());

            Reference = -1;
        }

        public void Attach(IShader Shader)
        {
            GL.AttachShader(Reference, Shader.GetReference());
            ErrorCode e = GL.GetError();
            LinkedShaders.Add(Shader);
        }

        public void Detach(IShader Shader)
        {
            GL.DetachShader(Reference, Shader.GetReference());
            LinkedShaders.Remove(Shader);
        }

        public void SetUniform(string Name, ref Vector2 Value)
        {
            int Location = GetUniformLocation(Name);
            GL.Uniform2(Location, ref Value);
        }

        public void SetUniform(string Name, ref Matrix4 Value)
        {
            int Location = GetUniformLocation(Name);
            GL.UniformMatrix4(Location, false, ref Value);
        }

        private int GetUniformLocation(string Name)
        {
            int Location = GL.GetUniformLocation(Reference, Name);
            if (Location == -1)
                throw new QuirkGraphicsException("Uniform '" + Name + "' could not be found in this shader program.");

            return Location;
        }

        public void SetupFormat(Type VertexFormat)
        {
            this.Link();

            // Ensure this implements IVertexFormat
            if (!VertexFormat.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IVertexFormat)))
                throw new ArgumentException("The given Type (" + VertexFormat.ToString() + ") does not implement IVertexFormat.");

            // Look through the fields within the format for ShaderLinkAttributes
            FieldInfo[] fieldInfos = VertexFormat.GetFields();
            List<ShaderLinkAttribute> ShaderLinks = new List<ShaderLinkAttribute>();

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                // Get all relevant Attributes, convert to ShaderLinkAttributes
                List<Attribute> rawLinkAttributes = fieldInfo.GetCustomAttributes().Where(x => x is ShaderLinkAttribute).ToList();

                // Can only be one ShaderLink per field
                if (rawLinkAttributes.Count > 1)
                    throw new ArgumentException("Format field '" + fieldInfo.Name + "' has more than one ShaderLinkAttribute.");
                else if (rawLinkAttributes.Count == 1)
                {
                    ShaderLinkAttribute Link = (ShaderLinkAttribute)rawLinkAttributes[0];
                    ShaderLinks.Add(Link);
                }
            }

            // Add them all
            SetupShaderLinks(ShaderLinks);
        }

        /// <summary>
        /// Translates a Type into a VertexAttribPointerType, which is understood by OpenTK.
        /// </summary>
        /// <param name="ComponentType">The type to translate.</param>
        /// <returns>The VertexAttribPointerType related to the given Type (i.e. Single -> VertexAttribPointerType.Float)</returns>
        private VertexAttribPointerType TranslateComponentType(Type ComponentType)
        {
            VertexAttribPointerType pointerType;

            // Can't use a switch statement here
            if (ComponentType == typeof(byte))
                pointerType = VertexAttribPointerType.Byte;
            else if (ComponentType == typeof(short))
                pointerType = VertexAttribPointerType.Short;
            else if (ComponentType == typeof(int))
                pointerType = VertexAttribPointerType.Int;
            else if (ComponentType == typeof(float))
                pointerType = VertexAttribPointerType.Float;
            else
                throw new NotImplementedException("Component type '" + ComponentType.ToString() + "' is unsupported.");

            return pointerType;
        }

        /// <summary>
        /// Sets up every shader link in the given list.
        /// This comprises one or more calls to SetupShaderLink along with offset and stride calculation.
        /// </summary>
        /// <param name="ShaderLinks">The shader links to add.</param>
        private void SetupShaderLinks(List<ShaderLinkAttribute> ShaderLinks)
        {
            int offset = 0;
            int stride = 0;

            // Calculate stride
            foreach (ShaderLinkAttribute link in ShaderLinks)
                stride += link.ComponentSize * link.ComponentCount;

            // Add each link & calculate offsets
            foreach (ShaderLinkAttribute link in ShaderLinks)
            {
                SetupShaderLink(link, offset, stride);
                offset += link.ComponentSize * link.ComponentCount;
            }
        }

        /// <summary>
        /// Sets up a single shader link, associating a specific section of the vertex format with a variable in this shader program.
        /// </summary>
        /// <param name="ShaderLink">The shader link itself.</param>
        /// <param name="Offset">The offset of this variable within the vertex format.</param>
        /// <param name="Stride">The total size of the vertex format.</param>
        private void SetupShaderLink(ShaderLinkAttribute ShaderLink, int Offset, int Stride)
        {
            string linkedName = ShaderLink.LinkedName;
            int componentCount = ShaderLink.ComponentCount;

            // Find the variable in the Shader Program
            int variableIndex = GL.GetAttribLocation(Reference, linkedName);
            if (variableIndex == -1)
                throw new QuirkGraphicsException("Variable name '" + linkedName + "' was not found or used reserved prefix 'gl_'.");

            // Make type usable by OpenTK
            VertexAttribPointerType pointerType = TranslateComponentType(ShaderLink.ComponentType);

            // Tell OpenGL where this variable is within vertex array data
            GL.VertexAttribPointer(variableIndex, ShaderLink.ComponentCount, pointerType, ShaderLink.Normalised, Stride, Offset);
        }
    }
}
