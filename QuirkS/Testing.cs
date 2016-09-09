using System.IO;

using Quirk.Graphics;
using Quirk.Graphics.LibOpenTK;
using Quirk.Graphics.Loaders;
using Quirk.Graphics.Shaders;
using Quirk.Graphics.Shaders.LibOpenTK;

namespace Quirk
{
    partial class Program
    {
        private static IRenderer activeRenderer = new RendererTK();
        private static IModel testModel;
        private static ILibraryContext libContext = new OpenTKLibraryContext();

        private static void SetupTest()
        {
            OBJLoader meshLoader = new OBJLoader(true);
            const string TEST_MODEL_PATH = @"E:\Projects\Data\Models\CylonRaider.obj";
            const string TEST_VERT_SHADER = @"Resources/Shaders/Test/test_vert_V3N3T2.glsl";
            const string TEST_FRAG_SHADER = @"Resources/Shaders/Test/test_frag_V3N3T2.glsl";

            IMesh modelMesh;
            IShader vertexShader, fragmentShader;
            IShaderProgram shaderProgram;

            // Build a mesh for the model first
            modelMesh = meshLoader.LoadMeshFromStream(libContext, File.OpenRead(TEST_MODEL_PATH));

            // Load a shader program for it to use ...
            vertexShader = new VertexShader(File.ReadAllText(TEST_VERT_SHADER));
            fragmentShader = new FragmentShader(File.ReadAllText(TEST_FRAG_SHADER));

            shaderProgram = new ShaderProgram();
            shaderProgram.Attach(vertexShader);
            shaderProgram.Attach(fragmentShader);
        }

        private static void RenderTest()
        {

        }
    }
}