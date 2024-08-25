using System.Numerics;

namespace ModelingToolkit.Core
{
    /*
     * Represents a joint from a model's skeleton.
     */
    public class MtJoint
    {
        public string Name { get; set; }
        public Dictionary<string, string> Metadata { get; set; } // To store additional information
        public MtTransform? AbsoluteTransform { get; set; }
        public MtTransform? RelativeTransform { get; set; }
        public Matrix4x4? AbsoluteTransformationMatrix { get; set; }
        public Matrix4x4? RelativeTransformationMatrix { get; set; }
        public int? ParentId { get; set; }
        public int? Length { get; set; } // Used rarely for leaf bones

        public MtJoint()
        {
            Name = "DEFAULT_NAME";
            Metadata = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            string res = "[" + ParentId + "] ";
            if (AbsoluteTransform != null) res += " Absolute TRS: " + AbsoluteTransform.Translation + " " + AbsoluteTransform.Rotation + " " + AbsoluteTransform.Scale;
            else if (RelativeTransform != null) res += " Relative TRS: " + RelativeTransform.Translation + " " + RelativeTransform.Rotation + " " + RelativeTransform.Scale;
            return res;
        }

        public void Decompose()
        {
            if (AbsoluteTransformationMatrix != null)
            {
                AbsoluteTransform = new MtTransform();
                Matrix4x4.Decompose((Matrix4x4)AbsoluteTransformationMatrix, out Vector3 scaleQ, out Quaternion rotationQ, out Vector3 translationQ);
                AbsoluteTransform.Scale = scaleQ;
                AbsoluteTransform.RotationQ = rotationQ;
                AbsoluteTransform.Translation = translationQ;

                MtTools.DecomposeEuler((Matrix4x4)AbsoluteTransformationMatrix, out Vector3 scale, out Vector3 rotation, out Vector3 translation);
                AbsoluteTransform.Translation = rotation;
            }

            if (RelativeTransformationMatrix != null)
            {
                RelativeTransform = new MtTransform();
                Matrix4x4.Decompose((Matrix4x4)RelativeTransformationMatrix, out Vector3 scaleQ, out Quaternion rotationQ, out Vector3 translationQ);
                RelativeTransform.Scale = scaleQ;
                RelativeTransform.RotationQ = rotationQ;
                RelativeTransform.Translation = translationQ;

                MtTools.DecomposeEuler((Matrix4x4)RelativeTransformationMatrix, out Vector3 scale, out Vector3 rotation, out Vector3 translation);
                RelativeTransform.Rotation = rotation;
            }
        }

        public void Compose(bool preferEuler = false)
        {
            if (AbsoluteTransform.Scale != null && (AbsoluteTransform.Rotation != null || AbsoluteTransform.RotationQ != null) && AbsoluteTransform.Translation != null)
            {
                Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(AbsoluteTransform.Scale);
                Matrix4x4 rotationMatrix;
                if (!preferEuler && AbsoluteTransform.RotationQ != null)
                {
                    rotationMatrix = Matrix4x4.CreateFromQuaternion(AbsoluteTransform.RotationQ);
                }
                else
                {
                    Matrix4x4 rotationMatrixX = Matrix4x4.CreateRotationX(AbsoluteTransform.Rotation.X);
                    Matrix4x4 rotationMatrixY = Matrix4x4.CreateRotationY(AbsoluteTransform.Rotation.Y);
                    Matrix4x4 rotationMatrixZ = Matrix4x4.CreateRotationZ(AbsoluteTransform.Rotation.Z);
                    rotationMatrix = rotationMatrixX * rotationMatrixY * rotationMatrixZ;
                }
                Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(AbsoluteTransform.Translation);

                AbsoluteTransformationMatrix = scaleMatrix * rotationMatrix * translationMatrix;
            }

            if (RelativeTransform.Scale != null && (RelativeTransform.Rotation != null || RelativeTransform.RotationQ != null) && RelativeTransform.Translation != null)
            {
                Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(RelativeTransform.Scale);
                Matrix4x4 rotationMatrix;
                if (!preferEuler && RelativeTransform.RotationQ != null)
                {
                    rotationMatrix = Matrix4x4.CreateFromQuaternion(RelativeTransform.RotationQ);
                }
                else
                {
                    Matrix4x4 rotationMatrixX = Matrix4x4.CreateRotationX(RelativeTransform.Rotation.X);
                    Matrix4x4 rotationMatrixY = Matrix4x4.CreateRotationY(RelativeTransform.Rotation.Y);
                    Matrix4x4 rotationMatrixZ = Matrix4x4.CreateRotationZ(RelativeTransform.Rotation.Z);
                    rotationMatrix = rotationMatrixX * rotationMatrixY * rotationMatrixZ;
                }
                Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(RelativeTransform.Translation);

                RelativeTransformationMatrix = scaleMatrix * rotationMatrix * translationMatrix;
            }
        }
    }
}
