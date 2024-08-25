using System.Numerics;

namespace ModelingToolkit.Core
{
    /*
     * Represents a transform in 3D space.
     */
    public class MtTransform
    {
        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; } // Radians
        public Quaternion RotationQ { get; set; }
        public Vector3 Translation { get; set; }

        public MtTransform()
        {
            Scale = Vector3.One;
            Rotation = Vector3.Zero;
            RotationQ = Quaternion.Identity;
            Translation = Vector3.Zero;
        }
    }
}
