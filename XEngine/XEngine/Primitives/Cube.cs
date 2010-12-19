using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {

    public class Cube : GeometricPrimitive {

        private static readonly float DEFAULT_SIZE = 1.0f;

        private float m_size;

        public Cube( Game game, Color color, float size )
            : base( game, color ) {
            m_size = size;
        }

        public Cube( Game game, Color color)
            : this(game, color, DEFAULT_SIZE) {
        }

        override protected void GenerateGeometry() {
            // A cube has six faces, each one pointing in a different direction.
            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };

            // Create each face in turn.
            foreach ( Vector3 normal in normals ) {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3( normal.Y, normal.Z, normal.X );
                Vector3 side2 = Vector3.Cross( normal, side1 );

                // Six indices (two triangles) per face.
                AddIndex( CurrentVertex + 0 );
                AddIndex( CurrentVertex + 1 );
                AddIndex( CurrentVertex + 2 );

                AddIndex( CurrentVertex + 0 );
                AddIndex( CurrentVertex + 2 );
                AddIndex( CurrentVertex + 3 );

                // Four vertices per face.
                AddVertex( ( normal - side1 - side2 ) * m_size / 2, normal );
                AddVertex( ( normal - side1 + side2 ) * m_size / 2, normal );
                AddVertex( ( normal + side1 + side2 ) * m_size / 2, normal );
                AddVertex( ( normal + side1 - side2 ) * m_size / 2, normal );
            }
        }
    }
}
