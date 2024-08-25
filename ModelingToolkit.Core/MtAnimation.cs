namespace ModelingToolkit.Core
{
    /*
     * Represents an animation for a model.
     */
    public class MtAnimation
    {
        public string Name { get; set; }
        public Dictionary<string, string> Metadata { get; set; } // To store additional information
        public MtModel Model { get; set; }
        public List<MtAnimationCurve> Curves { get; set; }
    }
}
