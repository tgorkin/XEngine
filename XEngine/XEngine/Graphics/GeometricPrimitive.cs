using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {

    public enum GeometricPrimitveType {
        Cube, Sphere
    }

    class GeometricPrimitive {

        protected List<VertexPositionNormal> m_vertices = new List<VertexPositionNormal>();

        protected List<ushort> m_indices = new List<ushort>();

        public GeometricPrimitive() {
        }

        public List<VertexPositionNormal> Vertices {
            get { return m_vertices; }
        }

        public List<ushort> Indices {
            get { return m_indices; }
        }

        protected void AddVertex( Vector3 position, Vector3 normal ) {
            m_vertices.Add( new VertexPositionNormal( position, normal ) );
        }

        protected void AddIndex( int index ) {
            if ( index > ushort.MaxValue )
                throw new ArgumentOutOfRangeException( "index" );

            m_indices.Add( (ushort)index );
        }

        protected int CurrentVertex {
            get { return m_vertices.Count; }
        }

        virtual protected void GenerateGeometry() {

        }

        static public GeometricPrimitive Factory( GeometricPrimitveType type ) {
            GeometricPrimitive result = null;
            switch ( type ) {
                case GeometricPrimitveType.Cube:
                    result = new Cube( 1.0f );
                    break;
                case GeometricPrimitveType.Sphere:
                    result = new Sphere( 1.0f, 30 );
                    break;
            }
            result.GenerateGeometry();
            return result;
        }
    }
}
