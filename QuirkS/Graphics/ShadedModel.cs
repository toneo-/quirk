using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quirk.Graphics.Shaders;

//vao = new VertexArrayObject();
//vao.Bind();
//                GL.Enable(EnableCap.DepthTest);
//                GL.Enable(EnableCap.Texture2D);

//                Mesh.Bind();
//                ubo.Bind();

//                shProg.SetupFormat(typeof(V3N3T2));
//                shProg.Use();
//                shProg.BindUniformBlock("BlockyBlock", ubo);

//                tex1.Bind();

//                vao.Unbind();
namespace Quirk.Graphics
{
    public class ShadedModel : IModel
    {
        private IMesh Mesh;
        private IVertexArrayObject VAO;
        private IShaderProgram ShaderProgram;

        public ShadedModel(ILibraryContext LibraryContext, IMesh Mesh, IShaderProgram ShaderProgram)
        {
            this.Mesh = Mesh;
            this.ShaderProgram = ShaderProgram;

            this.VAO = LibraryContext.CreateVertexArrayObject();

            // Prepare the VAO
            VAO.Bind();

            // todo

            VAO.Unbind();
        }

        public void Draw(ILibraryContext Context)
        {
            Mesh.Bind();
            ShaderProgram.Use();

            Mesh.Draw();
        }
    }
}

