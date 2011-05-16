using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XEngineTypes;

namespace XEngine {

    class GeometricPrimitive {

        protected List<VertexPositionNormal> m_vertices = new List<VertexPositionNormal>();

        protected List<ushort> m_indices = new List<ushort>();

        public GeometricPrimitive() {
        }

        public List<VertexPositionNormal> Vertices {
            get { return m_vertices; }
        }

        public int VertexCount {
            get { return m_vertices.Count; }
        }

        public List<ushort> Indices {
            get { return m_indices; }
        }

        public int IndexCount {
            get { return m_indices.Count; }
        }

        public int PrimitiveCount {
            get { return IndexCount / 3; }
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

        static public GeometricPrimitive Factory( GeometricPrimitiveType type ) {
            GeometricPrimitive result = null;
            switch ( type ) {
                case GeometricPrimitiveType.Cube:
                    result = new Cube( 1.0f );
                    break;
                case GeometricPrimitiveType.Sphere:
                    result = new Sphere( 1.0f, 30 );
                    break;
            }
            result.GenerateGeometry();
            return result;
        }
    }
}
