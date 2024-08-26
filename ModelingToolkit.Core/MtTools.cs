using System.Numerics;

namespace ModelingToolkit.Core
{
    /*
     * Various utilities for 3D space.
     */
    public class MtTools
    {
        // From Assimp library
        public static void DecomposeEuler(Matrix4x4 matrix, out Vector3 scale, out Vector3 rotation, out Vector3 position)
        {
            // Extract the translation
            position = new Vector3(matrix.M41, matrix.M42, matrix.M43);

            // Extract the scale
            scale = new Vector3(
                new Vector3(matrix.M11, matrix.M12, matrix.M13).Length(),
                new Vector3(matrix.M21, matrix.M22, matrix.M23).Length(),
                new Vector3(matrix.M31, matrix.M32, matrix.M33).Length()
            );

            // Remove scale from the matrix to isolate the rotation
            Matrix4x4 rotationMatrix = new Matrix4x4(
                matrix.M11 / scale.X, matrix.M12 / scale.X, matrix.M13 / scale.X, 0.0f,
                matrix.M21 / scale.Y, matrix.M22 / scale.Y, matrix.M23 / scale.Y, 0.0f,
                matrix.M31 / scale.Z, matrix.M32 / scale.Z, matrix.M33 / scale.Z, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
            );

            // Use a small epsilon to solve floating-point inaccuracies
            const float epsilon = 1e-6f;

            // Extract the rotation angles from the rotation matrix
            rotation.Y = (float)Math.Asin(-rotationMatrix.M13); // Angle around Y

            float cosY = (float)Math.Cos(rotation.Y);

            if (Math.Abs(cosY) > epsilon)
            {
                // Finding angle around X
                float tanX = rotationMatrix.M33 / cosY; // A
                float tanY = rotationMatrix.M23 / cosY; // B
                rotation.X = (float)Math.Atan2(tanY, tanX);

                // Finding angle around Z
                tanX = rotationMatrix.M11 / cosY; // E
                tanY = rotationMatrix.M12 / cosY; // F
                rotation.Z = (float)Math.Atan2(tanY, tanX);
            }
            else
            {
                // Y is fixed
                rotation.X = 0;

                // Finding angle around Z
                float tanX = rotationMatrix.M22; // E
                float tanY = -rotationMatrix.M21; // F
                rotation.Z = (float)Math.Atan2(tanY, tanX);
            }
        }

        /// From https://stackoverflow.com/a/70462919
        public static Vector3 ToEulerAngles(Quaternion q)
        {
            Vector3 angles = new();

            // roll / x
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
            {
                angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.Y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        // Normalizes the vector so that its values are within [-range, range]
        public static void NormalizeEuler(Vector3 vector, float range)
        {
            NormalizeEuler(vector.X, range);
            NormalizeEuler(vector.Y, range);
            NormalizeEuler(vector.Z, range);
        }
        // Normalizes the value so that its values are within [-range, range]
        public static void NormalizeEuler(float value, float range)
        {
            if (value < -range || value > range) value = value % range;
        }
    }
}
