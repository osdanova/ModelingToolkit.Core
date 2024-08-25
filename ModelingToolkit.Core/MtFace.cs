namespace ModelingToolkit.Core
{
    /*
     * Represents a face from a mesh.
     */
    public class MtFace
    {
        public List<int> VertexIndices { get; set; }
        public List<MtVertex> Vertices { get; set; } // Used for TriStrips
        public bool Clockwise { get; set; }
        public bool DoubleSided { get; set; }

        public MtFace()
        {
            VertexIndices = new List<int>();
            Vertices = new List<MtVertex>();
            Clockwise = true;
            DoubleSided = false;
        }

        public override string ToString()
        {
            string output = "[";
            foreach (int i in VertexIndices) { output += " " + i; }
            output += " ]";
            output += " Clockwise: " + Clockwise;
            return output + "; DoubleSided: " + DoubleSided;
        }
    }
}
