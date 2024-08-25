namespace ModelingToolkit.Core
{
    /*
     * Represents a key from an animation curve.
     */
    public class MtAnimationKey
    {
        public float Time { get; set; }
        public float Value { get; set; }
        public MtInterpolationMode InterpolationMode { get; set; } // For the next key

        public enum MtInterpolationMode
        {
            BEZIER,
            LINEAR,
            CONSTANT
        }
    }
}
