using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XEngine {
    class PrimitiveRenderComponent : BaseComponent {

        private Color m_color;

        private PositionAttribute m_position;

        private GeometricPrimitive m_primitive;

        private int m_vertexCount;

        private int m_indexCount;

        private VertexBuffer m_vertexBuffer;

        private IndexBuffer m_indexBuffer;

        private BasicEffect m_basicEffect;

        private RasterizerState m_rasterizerState;

        public PrimitiveRenderComponent( Entity entity, GeometricPrimitveType type, Color color )
            : base( entity ) {
            m_primitive = GeometricPrimitive.Factory( type );
            InitializeBuffers();
            m_color = color;
            m_position = entity.getAttribute( Attributes.POSITION ) as PositionAttribute;
            m_basicEffect = new BasicEffect( ServiceLocator.Graphics );
            m_rasterizerState = new RasterizerState();
        }

        ~PrimitiveRenderComponent() {
            Dispose();
        }

        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        private void Dispose() {
            if ( m_vertexBuffer != null )
                m_vertexBuffer.Dispose();

            if ( m_indexBuffer != null )
                m_indexBuffer.Dispose();

            if ( m_basicEffect != null )
                m_basicEffect.Dispose();
        }

        public bool Wireframe {
            set {
                if ( value ) {
                    m_rasterizerState.FillMode = FillMode.WireFrame;
                    m_rasterizerState.CullMode = CullMode.None;
                } else {
                    m_rasterizerState.FillMode = FillMode.Solid;
                }
            }
        }

        override public void Draw( GameTime gameTime ) {
            ICamera camera = ServiceLocator.Camera;

            // Set BasicEffect parameters.
            m_basicEffect.World = m_position.World;
            m_basicEffect.View = camera.View;
            m_basicEffect.Projection = camera.Projection;
            m_basicEffect.DiffuseColor = m_color.ToVector3();
            m_basicEffect.Alpha = m_color.A / 255.0f;
            m_basicEffect.EnableDefaultLighting();

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

                int primitiveCount = m_indexCount / 3;

                graphicsDevice.DrawIndexedPrimitives( PrimitiveType.TriangleList, 0, 0, m_vertexCount, 0, primitiveCount );

            }
        }

        private void InitializeBuffers() {
            GraphicsDevice graphicsDevice = ServiceLocator.Graphics;
            if ( graphicsDevice != null ) {

                // Create a vertex buffer, and copy our vertex data into it.
                List<VertexPositionNormal> vertices = m_primitive.Vertices;
                m_vertexCount = vertices.Count;
                m_vertexBuffer = new VertexBuffer( graphicsDevice, typeof( VertexPositionNormal ), m_vertexCount, BufferUsage.None );
                m_vertexBuffer.SetData( m_primitive.Vertices.ToArray() );

                // Create an index buffer, and copy our index data into it.
                List<ushort> indices = m_primitive.Indices;
                m_indexCount = indices.Count;
                m_indexBuffer = new IndexBuffer( graphicsDevice, typeof( ushort ), m_indexCount, BufferUsage.None );
                m_indexBuffer.SetData( indices.ToArray() );

                // Create a BasicEffect, which will be used to render the primitive.
                m_basicEffect = new BasicEffect( graphicsDevice );
            }
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Entity entity = null;
            testGame.InitDelegate = delegate {
                entity = new Entity();
                AddSphereTestComponent( entity );
                AddCubeTestComponent( entity );
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity.Draw( gameTime );
            };
            testGame.Run();
        }

        static public void AddSphereTestComponent( Entity entity ) {
            PositionAttribute position = new PositionAttribute();
            entity.addAttribute( Attributes.POSITION, position );

            PrimitiveRenderComponent renderComponent = new PrimitiveRenderComponent( entity, GeometricPrimitveType.Sphere, Color.Green );
            renderComponent.Wireframe = false;
            entity.AddComponent( renderComponent );
        }

        static public void AddCubeTestComponent( Entity entity ) {
            PositionAttribute position = new PositionAttribute();
            position.Position = new Vector3( 1.0f, 0, 0 );
            entity.addAttribute( Attributes.POSITION, position );

            PrimitiveRenderComponent renderComponent = new PrimitiveRenderComponent( entity, GeometricPrimitveType.Cube, Color.LightBlue );
            renderComponent.Wireframe = true;
            entity.AddComponent( renderComponent );
        }
    }
}
