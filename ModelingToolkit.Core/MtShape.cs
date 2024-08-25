using System.Numerics;

namespace ModelingToolkit.Core
{
    /*
     * Represents a shape in 3D space.
     */
    public class MtShape
    {
        public string Name { get; set; }
        public Dictionary<string, string> Metadata { get; set; } // To store additional information
        public ShapeType Type { get; set; }
        public Vector3 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }

        public MtShape()
        {
            Metadata = new Dictionary<string, string>();
        }

        public enum ShapeType
        {
            UNDEFINED,
            BOX,
            COLUMN,
            ELLIPSOID
        }

        public static MtShape CreateBox(Vector3 position, float width, float height, float depth)
        {
            MtShape shape = new MtShape();
            shape.Name = "DEFAULT_BOX";
            shape.Type = ShapeType.BOX;
            shape.Position = position;
            shape.Height = height;
            shape.Depth = depth;

            return shape;
        }

        public static MtShape CreateCube(Vector3 position, float size)
        {
            MtShape shape = new MtShape();
            shape.Name = "DEFAULT_CUBE";
            shape.Type = ShapeType.BOX;
            shape.Position = position;
            shape.Width = size;
            shape.Height = size;
            shape.Depth = size;

            return shape;
        }

        public static MtShape CreateColumn(Vector3 position, float diameter, float height)
        {
            MtShape shape = new MtShape();
            shape.Name = "DEFAULT_COLUMN";
            shape.Type = ShapeType.COLUMN;
            shape.Position = position;
            shape.Width = diameter;
            shape.Height = height;
            shape.Depth = diameter;

            return shape;
        }

        public static MtShape CreateEllipsoid(Vector3 position, float width, float height, float depth)
        {
            MtShape shape = new MtShape();
            shape.Name = "DEFAULT_ELLIPSOID";
            shape.Type = ShapeType.ELLIPSOID;
            shape.Position = position;
            shape.Height = height;
            shape.Width = width;
            shape.Depth = depth;

            return shape;
        }

        public static MtShape CreateSphere(Vector3 position, float diameter)
        {
            MtShape shape = new MtShape();
            shape.Name = "DEFAULT_SPHERE";
            shape.Type = ShapeType.ELLIPSOID;
            shape.Position = position;
            shape.Width = diameter;
            shape.Height = diameter;
            shape.Depth = diameter;

            return shape;
        }

        public void ToMinMax(out Vector3 min, out Vector3 max)
        {
            min = new Vector3(
                Position.X - Width / 2,
                Position.Y - Height / 2,
                Position.Z - Depth / 2
            );

            max = new Vector3(
                Position.X + Width / 2,
                Position.Y + Height / 2,
                Position.Z + Depth / 2
            );
        }

        public static MtShape FromMinMax(ShapeType shapeType, Vector3 min, Vector3 max)
        {
            MtShape shape = new MtShape
            {
                Name = "DEFAULT_SHAPE",
                Type = shapeType,
                Position = new Vector3(
                    (min.X + max.X) / 2,
                    (min.Y + max.Y) / 2,
                    (min.Z + max.Z) / 2
                ),
                Width = max.X - min.X,
                Height = max.Y - min.Y,
                Depth = max.Z - min.Z
            };

            return shape;
        }
    }
}
