using System.Numerics;

namespace ModelingToolkit.Core
{
    /*
     * Represents a model within the scene.
     */
    public class MtModel
    {
        public string Name { get; set; }
        public Dictionary<string, string> Metadata { get; set; } // To store additional information
        public List<MtJoint> Joints { get; set; }
        public List<MtMesh> Meshes { get; set; }
        public List<MtMaterial> Materials { get; set; }
        public MtShape BoundingBox { get; set; }
        public List<MtAnimation> Animations { get; set; }

        public MtModel()
        {
            Name = "DEFAULT_NAME";
            Metadata = new Dictionary<string, string>();
            Joints = new List<MtJoint>();
            Meshes = new List<MtMesh>();
            Materials = new List<MtMaterial>();
            Animations = new List<MtAnimation>();
        }

        public void CalculateJointAbsoluteMatrices()
        {
            for (int i = 0; i < Joints.Count; i++)
            {
                MtJoint joint = Joints[i];
                Matrix4x4 absoluteMatrix = (Matrix4x4)joint.RelativeTransformationMatrix;
                if (joint.ParentId != null)
                {
                    absoluteMatrix *= (Matrix4x4)Joints[(int)joint.ParentId].AbsoluteTransformationMatrix;
                }

                joint.AbsoluteTransformationMatrix = absoluteMatrix;
                joint.Decompose();
            }
        }

        public void CalculateFromRelativeData()
        {
            for (int i = 0; i < Joints.Count; i++)
            {
                MtJoint joint = Joints[i];

                // Initial parameters
                if (joint.ParentId == null)
                {
                    joint.AbsoluteTransform.Scale = Vector3.One;
                    joint.AbsoluteTransform.Rotation = Vector3.Zero;
                    joint.AbsoluteTransform.RotationQ = Quaternion.Identity;
                    joint.AbsoluteTransform.Translation = Vector3.Zero;
                }
                else
                {
                    MtJoint parentJoint = Joints[joint.ParentId.Value];
                    joint.AbsoluteTransform.Scale = parentJoint.AbsoluteTransform.Scale;
                    joint.AbsoluteTransform.Rotation = parentJoint.AbsoluteTransform.Rotation;
                    joint.AbsoluteTransform.RotationQ = parentJoint.AbsoluteTransform.RotationQ;
                    joint.AbsoluteTransform.Translation = parentJoint.AbsoluteTransform.Translation;
                }

                joint.AbsoluteTransform.Scale = new Vector3(joint.AbsoluteTransform.Scale.X * joint.RelativeTransform.Scale.X,
                                                            joint.AbsoluteTransform.Scale.Y * joint.RelativeTransform.Scale.Y,
                                                            joint.AbsoluteTransform.Scale.Z * joint.RelativeTransform.Scale.Z);

                Vector3 localTranslation = Vector3.Transform(joint.RelativeTransform.Translation, Matrix4x4.CreateFromQuaternion(joint.AbsoluteTransform.RotationQ));
                joint.AbsoluteTransform.Translation += localTranslation;

                var localRotation = Quaternion.Identity;
                if (joint.RelativeTransform.Rotation.Z != 0)
                    localRotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitZ, joint.RelativeTransform.Rotation.Z);
                if (joint.RelativeTransform.Rotation.Y != 0)
                    localRotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, joint.RelativeTransform.Rotation.Y);
                if (joint.RelativeTransform.Rotation.X != 0)
                    localRotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, joint.RelativeTransform.Rotation.X);
                joint.AbsoluteTransform.RotationQ *= localRotation;

                joint.Compose();
            }
        }

        public void BuildBoneDataFromRelativeMatrices()
        {
            for (int i = 0; i < Joints.Count; i++)
            {
                MtJoint joint = Joints[i];
                if (joint.ParentId == null)
                {
                    joint.AbsoluteTransformationMatrix = joint.RelativeTransformationMatrix;
                }
                else
                {
                    joint.AbsoluteTransformationMatrix = joint.RelativeTransformationMatrix * Joints[joint.ParentId.Value].AbsoluteTransformationMatrix;
                }
                joint.Decompose();
            }
        }

        public void GenerateBoundingBox()
        {
            Vector3 min = new Vector3(0, 0, 0);
            Vector3 max = new Vector3(0, 0, 0);

            foreach (MtMesh mesh in Meshes)
            {
                foreach (MtVertex vertex in mesh.Vertices)
                {
                    if (min.X > vertex.AbsolutePosition.Value.X) min.X = vertex.AbsolutePosition.Value.X;
                    if (min.Y > vertex.AbsolutePosition.Value.Y) min.Y = vertex.AbsolutePosition.Value.Y;
                    if (min.Z > vertex.AbsolutePosition.Value.Z) min.Z = vertex.AbsolutePosition.Value.Z;
                    if (max.X < vertex.AbsolutePosition.Value.X) max.X = vertex.AbsolutePosition.Value.X;
                    if (max.Y < vertex.AbsolutePosition.Value.Y) max.Y = vertex.AbsolutePosition.Value.Y;
                    if (max.Z < vertex.AbsolutePosition.Value.Z) max.Z = vertex.AbsolutePosition.Value.Z;
                }
            }

            BoundingBox = MtShape.FromMinMax(MtShape.ShapeType.BOX, min, max);
        }
    }
}
