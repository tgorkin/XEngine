using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class Cylinder : GeometricPrimitive {

        private static readonly float DEFAULT_HEIGHT = 1.0f;

        private static readonly float DEFAULT_DIAMETER = 0.5f;

        private static readonly int DEFAULT_TESSELLATION = 16;

        float m_height;

        float m_diameter;

        int m_tessellation;

        public Cylinder( Game game, Color color, float height, float diameter, int tessellation )
            : base( game, color ) {
                m_height = height;
                m_diameter = diameter;
                m_tessellation = tessellation;
        }

        public Cylinder( Game game, Color color, float height, float diameter )
            : this( game, color, height, diameter, DEFAULT_TESSELLATION ) {

        }

        public Cylinder( Game game, Color color )
            : this( game, color, DEFAULT_HEIGHT, DEFAULT_DIAMETER, DEFAULT_TESSELLATION ) {

        }

        protected override void GenerateGeometry() {
            if ( m_tessellation < 3 )
                throw new ArgumentOutOfRangeException( "tessellation" );

            float halfHeight = m_height / 2;

            float radius = m_diameter / 2;

            // Create a ring of triangles around the outside of the cylinder.
            for ( int i = 0; i < m_tessellation; i++ ) {
                Vector3 normal = GetCircleVector( i, m_tessellation );

                AddVertex( normal * radius + Vector3.Up * halfHeight, normal );
                AddVertex( normal * radius + Vector3.Down * halfHeight, normal );

                AddIndex( i * 2 );
                AddIndex( i * 2 + 1 );
                AddIndex( ( i * 2 + 2 ) % ( m_tessellation * 2 ) );

                AddIndex( i * 2 + 1 );
                AddIndex( ( i * 2 + 3 ) % ( m_tessellation * 2 ) );
                AddIndex( ( i * 2 + 2 ) % ( m_tessellation * 2 ) );
            }

            // Create flat triangle fan caps to seal the top and bottom.
            CreateCap( m_tessellation, halfHeight, radius, Vector3.Up );
            CreateCap( m_tessellation, halfHeight, radius, Vector3.Down );
        }

        /// <summary>
        /// Helper method creates a triangle fan to close the ends of the cylinder.
        /// </summary>
        void CreateCap( int tessellation, float height, float radius, Vector3 normal ) {
            // Create cap indices.
            for ( int i = 0; i < tessellation - 2; i++ ) {
                if ( normal.Y > 0 ) {
                    AddIndex( CurrentVertex );
                    AddIndex( CurrentVertex + ( i + 1 ) % tessellation );
                    AddIndex( CurrentVertex + ( i + 2 ) % tessellation );
                } else {
                    AddIndex( CurrentVertex );
                    AddIndex( CurrentVertex + ( i + 2 ) % tessellation );
                    AddIndex( CurrentVertex + ( i + 1 ) % tessellation );
                }
            }

            // Create cap vertices.
            for ( int i = 0; i < tessellation; i++ ) {
                Vector3 position = GetCircleVector( i, tessellation ) * radius +
                                   normal * height;

                AddVertex( position, normal );
            }
        }


        /// <summary>
        /// Helper method computes a point on a circle.
        /// </summary>
        static Vector3 GetCircleVector( int i, int tessellation ) {
            float angle = i * MathHelper.TwoPi / tessellation;

            float dx = (float)Math.Cos( angle );
            float dz = (float)Math.Sin( angle );

            return new Vector3( dx, 0, dz );
        }
    }
}
