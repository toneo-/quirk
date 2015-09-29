using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using OpenTK;
using Quirk.Graphics.LibOpenTK;
using Quirk.Graphics.VertexFormat;

namespace Quirk.Graphics.Loaders
{
    public class OBJLoader
    {
        private bool SwapYZ = false;

        public OBJLoader(bool SwapYZ)
        {
            this.SwapYZ = SwapYZ;
        }
        

        public void LoadFromStream(Stream DataStream, out V3N3T2[] Vertices, out int[] Indices)
        {
            string[] Lines = GetAllLines(DataStream);
            this.LoadVINUV(Lines, out Vertices, out Indices);

            DataStream.Dispose();
            DataStream.Close();
        }

        private string[] GetAllLines(Stream DataStream)
        {
            // Files above ~2.1GB are 'too large'
            if (DataStream.Length > int.MaxValue)
                throw new InvalidDataException("Data stream is too large (max " + int.MaxValue + " bytes)");

            int fileLength = (int)DataStream.Length;

            byte[] entireFile = new byte[fileLength];
            DataStream.Read(entireFile, 0, fileLength);

            // Split file rawLine-by-rawLine and return
            string[] lines = Encoding.UTF8.GetString(entireFile)
                .Replace('\r', '\n')
                .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return lines;
        }

        private void LoadVINUV(string[] Lines, out V3N3T2[] Vertices, out int[] Indices) //out Vector3[] Vertices, out int[] Indices, out Vector3[] Normals, out Vector2[] TextureCoords)
        {
            List<Vector3> workingPositions = new List<Vector3>();
            List<Vector3> workingNormals = new List<Vector3>();
            List<Vector2> workingUV = new List<Vector2>();
            List<int> workingIndices;

            List<string[]> faceDefinitions = new List<string[]>();
            
            foreach (string rawLine in Lines)
            {
                ReadLine(rawLine.Trim(),
                    workingPositions,
                    workingNormals,
                    workingUV,
                    faceDefinitions);
            }

            // Process face definitions
            ProcessFaces(faceDefinitions, workingPositions, workingNormals, workingUV, 
                out Vertices, out Indices);
        }

        private void ProcessFaces(List<string[]> FaceDefinitions, List<Vector3> Positions, List<Vector3> Normals, List<Vector2> UV, out V3N3T2[] Vertices, out int[] Indices)
        {
            List<V3N3T2> finalVertices = new List<V3N3T2>();
            List<int> indices = new List<int>();

            // Used to determine what already exists
            Dictionary<V3N3T2, int> existingVertices = new Dictionary<V3N3T2, int>();

            char[] vertexPartSplit = new char[]{ '/' };

            // For each face, generate vertices with the relevant position, normal, UV
            for (int faceID = 0; faceID < FaceDefinitions.Count; faceID++)
            {
                string[] faceDefinition = FaceDefinitions[faceID];
                
                if (faceDefinition.Length != 3 && faceDefinition.Length != 4)
                    throw new InvalidDataException("Face definition must contain either 3 or 4 vertices.");

                // For each vertex in the face
                for (int vertexNo = 0; vertexNo < faceDefinition.Length; vertexNo++)
                {
                    int positionIndex = -1;
                    int normalIndex = -1;
                    int UVIndex = -1;

                    // Seperate face part - 3/4/5 -> 3, 4, 5
                    string[] vertexParts = faceDefinition[vertexNo].Split(vertexPartSplit, StringSplitOptions.None);

                    // Valid format is one of V, V/VT, V/VT/VN or V//VN
                    if (vertexParts.Length > 3)
                        throw new InvalidDataException("A vertex within a face definition may only contain up to three parts.");

                    positionIndex = int.Parse(vertexParts[0]);

                    if (vertexParts.Length > 1)
                    {
                        // Load texture coordinate if present
                        if (vertexParts[1].Length > 0)
                            UVIndex = int.Parse(vertexParts[1]);

                        // Load normal if present
                        if (vertexParts.Length == 3)
                            normalIndex = int.Parse(vertexParts[2]);
                    }

                    // Assemble
                    V3N3T2 Vertex = new V3N3T2(Positions[positionIndex - 1], 
                        ((normalIndex == -1) ? Vector3.Zero : Normals[normalIndex - 1]), 
                        ((UVIndex == -1) ? Vector2.Zero : UV[UVIndex - 1])
                        );

                    // If the vertex already exists, reference it
                    if (existingVertices.ContainsKey(Vertex))
                        indices.Add(existingVertices[Vertex]);
                    else
                    {
                        // Doesn't exist - add as new vertex
                        finalVertices.Add(Vertex);
                        indices.Add(existingVertices.Count);

                        // Remember what already exists
                        existingVertices[Vertex] = existingVertices.Count;
                    }
                }
            }

            // Output
            Vertices = finalVertices.ToArray();
            Indices = indices.ToArray();
        }

