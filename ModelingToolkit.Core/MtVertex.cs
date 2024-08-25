using System.Numerics;

namespace ModelingToolkit.Core
{
    /*
     * Represents a vertex from a mesh.
     */
    public class MtVertex
    {
        public Vector3? AbsolutePosition { get; set; }
        public List<MtWeightPosition> Weights { get; set; }
        public Vector3? TextureCoordinates { get; set; } // UV
        public Vector4? ColorRGBA { get; set; } // RGBA
        public Vector3? Normals { get; set; }
        public bool HasWeights { get => Weights != null && Weights.Count > 0; }
        public bool HasTextureCoordinates { get => TextureCoordinates != null; }
        public bool HasColor { get => ColorRGBA != null; }
        public bool HasNormals { get => Normals != null; }

        public MtVertex()
        {
            Weights = new List<MtWeightPosition>();
        }

        public override string ToString()
        {
            string output = AbsolutePosition + "";
            if (TextureCoordinates != null)
            {
                output += " [UV: " + TextureCoordinates.Value.X + ";" + TextureCoordinates.Value.Y + "]";
            }
            return output + " [Weights: " + Weights.Count + "] [HasColor: " + HasColor + "] [HasNormal: " + HasNormals + "]";
        }

        public bool Equals(MtVertex secondVertex)
        {
            if (secondVertex == null)
            {
                return false;
            }

            if (HasWeights)
            {
                if (Weights.Count != secondVertex.Weights.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < Weights.Count; i++)
                    {
                        if (!Weights[i].Equals(secondVertex.Weights[i]))
                        {
                            return false;
                        }
                    }
                }
            }
            else if (AbsolutePosition != secondVertex.AbsolutePosition)
            {
                return false;
            }

            if (TextureCoordinates != secondVertex.TextureCoordinates)
            {
                return false;
            }

            if (ColorRGBA != secondVertex.ColorRGBA)
            {
                return false;
            }

            if (Normals != secondVertex.Normals)
            {
                return false;
            }

            return true;
        }
    }
}
