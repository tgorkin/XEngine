using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class Torus : GeometricPrimitive {

        private static readonly float DEFAULT_DIAMETER = 1.0f;

        private static readonly float DEFAULT_THICKNESS = 0.2f;

        private static readonly int DEFAULT_TESSELATION = 16;

        private float m_diameter;

        private float m_thickness;

        private int m_tessellation;

        public Torus( Game game, Color color, float diameter, float thickness, int tessellation )
            : base( game, color ) {
            m_diameter = diameter;
            m_thickness = thickness;
            m_tessellation = tessellation;
        }

        public Torus( Game game, Color color, float diameter, float thickness )
            : this( game, color, diameter, thickness, DEFAULT_TESSELATION ) {
        }

        public Torus( Game game, Color color )
            : this( game, color, DEFAULT_DIAMETER, DEFAULT_THICKNESS, DEFAULT_TESSELATION ) {
        }

        override protected void GenerateGeometry() {
            if ( m_tessellation < 3 )
                throw new ArgumentOutOfRangeException( "tessellation" );

            // First we loop around the main ring of the torus.
            for ( int i = 0; i < m_tessellation; i++ ) {
                float outerAngle = i * MathHelper.TwoPi / m_tessellation;

                // Create a transform matrix that will align geometry to
                // slice perpendicularly though the current ring position.
                Matrix transform = Matrix.CreateTranslation( m_diameter / 2, 0, 0 ) *
                                   Matrix.CreateRotationY( outerAngle );

                // Now we loop along the other axis, around the side of the tube.
                for ( int j = 0; j < m_tessellation; j++ ) {
                    float innerAngle = j * MathHelper.TwoPi / m_tessellation;

                    float dx = (float)Math.Cos( innerAngle );
                    float dy = (float)Math.Sin( innerAngle );

                    // Create a vertex.
                    Vector3 normal = new Vector3( dx, dy, 0 );
                    Vector3 position = normal * m_thickness / 2;

                    position = Vector3.Transform( position, transform );
                    normal = Vector3.TransformNormal( normal, transform );

                    AddVertex( position, normal );

                    // And create indices for two triangles.
                    int nextI = ( i + 1 ) % m_tessellation;
                    int nextJ = ( j + 1 ) % m_tessellation;

                    AddIndex( i * m_tessellation + j );
                    AddIndex( i * m_tessellation + nextJ );
                    AddIndex( nextI * m_tessellation + j );

                    AddIndex( i * m_tessellation + nextJ );
                    AddIndex( nextI * m_tessellation + nextJ );
                    AddIndex( nextI * m_tessellation + j );
                }
            }
        }

    }
}
