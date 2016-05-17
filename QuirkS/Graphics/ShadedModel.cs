using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quirk.Graphics.Shaders;

namespace Quirk.Graphics
{
    public class ShadedModel : IModel
    {
        private IMesh Mesh;
        private IShaderProgram ShaderProgram;

        public ShadedModel(IMesh Mesh, IShaderProgram ShaderProgram)
        {
            this.Mesh = Mesh;
            this.ShaderProgram = ShaderProgram;
        }

        public void Draw(ILibraryContext Context)
        {
            Mesh.Bind();
            ShaderProgram.Use();

            Mesh.Draw();
        }
    }
}
