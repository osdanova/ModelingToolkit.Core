namespace ModelingToolkit.Core
{
    /*
     * Represents a curve from an animation.
     */
    public class MtAnimationCurve
    {
        public MtJoint Joint { get; set; }
        public ChannelType Channel { get; set; }
        public List<MtAnimationKey> Keys { get; set; }

        public MtAnimationCurve()
        {
            Keys = new List<MtAnimationKey>();
        }


        public enum ChannelType
        {
            SCALE_X,
            SCALE_Y,
            SCALE_Z,
            ROTATION_X,
            ROTATION_Y,
            ROTATION_Z,
            TRANSLATION_X,
            TRANSLATION_Y,
            TRANSLATION_Z,
        }
    }
}
