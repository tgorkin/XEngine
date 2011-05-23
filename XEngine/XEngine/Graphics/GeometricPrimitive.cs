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

        private Color m_color = Color.White;

        private bool m_wireframe;

        private VertexBuffer m_vertexBuffer;

        private IndexBuffer m_indexBuffer;

        private BasicEffect m_basicEffect;

        private RasterizerState m_rasterizerState;

        public GeometricPrimitive() { }

        public void Dispose() {
            if ( m_vertexBuffer != null )
                m_vertexBuffer.Dispose();

            if ( m_indexBuffer != null )
                m_indexBuffer.Dispose();

            if ( m_basicEffect != null )
                m_basicEffect.Dispose();
        }

        private int PrimitiveCount {
            get { return m_indices.Count / 3; }
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

        public Color Color {
            set { m_color = value; }
        }

        public bool Wireframe {
            set {
                m_wireframe = value;
            }
        }

        virtual protected void GenerateGeometry() {

        }

        private void InitializeBuffers() {
            GraphicsDevice graphicsDevice = ServiceLocator.Graphics;
            if ( graphicsDevice != null ) {

                // Create a vertex buffer, and copy our vertex data into it.
                m_vertexBuffer = new VertexBuffer( graphicsDevice, VertexPositionNormal.VertexDeclaration, m_vertices.Count, BufferUsage.None );
                m_vertexBuffer.SetData( m_vertices.ToArray() );

                // Create an index buffer, and copy our index data into it.
                m_indexBuffer = new IndexBuffer( graphicsDevice, typeof( ushort ), m_indices.Count, BufferUsage.None );
                m_indexBuffer.SetData( m_indices.ToArray() );
            }
        }

        public void Initialize() {
            InitializeBuffers();
            // Create a BasicEffect, which will be used to render the primitive.
            m_basicEffect = new BasicEffect( ServiceLocator.Graphics );
            m_rasterizerState = new RasterizerState();
            if ( m_wireframe ) {
                m_rasterizerState.FillMode = FillMode.WireFrame;
                m_rasterizerState.CullMode = CullMode.None;
                m_basicEffect.LightingEnabled = false;
            } else {
                m_rasterizerState.FillMode = FillMode.Solid;
                m_basicEffect.EnableDefaultLighting();
            }
        }

        public void Draw( GameTime gameTime, Matrix world ) {
            ICamera camera = ServiceLocator.Camera;

            // Set BasicEffect parameters.
            m_basicEffect.World = world;
            m_basicEffect.View = camera.View;
            m_basicEffect.Projection = camera.Projection;
            m_basicEffect.DiffuseColor = m_color.ToVector3();
            m_basicEffect.Alpha = m_color.A / 255.0f;
            
            GraphicsDevice graphicsDevice = m_basicEffect.GraphicsDevice;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            if ( m_color.A < 255 ) {
                // Set renderstate for alpha blended rendering
                graphicsDevice.BlendState = BlendState.AlphaBlend;
            } else {
                // Set renderstate for opaque rendering
                graphicsDevice.BlendState = BlendState.Opaque;
            }
            // Set our vertex declaration, vertex buffer, and index buffer.
            graphicsDevice.SetVertexBuffer( m_vertexBuffer );
            graphicsDevice.Indices = m_indexBuffer;
            graphicsDevice.RasterizerState = m_rasterizerState;

            foreach ( EffectPass effectPass in m_basicEffect.CurrentTechnique.Passes ) {
                effectPass.Apply();

                graphicsDevice.DrawIndexedPrimitives( PrimitiveType.TriangleList, 0, 0, m_vertices.Count, 0, this.PrimitiveCount );

            }
        }

        static public GeometricPrimitive Factory( GeometricPrimitiveType type, float size ) {
            GeometricPrimitive result = null;
            switch ( type ) {
                case GeometricPrimitiveType.Cube:
                    result = new Cube( size );
                    break;
                case GeometricPrimitiveType.Sphere:
                    result = new Sphere( size, 20 );
                    break;
            }
            result.GenerateGeometry();
            return result;
        }
    }
}