        private void ReadLine(string RawLine, List<Vector3> Positions, List<Vector3> Normals, List<Vector2> UV, List<string[]> FaceDefinitions)
        {
            // Ignore pure comments
            if (RawLine.StartsWith("#")) return;

            // Allow comments part-way through a rawLine
            string line = StripComment(RawLine).Trim();

            // Split line, e.g. v 1.0000 3.000 -2.000
            char[] spaceSplit = new char[] { ' ' };

            string[] lineParts = line.Split(spaceSplit, StringSplitOptions.RemoveEmptyEntries);
            string lineType = lineParts[0];

            switch (lineType)
            {
                case "v":
                    if (lineParts.Length < 4)
                        throw new ArgumentException("Failed to read vertex position; X, Y and Z must be present.");

                    Positions.Add(ReadVector3(lineParts));
                    break;

                case "vn":
                    if (lineParts.Length < 4)
                        throw new ArgumentException("Failed to read vertex normal; X, Y and Z must be present.");

                    Normals.Add(ReadVector3(lineParts));
                    break;

                case "vt":
                    if (lineParts.Length < 3)
                        throw new ArgumentException("Failed to read texture coordinates; U and V must be present.");

                    UV.Add(ReadVector2(lineParts));
                    break;

                case "f":
                    // Triangle
                    if (lineParts.Length == 4)
                        FaceDefinitions.Add(new string[] { lineParts[1], lineParts[2], lineParts[3] });
                    // Quad
                    else if (lineParts.Length == 5)
                        FaceDefinitions.Add(new string[] { lineParts[1], lineParts[2], lineParts[3], lineParts[4] });
                    else
                        throw new InvalidDataException("Failed to read face definition; Must define a triangle or quad");

                    break;

                default:
                    // Invalid lines are ignored.
                    break;
            }
        }

        private Vector2 ReadVector2(string[] LineParts)
        {

            // LineParts[] -> v 0.004 -1.0677711
                return new Vector2(
                    float.Parse(LineParts[1]),
                    float.Parse(LineParts[2]));
        }

        private Vector3 ReadVector3(string[] LineParts)
        {
            // LineParts[] -> v 0.004 -1.0677711 4.8817
            // w coordinate may be present but can be ignored.

            if (LineParts.Length < 4)
                throw new ArgumentException("Cannot read Vector3: There must be three coordinates.");

            if (!SwapYZ)
            {
                return new Vector3(
                    float.Parse(LineParts[1]),
                    float.Parse(LineParts[2]),
                    float.Parse(LineParts[3]));
            }
            else
            {
                return new Vector3(
                    float.Parse(LineParts[1]),
                    float.Parse(LineParts[3]),
                    float.Parse(LineParts[2]));
            }
        }

        /// <summary>
        /// Looks for and strips a comment from a line if it exists.
        /// Example: v 0.04 0.1 2.3 # comment blah blah
        /// </summary>
        private string StripComment(string Line)
        {
            int index = Line.IndexOf('#');
            if (index != -1)
                return Line.Substring(0, index);
            else return Line;
        }
    }
}
