using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XEngineTypes;

namespace XEngine {
    class PrimitiveRenderComponent : BaseComponent {

        private GeometricPrimitiveType m_primitiveType;

        private Color m_color = Color.White;

        private bool m_wireframe;

        private EntityAttribute<Transform> m_transform;

        private GeometricPrimitive m_primitive;

        private VertexBuffer m_vertexBuffer;

        private IndexBuffer m_indexBuffer;

        private BasicEffect m_basicEffect;

        private RasterizerState m_rasterizerState;

        public PrimitiveRenderComponent( Entity entity)
            : base( entity ) {
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


        public Color Color {
            set { m_color = value; }
        }

        public GeometricPrimitiveType GeometricPrimitiveType {
            set { m_primitiveType = value; }
        }

        public bool Wireframe {
            set {
                m_wireframe = value;
                ToggleWireframe( m_wireframe );
            }
        }

        private void ToggleWireframe(bool wireframeOn) {
            if ( m_rasterizerState != null ) {
                if ( wireframeOn ) {
                    m_rasterizerState.FillMode = FillMode.WireFrame;
                    m_rasterizerState.CullMode = CullMode.None;
                } else {
                    m_rasterizerState.FillMode = FillMode.Solid;
                }
            }
        }

        override public void LoadData( ComponentData componentData ) {
            PrimitiveRenderData data = componentData as PrimitiveRenderData;
            if ( data != null ) {
                this.GeometricPrimitiveType = data.PrimitiveType;
                this.Color = data.Color;
                this.Wireframe = data.Wireframe;
            }
        }

        override public void Initialize() {
            m_primitive = GeometricPrimitive.Factory( this.m_primitiveType );
            InitializeBuffers();
            m_transform = this.Entity.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform>;
            m_basicEffect = new BasicEffect( ServiceLocator.Graphics );
            m_rasterizerState = new RasterizerState();
            ToggleWireframe( m_wireframe );
        }

        override public void Draw( GameTime gameTime ) {
            ICamera camera = ServiceLocator.Camera;

            Matrix world;
            if ( m_transform != null ) {
                world = m_transform.Value.World;
            } else {
                world = Matrix.Identity;
            }

            // Set BasicEffect parameters.
            m_basicEffect.World = world;
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

                graphicsDevice.DrawIndexedPrimitives( PrimitiveType.TriangleList, 0, 0, m_primitive.VertexCount, 0, m_primitive.PrimitiveCount );

            }
        }

        private void InitializeBuffers() {
            GraphicsDevice graphicsDevice = ServiceLocator.Graphics;
            if ( graphicsDevice != null && m_primitive != null) {

                // Create a vertex buffer, and copy our vertex data into it.
                m_vertexBuffer = new VertexBuffer( graphicsDevice, VertexPositionNormal.VertexDeclaration, m_primitive.VertexCount, BufferUsage.None );
                m_vertexBuffer.SetData( m_primitive.Vertices.ToArray() );

                // Create an index buffer, and copy our index data into it.
                m_indexBuffer = new IndexBuffer( graphicsDevice, typeof( ushort ), m_primitive.IndexCount, BufferUsage.None );
                m_indexBuffer.SetData( m_primitive.Indices.ToArray() );

                // Create a BasicEffect, which will be used to render the primitive.
                m_basicEffect = new BasicEffect( graphicsDevice );
            }
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Entity entity1 = null;
            Entity entity2 = null;
            testGame.InitDelegate = delegate {
                entity1 = new Entity();
                AddSphereTestComponent( entity1 );
                entity1.Initialize();

                entity2 = new Entity();
                AddCubeTestComponent( entity2 );
                entity2.Initialize();
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity1.Draw( gameTime );
                entity2.Draw( gameTime );
            };
            testGame.Run();
        }

        static public void AddSphereTestComponent( Entity entity ) {
            EntityAttribute<Transform> transform = new EntityAttribute<Transform>();
            transform.Value = new Transform();
            entity.AddAttribute( Attributes.TRANSFORM, transform );

            PrimitiveRenderComponent renderComponent = new PrimitiveRenderComponent( entity );
            renderComponent.GeometricPrimitiveType = GeometricPrimitiveType.Sphere;
            renderComponent.Color = Color.Green;
            renderComponent.Wireframe = false;
            entity.AddComponent( renderComponent );
        }

        static public void AddCubeTestComponent( Entity entity ) {
            EntityAttribute<Transform> transform = new EntityAttribute<Transform>();
            transform.Value = new Transform();
            transform.Value.Position = new Vector3( 1.0f, 0, 0 );
            entity.AddAttribute( Attributes.TRANSFORM, transform );

            PrimitiveRenderComponent renderComponent = new PrimitiveRenderComponent( entity );
            renderComponent.GeometricPrimitiveType = GeometricPrimitiveType.Cube;
            renderComponent.Color = Color.LightBlue;
            renderComponent.Wireframe = true;
            entity.AddComponent( renderComponent );
        }
    }
}
