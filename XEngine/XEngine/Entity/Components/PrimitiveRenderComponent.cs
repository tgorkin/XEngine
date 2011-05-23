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

        private Transform m_transform;

        private GeometricPrimitive m_primitive;

        private Color m_color;

        private bool m_wireframe;

        private float m_size = 1.0f;

        public PrimitiveRenderComponent( Entity entity)
            : base( entity ) {
        }

        ~PrimitiveRenderComponent() {
            Dispose();
        }

        private void Dispose() {
            m_primitive.Dispose();
        }

        public GeometricPrimitiveType GeometricPrimitiveType {
            set { m_primitiveType = value; }
        }

        public Color Color {
            set { 
                m_color = value;
                if ( m_primitive != null ) {
                    m_primitive.Color = m_color;
                }
            }
        }

        public bool Wireframe {
            set {
                m_wireframe = value;
                if ( m_primitive != null ) {
                    m_primitive.Color = m_color;
                }
            }
        }

        override public void LoadData( ComponentData componentData ) {
            PrimitiveRenderData data = componentData as PrimitiveRenderData;
            if ( data != null ) {
                this.GeometricPrimitiveType = data.PrimitiveType;
                this.m_color = data.Color;
                this.Wireframe = data.Wireframe;
                this.m_size = data.Size;
            }
        }

        override public void Initialize() {
            m_primitive = GeometricPrimitive.Factory( this.m_primitiveType, m_size );
            m_transform = this.Entity.GetAttribute<Transform>( Attributes.TRANSFORM );
            m_primitive.Color = m_color;
            m_primitive.Wireframe = m_wireframe;
            m_primitive.Initialize();
        }

        override public void Draw( GameTime gameTime ) {
            Matrix world = Matrix.Identity;
            if ( m_transform != null ) {
                world = m_transform.World;
            }
            m_primitive.Draw( gameTime, world );
        }



        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Entity entity1 = null;
            Entity entity2 = null;
            testGame.InitDelegate = delegate {
                entity1 = new Entity();
                AddTestComponent( entity1, GeometricPrimitiveType.Sphere, 2.0f );
                entity1.Initialize();

                entity2 = new Entity();
                AddTestComponent( entity2, GeometricPrimitiveType.Cube, 1.0f );
                Transform transform = entity2.GetAttribute<Transform>( Attributes.TRANSFORM );
                transform.Position = new Vector3( 5.0f, 0, 0 );
                transform.UpdateWorld(null);
                entity2.Initialize();
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity1.Draw( gameTime );
                entity2.Draw( gameTime );
            };
            testGame.Run();
        }

        static public void AddTestComponent( Entity entity, GeometricPrimitiveType primitiveType, float size ) {
            Transform transform = new Transform();
            entity.AddAttribute( Attributes.TRANSFORM, transform );

            PrimitiveRenderComponent renderComponent = new PrimitiveRenderComponent( entity );
            renderComponent.GeometricPrimitiveType = primitiveType;
            renderComponent.Color = Color.LimeGreen;
            renderComponent.Wireframe = false;
            renderComponent.m_size = size;
            entity.AddComponent( renderComponent );
        }
    }
}
