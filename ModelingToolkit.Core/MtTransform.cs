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

        public override string ToString()
        {
            return "TRS: " + Translation + " " + Rotation + " " + Scale + " | Q: " + RotationQ;
        }

        // Returns the Transformation Matrix for the MtTransform
        public Matrix4x4 GetTransformationMatrix(bool useEuler = false)
        {
            Matrix4x4 composed = Matrix4x4.CreateTranslation(Translation);
            composed *= Matrix4x4.CreateScale(Scale);

            if (!useEuler)
            {
                Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(RotationQ);
                composed *= rotationMatrix;
            }
            else
            {
                Matrix4x4 rotationMatrix = Matrix4x4.CreateRotationX(Rotation.X);
                rotationMatrix *= Matrix4x4.CreateRotationY(Rotation.Y);
                rotationMatrix *= Matrix4x4.CreateRotationZ(Rotation.Z);
                composed *= rotationMatrix;
            }

            return composed;
        }

        // Normalizes the values of the euler translations to be within [-2 PI, 2 PI]
        public void NormalizeEuler()
        {
            Vector3 tempTranslation = Translation;
            MtTools.NormalizeEuler(tempTranslation, (float)(2 * Math.PI));
            Translation = tempTranslation;
        }
    }
}
