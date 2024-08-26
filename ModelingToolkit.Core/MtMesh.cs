using System.Numerics;

namespace ModelingToolkit.Core
{
    /*
     * Represents a mesh from a model.
     */
    public class MtMesh
    {
        public string Name { get; set; }
        public Dictionary<string, string> Metadata { get; set; } // To store additional information
        public int? MaterialId { get; set; }
        public List<MtVertex> Vertices { get; set; }
        public List<MtFace> Faces { get; set; }
        public List<MtTriangleStrip> TriangleStrips { get; set; }
        public bool AreFacesDoubleSided { get; set; }

        public MtMesh()
        {
            Name = "DEFAULT_NAME";
            Metadata = new Dictionary<string, string>();
            Vertices = new List<MtVertex>();
            TriangleStrips = new List<MtTriangleStrip>();
            Faces = new List<MtFace>();
        }

        public MtShape GetBoundingBox()
        {
            Vector3 min = new Vector3 (0, 0, 0);
            Vector3 max = new Vector3 (0, 0, 0);

            foreach (MtVertex vertex in Vertices)
            {
                if (min.X > vertex.AbsolutePosition.Value.X) min.X = vertex.AbsolutePosition.Value.X;
                if (min.Y > vertex.AbsolutePosition.Value.Y) min.Y = vertex.AbsolutePosition.Value.Y;
                if (min.Z > vertex.AbsolutePosition.Value.Z) min.Z = vertex.AbsolutePosition.Value.Z;
                if (max.X < vertex.AbsolutePosition.Value.X) max.X = vertex.AbsolutePosition.Value.X;
                if (max.Y < vertex.AbsolutePosition.Value.Y) max.Y = vertex.AbsolutePosition.Value.Y;
                if (max.Z < vertex.AbsolutePosition.Value.Z) max.Z = vertex.AbsolutePosition.Value.Z;
            }

            return MtShape.FromMinMax(MtShape.ShapeType.BOX, min, max);
        }

        public void SetFaceVertices()
        {
            foreach (MtFace face in Faces)
            {
                face.Vertices.Add(Vertices[face.VertexIndices[0]]);
                face.Vertices.Add(Vertices[face.VertexIndices[1]]);
                face.Vertices.Add(Vertices[face.VertexIndices[2]]);
            }
        }

        public void BuildTriangleStrips()
        {
            TriangleStrips.Clear();
            SetFaceVertices();

            for (int i = 0; i < Faces.Count; i++)
            {
                MtFace iFace = Faces[i];

                // If there are no strips, create the first one
                if (TriangleStrips.Count == 0)
                {
                    TriangleStrips.Add(new MtTriangleStrip());
                }
                MtTriangleStrip triStrip = TriangleStrips[TriangleStrips.Count - 1];

                bool faceAdded = triStrip.AddFace(iFace);
                if (faceAdded)
                {
                    continue;
                }
                else
                {
                    TriangleStrips.Add(new MtTriangleStrip());
                    triStrip = TriangleStrips[TriangleStrips.Count - 1];
                    bool faceAdded2 = triStrip.AddFace(iFace);
                    if (!faceAdded2)
                    {
                        throw new Exception("Couldn't add a face to a tri strip. The tri strip algorythm is wrong");
                    }
                }
            }
        }
    }
}
