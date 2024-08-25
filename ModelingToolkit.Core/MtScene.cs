using System.Collections.Generic;

namespace ModelingToolkit.Core
{
    /*
     * The main object. Scenes contain a set of models and their position within the scene.
     */
    public class MtScene
    {
        public List<MtModelNode> Models { get; set; }
        public Dictionary<string, string> Metadata { get; set; } // To store additional information

        public MtScene()
        {
            Models = new List<MtModelNode>();
            Metadata = new Dictionary<string, string>();
        }

        public class MtModelNode
        {
            public MtModel Model { get; set; }
            public MtTransform Transform { get; set; }
        }
    }
}
